namespace Zipper.Context
{
    public interface IConfiguration
    {
        int ThreadsCount { get; }

        int ChunkSize { get; }
    }
}
