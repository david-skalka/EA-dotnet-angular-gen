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
        private readonly bool _force;

        private readonly string _path;
        private readonly object _template;

        public T4GeneratorCommand(object template, string path, bool force)
        {
            this._template = template;
            this._path = path;
            this._force = force;
        }


        public void Execute()
        {
            if (CanWrite())
            {
                string result = ((dynamic)_template).TransformText();
                File.WriteAllText(_path, result);
            }
        }

        private bool CanWrite()
        {
            if (!File.Exists(_path)) return true;

            if (_force) return true;

            var confirm = Prompt.Confirm("File " + _path + " already exists. Do you want to overwrite it? (y/n)", false);


            if (confirm) return true;


            return false;
        }
    }


    public class ShellGeneratorCommand : IGeneratorCommand
    {
        private readonly string _cwd;
        private readonly string _args;
        private readonly string _filename;

        public ShellGeneratorCommand(string filename, string args, string cwd)
        {
            this._filename = filename;
            this._args = args;
            this._cwd = cwd;
        }

        public void Execute()
        {
            var process = new Process();
            process.StartInfo.FileName = _filename;
            process.StartInfo.Arguments = _args;
            process.StartInfo.WorkingDirectory = _cwd;
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0) throw new Exception("Error executing command " + _filename + " " + _args);
        }
    }




    public class MkdirGeneratorCommand : IGeneratorCommand
    {
        private readonly string _path;

        public MkdirGeneratorCommand(string path)
        {
            this._path = path;
        }

        public void Execute()
        {
            Directory.CreateDirectory(_path);
        }
    }


    public class JsonCommand : IGeneratorCommand
    {
        private readonly Func<dynamic, dynamic> _func;
        private readonly string _path;

        public JsonCommand(string path, Func<dynamic, dynamic> func)
        {
            this._path = path;
            this._func = func;
        }

        public void Execute()
        {
            var des = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(_path));

            var output = _func(des);

            File.WriteAllText(_path, JsonConvert.SerializeObject(output, Formatting.Indented));
        }
    }


    public class MultiCommand : IGeneratorCommand
    {
        private readonly IGeneratorCommand[] _commands;

        public MultiCommand(params IGeneratorCommand[] commands)
        {
            this._commands = commands;
        }

        public void Execute()
        {
            foreach (var command in _commands)
            {
                command.Execute();
            }
        }
    }
}