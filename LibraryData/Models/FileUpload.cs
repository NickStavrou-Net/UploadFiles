using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LibraryData.Models
{
    public class FileUpload
    {
        [NotMapped]
        public List<IFormFile> Images { get; set; }
    }
}
