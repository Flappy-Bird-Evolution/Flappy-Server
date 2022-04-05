using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class NeuralNetwork : System.IComparable<NeuralNetwork>
{
	[JsonProperty] InputNeuron[] inputs;
	[JsonProperty] HiddenNeuron[] hidden;
	[JsonProperty] OutputNeuron[] outputs;
	float networkThershold;
	public float Fitness { get; set; }

	
	public NeuralNetwork(int i_NumberOfInputs, int i_NumberOfOutputs)
	{
		inputs = new InputNeuron[i_NumberOfInputs];
		outputs = new OutputNeuron[i_NumberOfOutputs];

		int numberOfHiddenNeurons = (i_NumberOfInputs * 2) + 1;
		//int numberOfHiddenNeurons = (i_NumberOfInputs + i_NumberOfOutputs) / 2;
		hidden = new HiddenNeuron[numberOfHiddenNeurons];
		networkThershold = 0.5f;
		InitNeurons();
	}


	private void InitNeurons()
	{
		for (int i = 0; i < inputs.Length; i++)
		{
			inputs[i] = new InputNeuron(hidden.Length);
		}

		for (int i = 0; i < hidden.Length; i++)
		{
			hidden[i] = new HiddenNeuron(outputs.Length);
		}

		for(int i = 0; i < outputs.Length; i++)
		{
			outputs[i] = new OutputNeuron();
		}


	}

	public void  FeedForward(float[] i_Inputs)
	{
		FeedInput(i_Inputs);
		FeedInputToHiddens();
		FeedHiddenToOutput();
	}

	public void FeedInput(params float[] i_Inputs)
	{
		for(int i = 0; i < inputs.Length; i++)
		{
			inputs[i].RecieveInput(i_Inputs[i]);
		}
	}


	public void FeedInputToHiddens()
	{
		foreach(InputNeuron input in inputs)
		{
			input.FeedForward(hidden);
			input.Reset();
		}

		foreach(HiddenNeuron hid in hidden)
		{
			hid.Activate(networkThershold);
		}
	}

	public void FeedHiddenToOutput()
	{
		foreach(HiddenNeuron hid in hidden)
		{
			hid.FeedForward(outputs);
			hid.Reset();
		}

		foreach(OutputNeuron output in outputs)
		{
			output.Activate(networkThershold);
			output.Reset();
		}
	}
	
	public void MutateNetwork()
	{

		int randomIndex = Utils.RandomRange(0, hidden.Length);
		foreach(Neuron n in inputs)
		{
			n.MutateWeight(randomIndex);
		}

		randomIndex = Utils.RandomRange(0, hidden.Length);
		foreach (Neuron n in inputs)
		{
			n.MutateWeight(randomIndex);
		}





		//System.Random rand = new System.Random();
		//int numberOfNeurons = (int) (inputs.Length + hidden.Length * outputs.Length * 0.05f);

		//List<Neuron> neurons = new List<Neuron>();
		//for(int i = 0; i < numberOfNeurons/2; i++)
		//{
		//	Neuron n = inputs[rand.Next(inputs.Length)];
		//	if (!neurons.Contains(n))
		//		neurons.Add(n);
		//}

		//for(int i = 0; i < hidden.Length; i++)
		//{
		//	Neuron n = hidden[rand.Next(hidden.Length)];
		//	if (!neurons.Contains(n))
		//		neurons.Add(n);
		//}

		//foreach(Neuron n in neurons)
		//{
		//	n.Mutate();
		//}
	}

	public int CompareTo(NeuralNetwork other)
	{
		return (int) (Fitness - other.Fitness);
	}
}