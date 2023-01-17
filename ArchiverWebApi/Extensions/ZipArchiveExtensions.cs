using System.IO.Compression;

namespace ArchiverWebApi.Extensions
{
    public static class ZipArchiveExtensions
    {
        public static ZipArchiveEntry Add(this ZipArchive archive, byte[] fileContent, string filename)
        {
            var entry = archive.CreateEntry(filename, CompressionLevel.Fastest);
            using var stream = entry.Open();
            stream.Write(fileContent, 0, fileContent.Length);
            return entry;
        }
    }
}
