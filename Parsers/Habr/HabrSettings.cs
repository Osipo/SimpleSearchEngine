using System;
using AngleSharp.Html.Dom;
namespace SimpleTextCrawler.Parsers.Habr {
    class HabrSettings : IParserSettings {
        public HabrSettings(Int32 start, Int32 end){
            StartPoint = start;
            EndPoint = end;
        }
        public String BaseUrl{ get; set; } = "https://habr.com/ru/all/";
        public String Preffix { get; set; } = "page{CurrentId}";
        public Int32 StartPoint { get; set; }
        public Int32 EndPoint { get; set; }
    }
}