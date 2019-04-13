using System;
using AngleSharp.Html.Dom;
namespace SimpleTextCrawler.Parsers.Nekdo {
    class NekdoSettings : IParserSettings {
        public NekdoSettings(Int32 start, Int32 end){
            StartPoint = start;
            EndPoint = end;
        }
        public String BaseUrl{ get; set; } = "https://nekdo.ru";
        public String Preffix { get; set; } = "/page/{CurrentId}";
        public Int32 StartPoint { get; set; }
        public Int32 EndPoint { get; set; }
    }
}