public class InputNeuron : Neuron
{

	public InputNeuron(int i_SizeOfNextLayer) : base(i_SizeOfNextLayer)
	{

	}

	public override InputNeuron Clone()
	{
		InputNeuron n = (InputNeuron) base.Clone();
		return n;
	}

}
