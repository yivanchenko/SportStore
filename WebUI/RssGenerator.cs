using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using DomainModel.Entities;


namespace WebUI
{
    public static class RssGenerator
    {
        public static string GetRss(IQueryable<Product> products, Uri currentUrl)
        {
            var entries = from t in products.AsEnumerable()
                          select
                          new XElement("item",
                              new XElement("title", t.Name),
                              new XElement("link", currentUrl.ToString()),
                              new XElement("pubDate", t.CreateDate.ToUniversalTime()),
                              new XElement("description", new XCData(t.Description)),
                              new XElement("comments", ""),
                              new XElement("author", "SportStore"),
                              new XElement("guid", new XAttribute("isPermaLink", "false"), t.ProductId)
                          );

            XDocument doc = new XDocument(
                new XElement("rss",
                    new XAttribute("version", "2.0"),
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                    new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                    new XComment("Generated " + DateTime.Now.ToString("r")),

                    new XElement("channel",
                        new XElement("title", "SportStore new products"),
                        new XElement("description", "Buy all the hottest new sport gear"),
                        new XElement("link", currentUrl),
                        new XElement("language", "en-us"),
                        new XElement("copyright", "Copyright 2012, SportStore"),
                        new XElement("managingEditor", "Yuri"),
                        new XElement("webMaster", "Yuri"),
                        new XElement("generator", "RssGenerator class"),
                        new XElement("lastBuildDate", DateTime.Now.AddDays(-7).ToUniversalTime()),
                        entries
                    )
                )
            );

            return @"<?xml version=""1.0"" encoding=""utf-8""?>" + Environment.NewLine + doc.ToString();
        }
    }
}