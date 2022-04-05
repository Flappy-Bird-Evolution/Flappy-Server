namespace ConsoleTCPServer
{
	class Program
	{
		static void Main(string[] args)
		{
			InitialPopulation.Log();
			Server server = new Server();
			server.Run();
		}
	}
}
