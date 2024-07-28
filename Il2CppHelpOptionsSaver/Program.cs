using System.Diagnostics;

namespace Il2CppHelpOptionsSaver;

internal static class Program
{
	static void Main(string[] args)
	{
		string inputDirectory = args[0];
		string outputDirectory = args[1];

		foreach (string unityVersion in Directory.EnumerateDirectories(inputDirectory))
		{
			string outputPath = Path.Combine(outputDirectory, Path.GetFileName(unityVersion) + ".txt");
			if (File.Exists(outputPath))
			{
				continue;
			}

			string il2cppDirectory = Path.Combine(unityVersion, "Editor", "Data", "il2cpp", "build");
			if (!Directory.Exists(il2cppDirectory))
			{
				continue;
			}

			Console.WriteLine($"Processing {unityVersion}...");

			string[] il2CppPaths =
			[
				Path.Combine(il2cppDirectory, "il2cpp.exe"),
				Path.Combine(il2cppDirectory, "deploy", "il2cpp.exe"),
				Path.Combine(il2cppDirectory, "deploy", "net8.0", "il2cpp.exe"),
				Path.Combine(il2cppDirectory, "deploy", "net7.0", "il2cpp.exe"),
				Path.Combine(il2cppDirectory, "deploy", "net6.0", "il2cpp.exe"),
				Path.Combine(il2cppDirectory, "deploy", "net5.0", "il2cpp.exe"),
				Path.Combine(il2cppDirectory, "deploy", "netcoreapp3.1", "il2cpp.exe"),
				Path.Combine(il2cppDirectory, "deploy", "netcoreapp3.0", "il2cpp.exe"),
				Path.Combine(il2cppDirectory, "deploy", "net471", "il2cpp.exe"),
			];

			string il2CppPath = il2CppPaths.First(File.Exists);
			Save(il2CppPath, outputPath);
		}
		Console.WriteLine("Done!");
	}

	static void Save(string inputPath, string outputPath)
	{
		// Specify the process to run
		string processPath = Path.GetFullPath(inputPath);

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
			if (!string.IsNullOrEmpty(output))
			{
				File.WriteAllText(outputPath, output);
			}
		}
	}
}
