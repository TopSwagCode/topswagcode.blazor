
namespace TopSwagCode.Blazor.Server.Services
{
    public interface IComputerVisionService
    {
        Task<string> FaceRec(byte[] imageData);
    }
}