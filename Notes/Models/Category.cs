using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notes.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}