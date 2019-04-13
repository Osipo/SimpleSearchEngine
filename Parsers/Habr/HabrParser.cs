using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
namespace SimpleTextCrawler.Parsers.Habr {
    class HabrParser : IParser<String[]> {
        //EXTRACT HEADERS FROM THE HabraHabr.com
        public String[] Parse(IHtmlDocument document){
            var list = new List<String>();
            var items = document.QuerySelectorAll("a").Where(i => i.ClassName != null && i.ClassName.Contains("post__title_link"));
            foreach(var item in items){
                list.Add(item.TextContent);
            }
            return list.ToArray();
        }
    }
}