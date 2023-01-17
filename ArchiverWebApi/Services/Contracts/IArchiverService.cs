using System.IO.Compression;

namespace ArchiverWebApi.Services.Contracts
{
    public interface IArchiverService
    {
        bool TryArchive(byte[] content, out byte[] archive, string modelConnectionId);
    }
}
