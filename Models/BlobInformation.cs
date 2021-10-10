using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
