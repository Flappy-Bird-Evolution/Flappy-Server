﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ConsoleTCPServer
{
	class Server
	{
		string path;
		public Server()
		{
			path = GetPathToCurrentFolder();
		}

		private void distributeInitialPopulation()
		{
			IPAddress ipAddress = IPAddress.Parse("10.100.102.8");
			TcpListener listener = new TcpListener(ipAddress, 1234);
			listener.Start();

			Console.WriteLine("Recieving initial client request..");
			TcpClient client = listener.AcceptTcpClient();
			NetworkStream stream = client.GetStream();

			int ack = NetworkUtils.ReadInt(stream);
			Population pop = new Population(0.01f, 100, 4, 1);
		}
		public void Run()
		{
			IPAddress ipAddress = IPAddress.Parse("10.100.102.8");
			TcpListener listener = new TcpListener(ipAddress, 1234);
			listener.Start();
			bool done = false;
			
			while (!done)
			{
				Console.WriteLine("Recieving..");
				TcpClient client = listener.AcceptTcpClient();
				NetworkStream stream = client.GetStream();
				
				Population population = ParsePopulation(stream);
				Console.WriteLine($"Recived {population.Size()} networks");
				Console.WriteLine($"Processed population");

				StorePopulation(population);
				Console.WriteLine("Stored population in " + path);

				ApplyGeneticOperators(population);

				sendResponse(population, stream);
				Console.WriteLine("Sent Response\n\n");
			}
		}

		private Population ApplyGeneticOperators(Population population)
		{
			population.ApplyGeneticOperators();
			return population;
		}

		private Population ParsePopulation(NetworkStream stream)
		{
			//try
			{
				List<NeuralNetwork> list = new List<NeuralNetwork>();

				float mutationRate = NetworkUtils.ReadFloat(stream);
				int numberOfElements = NetworkUtils.ReadInt(stream);

				for (int i = 0; i < numberOfElements; i++)
				{
					NeuralNetwork n = ProcessIndividual(stream);
					list.Add(n);
				}
				return new Population(list.ToArray(), mutationRate);
			}

			//catch (Exception e)
			//{
			//	Console.WriteLine(e);
			//	mutationRate = 0;
			//	return null;
			//}
		}


		private void sendResponse(Population pop, NetworkStream stream)
		{
			NetworkUtils.WriteFloat(stream, pop.mutationRate);
			NetworkUtils.WriteInt(stream, pop.Size());

			string[] stringReps = pop.SerializeAll();

			foreach(NeuralNetwork p in pop.Pop)
			{
				NetworkUtils.WriteNN(stream, p);
			}

			stream.Close();
		}

		private Population processPopulation(float mutationRate, NeuralNetwork[] networks)
		{
			Population pop = new Population(networks, mutationRate);
			pop.ApplyGeneticOperators();
			Console.WriteLine("Applied genetic operators");
			return pop;
		}

		public NeuralNetwork ProcessIndividual(NetworkStream stream)
		{
			int length = NetworkUtils.ReadInt(stream);
			NeuralNetwork network = NetworkUtils.ReadNN(stream, length);

			return network;

		}

		public void StorePopulation(Population pop)
		{
			GenerateMetaData(pop);

			string[] elements = pop.SerializeAll();
			int num = 1;
			foreach (string e in elements)
			{
				string fileName = "network " + (num++) + ".json";
				string fullPath = Path.Combine(path, fileName);
				StreamWriter write = new StreamWriter(fullPath);
				write.Write(e);
				write.Close();
			}

		}

		public void GenerateMetaData(Population pop)
		{
			float highestFitness = pop.GetFittest().Fitness;
			float averageFitness = 0;

			string fileName = "Population data.txt";
			string date = DateTime.Now.ToString();

			foreach (NeuralNetwork n in pop.Pop)
			{
				averageFitness += n.Fitness;
			}

			averageFitness /= pop.Size();
			string fullPath = Path.Combine(path, fileName);
			StreamWriter write = new StreamWriter(fullPath);
			write.Write("Date: " + date + "\n");
			write.Write("Elements in generation: " + pop.Size()+ "\n");
			write.Write("Highest Fitness: " + highestFitness + "\n");
			write.Write("Average Fitness: " + averageFitness + "\n");
			write.Close();
		}

		private static string GetPathToCurrentFolder()
		{
			string path = @"C:\Users\Guy\Documents\NeuralYou Data\";

			string[] files = Directory.GetDirectories(path);
			bool todayExists = false;
			string today = DateTime.Today.ToShortDateString();
			today = today.Replace("/", ".");
			string s = Directory.GetDirectories(path).SingleOrDefault(file => file.Contains(today));

			if (s is null)
				s = Directory.CreateDirectory(path + @"\" + today).FullName;

			int amountOfFiles = Directory.GetDirectories(s).Length;

			string pathToCurrent = Directory.CreateDirectory(s + @"\" + amountOfFiles).FullName;

			return pathToCurrent;
		}
	}
}