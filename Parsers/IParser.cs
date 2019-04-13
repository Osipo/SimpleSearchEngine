using AngleSharp.Html.Dom;
namespace SimpleTextCrawler.Parsers{
    interface IParser<T> where T : class {
        T Parse(IHtmlDocument document);
    }
}