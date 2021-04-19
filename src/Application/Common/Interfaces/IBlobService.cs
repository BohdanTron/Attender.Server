using System.IO;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface IBlobService
    {
        Task<string> UploadAvatar(string contentType, Stream content);
    }
}
