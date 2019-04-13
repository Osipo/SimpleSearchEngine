using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
namespace SimpleTextCrawler.Parsers{
    
    //This class extracts info from the IParserSettings and download its html-content.
    class HtmlLoader{
        private readonly HttpClient _client;
        private readonly String _url;
        public HtmlLoader(IParserSettings settings){
            _client = new HttpClient();
            _url = $"{settings.BaseUrl}/{settings.Preffix}/";
        }
        
        public async Task<String> GetSourceByPageId(Int32 id){
            var currentUrl = _url.Replace("{CurrentId}",id.ToString());
            var response = await _client.GetAsync(currentUrl);
            String source = null;
            
            if(response != null && response.StatusCode == System.Net.HttpStatusCode.OK){
                source = await response.Content.ReadAsStringAsync();
            }
            return source;
        }
    }
}