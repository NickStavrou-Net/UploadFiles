using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace LibraryData.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        public string Title { get; set; }

        //[Required]
        public string Description { get; set; }

        //[Required]
        [NotMapped]
        public List<string> ImageFilePaths { get; set; }

    }


}
