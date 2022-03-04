using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebEnterprise.Models
{
    public class Comment
    {
        [Key]
        public int IdCommment { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public int Like { get; set; } = 0;

        [Required(ErrorMessage = "Please enter Content")]
        public string Content { get; set; }
        public int IdeaID { get; set; }
        public Idea Idea { get; set; }


    }
}
