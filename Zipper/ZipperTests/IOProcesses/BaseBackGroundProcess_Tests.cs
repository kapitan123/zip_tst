using System.IO.Abstractions.TestingHelpers;
using Zipper.Context;
using System;
using ZipperTests.Helpers;
using ZipperTests.Mocks;

namespace ZipperTests.IOProcesses
{
    public abstract class BaseBackGroundProcess_Tests
    {
        private Lazy<MockFileSystem> _lazyfileSystemMock = new Lazy<MockFileSystem>(() => new MockFileSystem());
        protected MockFileSystem FileSystemMock => _lazyfileSystemMock.Value;

        protected virtual IConfiguration Config => new ZipConfigMock();

        private Lazy<RandomFile> _genfile = new Lazy<RandomFile>(() => new RandomFile(1, ByteSize.Mega));
        protected virtual RandomFile GenFile => _genfile.Value;

        protected virtual string DefaultPath => @"C:\testingFile.txt";
    }
}
