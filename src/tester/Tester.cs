using System;
using System.Diagnostics;
using System.IO;

namespace HappyCspp.Tester
{
    public class App
    {
        static readonly bool isWindows = Path.DirectorySeparatorChar == '\\';

        static void PrintUsage()
        {
            Console.WriteLine("Usage: tester abc.cs test_run_folder cc.json");        
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
            // Run tests with:
            // tester abc.cs test_run_folder cc.json

            // Implementation
            // 1. Copy abc.cs to test_run_folder/abc
            // 2. Create the common project.json file to test_run_folder/abc
            // 3. Compile abc.cs by cspp test_run_folder/abc/project.json cc_current_platform.json
            // 4. Run the compiled native program and expect exit code 0

            if (args.Length != 3)
            {
                PrintUsage();
                return;
            }

            string testCaseFile = args[0];
            string testRunFolder = args[1];
            string cc = args[2];

            if (!File.Exists(testCaseFile))
            {
                PrintUsage();
                return;
            }

            if (!Directory.Exists(testRunFolder))
            {
                PrintUsage();
                return;
            }

            if (!File.Exists(cc))
            {
                PrintUsage();
                return;
            }

            string testCaseName = Path.GetFileNameWithoutExtension(testCaseFile);
            string testCaseFolder = Path.Combine(testRunFolder, testCaseName);
            string testCaseProject = Path.Combine(testCaseFolder, "project.json");
            string testCaseExecutable = Path.Combine(testCaseFolder, "Debug", testCaseName + (isWindows ? ".exe" : null));

            if (Directory.Exists(testCaseFolder))
            {
                Directory.Delete(testCaseFolder, true);
            }

            Directory.CreateDirectory(testCaseFolder);

            File.Copy(testCaseFile, Path.Combine(testCaseFolder, testCaseName + ".cs"));
            File.WriteAllText(testCaseProject, @"
{
  'version': '1.0.0-*',
  'buildOptions': {
    'debugType': 'portable'
  },

  'dependencies': {
    'Microsoft.NETCore.Runtime.CoreCLR': '*'
  },

  'frameworks': {
    'netstandard1.6.1': {
      'bin': { 
          'assembly': '../../../corelib.dll'
      }
    }
  },

  'cspp': {
    'emitEntryPoint': true
  }
}
".Replace('\'', '"'));

            RunCommand("cspp", testCaseProject + " " + cc);
            RunCommand(testCaseExecutable);
        }
    }
}
