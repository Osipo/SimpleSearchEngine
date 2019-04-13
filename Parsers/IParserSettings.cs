using System;
namespace SimpleTextCrawler.Parsers{
    //Information about Web site.
    interface IParserSettings {
        String BaseUrl{ get; set; }
        String Preffix { get; set; }
        Int32 StartPoint { get; set; }
        Int32 EndPoint { get; set; }
    }
}