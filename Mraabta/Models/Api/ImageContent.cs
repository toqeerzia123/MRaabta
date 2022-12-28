using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models.Api
{
    public class ImageContent 
    {
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public string Name { get; set; }
    }

    public class ImageContentList
    {
        public ImageContentList()
        {
            Images = new List<ImageContent>();
        }
        public List<ImageContent> Images { get; set; }
    }

    public class CategoryPostModel : ImageContentList
    {
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}