using System;
using System.IO.Abstractions;
using Xunit;
using ZipperTests.Helpers;
using Zipper.Context;
using Zipper;

namespace ZipperIntegrationTests
{
    public class Compression_Tests : IDisposable
    {
        private IFileSystem _fileSystem;
        private static string _rawFilePath = @".\test.txt";
        private static string _compressedFilePath = @".\test_compressed.gz";
        private static string _decompressedFilePath = @".\test_decompressed.txt";

        public Compression_Tests()
        {
            _fileSystem = new FileSystem();
        }

        [Fact]
        public void Compress_Decompress_Bytes_Are_Equal()
        {
            var rawGenContent = CreateInputFile();

            Program.Main(new string[] { "compress", _rawFilePath, _compressedFilePath });
          
            Program.Main(new string[] { "decompress", _compressedFilePath, _decompressedFilePath });

            var decompresedContent = ReadUnZippedFile();
            var dataIsEqual = ByteArrayHelper.AreEqual(rawGenContent, decompresedContent);

            Assert.True(dataIsEqual);
        }

        private byte[] CreateInputFile()
        {
            var randomFile = new RandomFile(300, ByteSize.Mega);

            randomFile.Create(_fileSystem, _rawFilePath);
            return randomFile.Content;
        }

        private byte[] ReadUnZippedFile() => _fileSystem.File.ReadAllBytes(_decompressedFilePath);

        private byte[] ReadRawFile() => _fileSystem.File.ReadAllBytes(_rawFilePath);

        public void Dispose()
        {
            _fileSystem.File.Delete(_rawFilePath);
            _fileSystem.File.Delete(_compressedFilePath);
            _fileSystem.File.Delete(_decompressedFilePath);
        }
    }
}
