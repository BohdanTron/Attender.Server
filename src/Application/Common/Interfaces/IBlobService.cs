using Attender.Server.Application.Common.Models;
using System.IO;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface IBlobService
    {
        Task<BlobInfo> UploadAvatar(string contentType, Stream content);
    }
}
