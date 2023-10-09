using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notes.Models
{
    public class Note
    {
        public int id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public DateTime lastEdited { get; set; }
        public int categoryId { get; set; }
    }
}