using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace RomSoft.Extensions
{
    public static class StreamExtensions
    {
        public static FileContentResult FromZipToFile(this byte[] compressedBytes, string zipName)
        {
            using var compressedFileStream = new MemoryStream();
            //Create an archive and store the stream in memory.
            using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, false))
            {
                //Create a zip entry for each attachment
                var zipEntry = zipArchive.CreateEntry(zipName);

                //Get the stream of the attachment
                using (var originalFileStream = new MemoryStream(compressedBytes))
                using (var zipEntryStream = zipEntry.Open())
                {
                    //Copy the attachment stream to the zip entry stream
                    originalFileStream.CopyTo(zipEntryStream);
                }
            }

            return new FileContentResult(compressedFileStream.ToArray(), "application/zip")
                { FileDownloadName = "Filename.zip" };
        }
    }
}
