using System.Collections;
using System.IO.Compression;
using ArchiverWebApi.Services.Contracts;

namespace ArchiverWebApi.Services
{
    public class ArchiverService : IArchiverService
    {
        public bool TryArchive(byte[] content, out byte[] archive, string modelConnectionId, string filename)
        {
            try
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                using var ms = new MemoryStream();
                using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    var entry = zipArchive.CreateEntry(filename, CompressionLevel.Fastest);
                    using (var entryStream = entry.Open())
                    {
                        entryStream.Write(content, 0, content.Length);
                    }

                }
                archive = ms.ToArray();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                archive = new byte[]{};
                return false;
            }

            //possible solution to the zip content. Have to investigate further
            //using var compressedFileStream = new MemoryStream();
            ////Create an archive and store the stream in memory.
            //using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
            //{
            //    var zipEntry = zipArchive.CreateEntry("file.txt");

            //    //Get the stream of the attachment
            //    using (var originalFileStream = new MemoryStream(content))
            //    {
            //        using (var zipEntryStream = zipEntry.Open())
            //        {
            //            //Copy the attachment stream to the zip entry stream
            //            originalFileStream.CopyTo(zipEntryStream);
            //        }
            //    }

            //}

            //archive = compressedFileStream.ToArray();
        }
    }
}
