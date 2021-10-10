namespace RentItAPI.Models
{
    public class CreateItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string ImageThumbnail { get; set; }
        public bool RequestRequired { get; set; }
    }
}
