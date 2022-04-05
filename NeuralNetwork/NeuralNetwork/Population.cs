using System;
using Newtonsoft.Json;
using System.Collections.Generic;

public class Population
{
	List<NeuralNetwork> elements;
	public float mutationRate;
	int m_PopulationSize;

	public List<NeuralNetwork> Elements
	{
		get { return elements; }
		private set { elements = value; }
	}

	public Population(float i_MutationRate, int i_PopulationSize, int i_Inputs, int i_Outputs)
	{
		mutationRate = i_MutationRate;
		m_PopulationSize = i_PopulationSize;
		initPopulation(i_Inputs, i_Outputs);
	}

	public Population(NeuralNetwork[] networks, float i_MutationRate)
	{
		Elements = new List<NeuralNetwork>(networks);
		m_PopulationSize = Elements.Count;
		mutationRate = i_MutationRate;
	}

	private void initPopulation(int i_Inputs, int i_Outputs)
	{
		Elements = new List<NeuralNetwork>();
		for(int i = 0; i < m_PopulationSize; i++)
		{
			NeuralNetwork n = new NeuralNetwork(i_Inputs, i_Outputs);
			Elements.Add(n);
		}
	}

	public int Size()
	{
		return Elements.Count;
	}

	public void ApplyGeneticOperators()
	{
		NeuralNetwork[] arrayElements = Elements.ToArray();
		arrayElements = GeneticOperators.Selection(arrayElements);
		arrayElements = GeneticOperators.CrossOver(arrayElements);
		arrayElements = GeneticOperators.Mutation(arrayElements);
		
		Elements = new List<NeuralNetwork>(arrayElements);
		ResetFitness();
	}
	private void ResetFitness()
	{
		foreach(NeuralNetwork n in Elements)
		{
			n.Fitness = 0;
		}
	}

	public string[] SerializeAll()
	{ 
		List<string> list = new List<string>();

		foreach(NeuralNetwork n in Elements)
		{
			string json = JsonConvert.SerializeObject(n, Formatting.Indented);
			list.Add(json);
		}

		return list.ToArray();
	}
}