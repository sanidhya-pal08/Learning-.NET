namespace NZWalks.API.Models.DTOs
{
    public class RegionDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }  //makes it optional , can be null
    }
    
}
