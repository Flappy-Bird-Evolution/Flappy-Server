using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ConsoleTCPServer
{
	class Server
	{
		public Server()
		{
			//path = GetPathToCurrentFolder();
		}

		private void initPopulation(NetworkStream stream)
		{
			Population pop = new Population(0.1f, 100, 5, 1);
			sendInitialResponse(pop, stream);
		}
		public void Run()
		{
			IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
			TcpListener listener = new TcpListener(ipAddress, 1234);
			listener.Start();
			bool done = false;
			
			while (!done)
			{
				Console.WriteLine("Recieving..");
				TcpClient client = listener.AcceptTcpClient();
				NetworkStream stream = client.GetStream();

				//Check if this is a request for initialization
				//Of a population
				int initial = NetworkUtils.ReadInt(stream);
				if (initial == 0) 
				{	
					initPopulation(stream);
					continue;
				}

				Population population = ParsePopulation(stream);
				Console.WriteLine($"Recived {population.Size()} networks");
				Console.WriteLine($"Processed population");

				//StorePopulation(population);
				//Console.WriteLine("Stored population in " + path);

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
			List<NeuralNetwork> list = new List<NeuralNetwork>();

			int numberOfElements = NetworkUtils.ReadInt(stream);

			for (int i = 0; i < numberOfElements; i++)
			{
				NeuralNetwork n = ProcessIndividual(stream);
				list.Add(n);
			}
			return new Population(list.ToArray(), 0.01f);
		}


		private void sendResponse(Population pop, NetworkStream stream)
		{
			NetworkUtils.WriteInt(stream, pop.Size());
			Console.WriteLine($"population size: {pop.Size()}");
			string[] stringReps = pop.SerializeAll();
			foreach(NeuralNetwork p in pop.Pop)
			{
				NetworkUtils.WriteNN(stream, p);
			}

			stream.Close();
		}		
		
		private void sendInitialResponse(Population pop, NetworkStream stream)
		{
			NetworkUtils.WriteInt(stream, pop.Size());
			string[] stringReps = pop.SerializeAll();
			foreach(NeuralNetwork p in pop.Pop)
			{
				NetworkUtils.WriteNN(stream, p);
			}
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


	}
}
