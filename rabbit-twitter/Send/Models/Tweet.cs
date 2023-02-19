using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Send.Models
{
    public class Tweet
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Text { get; set; }
        public DateTime DataPostagem { get; set; } = DateTime.Now;
    }
}