using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LibraryData.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<FileUpload> ImageUploads { get; set; }

    }
}
