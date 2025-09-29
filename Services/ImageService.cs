using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System; // Adicione esta using

namespace SuppGamesBack.Services
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public ImageService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]);
            
            _cloudinary = new Cloudinary(account);
        }

        public string TransformUrl(string imageUrl, int width, int height)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                // Retorna uma URL de placeholder se a imagem for nula ou vazia
                return "https://via.placeholder.com/400x300.png?text=No+Image";
            }

            // --- A CORREÇÃO PRINCIPAL ESTÁ AQUI ---
            // Garante que a URL seja sempre absoluta
            string fullUrl = imageUrl;
            if (imageUrl.StartsWith("/media/"))
            {
                fullUrl = $"https://media.rawg.io{imageUrl}";
            }
            // --- FIM DA CORREÇÃO ---

            var transformation = new Transformation()
                .Width(width).Height(height).Crop("fill").Gravity("auto")
                .FetchFormat("auto").Quality("auto");
            
            var transformedUrl = _cloudinary.Api.UrlImgUp.Transform(transformation)
                                                        .Type("fetch")
                                                        .BuildUrl(fullUrl); // Usa a URL completa

            return $"{transformedUrl}?v={DateTime.UtcNow.Ticks}";
        }
    }
}