using System.Diagnostics;

namespace HelpOptionsSaver;

internal class Program
{
	static void Main(string[] args)
	{
		// Specify the process to run
		string processPath = Path.GetFullPath(args[0]);

		// Specify the output file path
		string outputFilePath = args[1];

		// Create a new process start info
		ProcessStartInfo psi = new ProcessStartInfo
		{
			FileName = processPath,
			RedirectStandardOutput = true,
			UseShellExecute = false,
			CreateNoWindow = true,
			Arguments = "--help",
		};

		// Create and start the process
		using (Process process = Process.Start(psi) ?? throw new NullReferenceException("Could not start process"))
		{
			// Read the output
			string output = process.StandardOutput.ReadToEnd().Replace("\r", null).Trim();

			// Wait for the process to exit
			process.WaitForExit();

			// Write the output to a file
			File.WriteAllText(outputFilePath, output);
		}

		Console.WriteLine($"Process output has been saved to: {outputFilePath}");
	}
}
