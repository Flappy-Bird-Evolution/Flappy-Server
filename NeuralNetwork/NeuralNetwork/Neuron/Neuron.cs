using Newtonsoft.Json;
using System;

public abstract class Neuron
{
	protected float currentValue;
	[JsonProperty] protected float[] outputWeights;
	protected bool active;

	public Neuron(int nextLayerLength)
	{
		outputWeights = new float[nextLayerLength];

		for(int i = 0; i < outputWeights.Length; i++)
		{
			outputWeights[i] = Utils.RandomRange(-1f, 1f);
		}
	}
	public virtual void RecieveInput(float i_InputVal)
	{
		currentValue += i_InputVal;
	}

	public virtual void Activate(float i_Threshold)
	{
		currentValue = (float)Math.Tanh(currentValue);
		
		if(currentValue > i_Threshold) { active = true; }
		else { active = false; }
	}

	public void Reset()
	{
		currentValue = 0;
	}

	public void FeedForward(Neuron[] i_NextLayerNeurons)
	{
		for(int i = 0; i < i_NextLayerNeurons.Length; i++)
		{
			i_NextLayerNeurons[i].RecieveInput(currentValue * outputWeights[i]);
		}
	}

	public void Mutate()
	{
		for (int i = 0; i < outputWeights.Length; i++)
		{
			outputWeights[i] += Utils.RandomRange(-10f, 10f);
		}
	}

	public void MutateWeight(int index)
	{
		outputWeights[index] += Utils.RandomRange(-10f, 10f);

	}
}
