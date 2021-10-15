using System.IO;

namespace RentItAPI.Models
{
    public class BlobInformation
    {
        public BlobInformation(Stream Content, string ContentType)
        {
            this.Content = Content;
            this.ContentType = ContentType;
        }

        public Stream Content { get; set; }
        public string ContentType { get; set; }
    }
}