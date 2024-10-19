using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Sharprompt;

namespace EADotnetAngularGen
{
    public interface IGeneratorCommand
    {
        void Execute();
    }


    public class T4GeneratorCommand : IGeneratorCommand
    {
        private readonly bool force;

        public string path;
        public object template;

        public T4GeneratorCommand(object template, string path, bool force)
        {
            this.template = template;
            this.path = path;
            this.force = force;
        }


        public void Execute()
        {
            if (canWrite())
            {
                string result = ((dynamic)template).TransformText();
                File.WriteAllText(path, result);
            }
        }

        private bool canWrite()
        {
            if (!File.Exists(path)) return true;

            if (force) return true;

            var confirm = Prompt.Confirm("File " + path + " already exists. Do you want to overwrite it? (y/n)", false);


            if (confirm) return true;


            return false;
        }
    }


    public class ShellGeneratorCommand : IGeneratorCommand
    {
        private readonly string cwd;
        private readonly string args;
        private readonly string filename;

        public ShellGeneratorCommand(string filename, string args, string cwd)
        {
            this.filename = filename;
            this.args = args;
            this.cwd = cwd;
        }

        public void Execute()
        {
            var process = new Process();
            process.StartInfo.FileName = filename;
            process.StartInfo.Arguments = args;
            process.StartInfo.WorkingDirectory = cwd;
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0) throw new Exception("Error executing command " + filename + " " + args);
        }
    }


    public class RmGeneratorCommand : IGeneratorCommand
    {
        private readonly string directoryPath;
        private readonly string searchPattern;

        public RmGeneratorCommand(string directoryPath, string searchPattern)
        {
            this.directoryPath = directoryPath;
            this.searchPattern = searchPattern;
        }

        public void Execute()
        {
            var files = Directory.GetFiles(directoryPath, searchPattern);

            foreach (var item in files) File.Delete(item);
        }
    }


    public class RmDirGeneratorCommand : IGeneratorCommand
    {
        private readonly string directoryPath;

        public RmDirGeneratorCommand(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public void Execute()
        {
            Directory.Delete(directoryPath, true);
        }
    }


    public class MkdirGeneratorCommand : IGeneratorCommand
    {
        private readonly string path;

        public MkdirGeneratorCommand(string path)
        {
            this.path = path;
        }

        public void Execute()
        {
            Directory.CreateDirectory(path);
        }
    }


    public class JsonCommand : IGeneratorCommand
    {
        private readonly Func<dynamic, dynamic> func;
        private readonly string path;

        public JsonCommand(string path, Func<dynamic, dynamic> func)
        {
            this.path = path;
            this.func = func;
        }

        public void Execute()
        {
            var des = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(path));

            var output = func(des);

            File.WriteAllText(path, JsonConvert.SerializeObject(output, Formatting.Indented));
        }
    }
}