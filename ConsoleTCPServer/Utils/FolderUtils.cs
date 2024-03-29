﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyServer.Utils
{

	class FolderUtils
	{
		string path;

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
			float highestFitness = 0;//pop.GetFittest().Fitness;
			float averageFitness = 0;

			string fileName = "Population data.txt";
			string date = DateTime.Now.ToString();

			foreach (NeuralNetwork n in pop.Elements)
			{
				averageFitness += n.Fitness;
			}

			averageFitness /= pop.Size();
			string fullPath = Path.Combine(path, fileName);
			StreamWriter write = new StreamWriter(fullPath);
			write.Write("Date: " + date + "\n");
			write.Write("Elements in generation: " + pop.Size() + "\n");
			write.Write("Highest Fitness: " + highestFitness + "\n");
			write.Write("Average Fitness: " + averageFitness + "\n");
			write.Close();
		}

		private static string GetPathToCurrentFolder()
		{
			string path = @"C:\Users\Guy\Documents\NeuralYou Data\";

			string[] files = Directory.GetDirectories(path);
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
