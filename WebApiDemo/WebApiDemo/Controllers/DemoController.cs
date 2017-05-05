using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    public class DemoController : ApiController
    {
        [HttpGet]
        public IList<Site> SiteList(int startId, int itemcount)
        {
            var sites = new List<Site>();
            sites.Add(new Site { SiteId = 1, Title = "test", Uri = "www.cnblogs.cc" });
            sites.Add(new Site { SiteId = 2, Title = "博客园首页", Uri = "www.cnblogs.com" });
            sites.Add(new Site { SiteId = 3, Title = "博问", Uri = "q.cnblogs.com" });
            sites.Add(new Site { SiteId = 4, Title = "新闻", Uri = "news.cnblogs.com" });
            sites.Add(new Site { SiteId = 5, Title = "招聘", Uri = "job.cnblogs.com" });

            var result = (from Site site in sites
                          where site.SiteId > startId
                          select site)
                            .Take(itemcount)
                            .ToList();
            return result;
        }
    }
}