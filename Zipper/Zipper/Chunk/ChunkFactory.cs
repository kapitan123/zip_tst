namespace Zipper.Chunk
{
    public static class ChunkFactory
    {
        public static IChunk GetNew(byte[] content, int number) => new FileChunk(content, number);

        public static IChunk GetNull() => new NullChunk();
    }
}
