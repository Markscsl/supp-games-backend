namespace SuppGamesBack.Services
{
    public interface IImageService
    {
        string TransformUrl(string imageUrl, int width, int height);
    }
}
