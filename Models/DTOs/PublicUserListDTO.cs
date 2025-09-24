namespace SuppGamesBack.Models.DTOs
{
    public class PublicUserListDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<PublicGameInfoDTO> FavoriteGames { get; set; } = new List<PublicGameInfoDTO>();
    }
}
