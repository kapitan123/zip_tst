using System;
using System.IO.Abstractions;
using Zipper.Context;

namespace ZipperTests.Helpers
{
    public class RandomFile
    {
        public string Path { get; private set; }

        private byte[] _lazyContent;
        public byte[] Content
        {
            get
            {
                if(_lazyContent == null)
                {
                    _lazyContent = GenerateContent();
                }
                return _lazyContent;
            }
        }

        public int Length { get; }

        public RandomFile(int length, int multiplier)
        {
            Length = length * multiplier;
        }

        public byte[] GenerateContent()
        {
            var data = new byte[Length];
            var rng = new Random();
            rng.NextBytes(data);
            return data;
        }

        public void Create(IFileSystem _fileSystem, string filePath)
        {
            Path = filePath;
            _fileSystem.File.WriteAllBytes(filePath, Content);
        }
    }
}
