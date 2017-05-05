using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; }
    }
}