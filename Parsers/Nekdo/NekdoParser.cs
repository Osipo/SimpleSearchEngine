using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
namespace SimpleTextCrawler.Parsers.Nekdo {
    class NekdoParser : IParser<String[]> {
        
        //EXTRACT text of the jokes at the nekdo.ru
        public String[] Parse(IHtmlDocument document){
            var list = new List<String>();
            var items = document.QuerySelectorAll("div").Where(i => i.ClassName != null && i.ClassName.Contains("text"));
            foreach(var item in items){
                list.Add(item.TextContent);
            }
            return list.ToArray();
        }
    }
}