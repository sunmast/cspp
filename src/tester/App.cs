using System;
using System.Diagnostics;
using System.IO;

namespace HappyCspp.Tester
{
    public class App
    {
        static void PrintUsage()
        {
            Console.WriteLine("Usage: tester abc.cs test_run_folder");        
        }

        static int RunCommand(string program, string arguments = null)
        {
            Console.WriteLine("{0} {1}", program, arguments);

            Process p = Process.Start(program, arguments);
            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                throw new Exception(string.Format("{0} {1} failed with exit code {2}.", program, arguments, p.ExitCode));
            }

            return p.ExitCode;
        }

        public static void Main(string[] args)
        {
            // Assuming each .cs file in ../tests folder is a test case
            // Run tests with:
            // tester abc.cs test_run_folder

            // Implementation
            // 1. Copy abc.cs to test_run_folder/abc
            // 2. Create the common project.json file to test_run_folder/abc
            // 3. Compile abc.cs by cspp test_run_folder/abc/project.json cc_current_platform.json
            // 4. Run the compiled native program and expect exit code 0

            if (args.Length != 2)
            {
                PrintUsage();
            }

            string testCaseFile = args[0];
            string testRunFolder = args[1];

            if (!File.Exists(testCaseFile))
            {
                PrintUsage();
            }

            if (!Directory.Exists(testRunFolder))
            {
                PrintUsage();
            }

            string testCaseName = Path.GetFileNameWithoutExtension(testCaseFile);
            string testCaseFolder = Path.Combine(testRunFolder, testCaseName);
            string testCaseProject = Path.Combine(testCaseFolder, "project.json");
            string testCaseExecutable = Path.Combine(testCaseFolder, "Debug", testCaseName);

            Directory.CreateDirectory(testCaseFolder);

            File.Copy(testCaseFile, Path.Combine(testCaseFolder, testCaseName + ".cs"));
            File.WriteAllText(testCaseProject, @"
{
  'version': '1.0.0-*',
  'buildOptions': {
    'debugType': 'portable',
    'emitEntryPoint': true
  },
  'dependencies': {
    'corelib': '1.0.0-*'
  },
  'frameworks': {
    'netcoreapp1.0': {
      'dependencies': {
        'Microsoft.NETCore.Runtime.CoreCLR': {
          'type': 'platform',
          'version': '1.0.1'
        }
      }
    }
  }
}".Replace('\'', '"'));

            RunCommand("cspp", testCaseProject + "cc_gcc.json");
            RunCommand(testCaseExecutable);
        }
    }
}
