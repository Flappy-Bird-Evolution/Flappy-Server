using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class NeuralNetwork : System.IComparable<NeuralNetwork>
{
	[JsonProperty] public InputNeuron[] inputs;
	[JsonProperty] public HiddenNeuron[] hidden;
	[JsonProperty] public OutputNeuron[] outputs;
	private float networkThershold;
	public float Fitness { get; set; }
	
	public NeuralNetwork(int i_NumberOfInputs, int i_NumberOfOutputs)
	{
		inputs = new InputNeuron[i_NumberOfInputs];
		outputs = new OutputNeuron[i_NumberOfOutputs];

		int numberOfHiddenNeurons = (i_NumberOfInputs * 2) + 1;
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

	public NeuralNetwork SelfReplicate()
	{
		NeuralNetwork clone = new NeuralNetwork(5,1);
		clone.inputs = (InputNeuron[]) inputs.Clone();
		clone.hidden = (HiddenNeuron[]) hidden.Clone();
		clone.outputs = (OutputNeuron[]) outputs.Clone();
		clone.Fitness = Fitness;

		return clone;
	}
	public int CompareTo(NeuralNetwork other)
	{
		return (int) (Fitness - other.Fitness);
	}
}