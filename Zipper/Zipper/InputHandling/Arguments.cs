using System;
using System.IO;
using System.Linq;

namespace Zipper.InputHandling
{
    public class Arguments
    {
        private string _inputFilePath;
        public string InputFilePath
        {
            get => _inputFilePath;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(_inputFilePath));
            
                _inputFilePath = value;
            }
        }

        private string _outputFilePath;
        public string OutputFilePath
        {
            get => _outputFilePath;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(_outputFilePath));

                _outputFilePath = value;
            }
        }

        private static string[] _commandList = new string[] { "compress", "decompress" };
        private string _command;
        public string Command
        {
            get => _command;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(_command));
                if (!_commandList.Contains(value)) throw new ArgumentOutOfRangeException(nameof(_command));

                _command = value;
            }
        }

        public static Arguments Parse(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("One of the arguments is missing");
            }

            var inputFilePath = Path.GetFullPath(args[1]);
            var outputFilePath = Path.GetFullPath(args[2]);

            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException($"{inputFilePath} was not found");
            }
            if (File.Exists(outputFilePath))
            {
                throw new ArgumentException($"{outputFilePath} already exist");
            }

            return new Arguments
            {
                InputFilePath = inputFilePath,
                OutputFilePath = outputFilePath,
                Command = args[0]
            };
        }
    }
}
