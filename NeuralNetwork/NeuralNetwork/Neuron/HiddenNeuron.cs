using Newtonsoft.Json;


[System.Serializable]
public class HiddenNeuron: Neuron
{
	//[JsonProperty] float[] outputWeights;
	[JsonIgnore] public float CurrentValue { get; private set; }

	//For construction of an input\hidden neuron.
	public HiddenNeuron(int i_NextLayerSize) : base(i_NextLayerSize)
	{
		
	}

	public void FeedForward(OutputNeuron[] i_OutputLayer)
	{
		for(int i = 0; i < i_OutputLayer.Length; i++)
		{
			i_OutputLayer[i].RecieveInput(outputWeights[i] * CurrentValue);
		}
	}
}
