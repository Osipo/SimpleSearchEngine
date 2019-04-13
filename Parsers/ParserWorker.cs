using System;
using AngleSharp.Html.Parser;
using System.Threading;
using System.Threading.Tasks;
namespace SimpleTextCrawler.Parsers {
    
    //Class for Parsers.
    //You can Set Parser with its Settings.
    //IMPORTANT: Ensure, that your parser process a web site, which contains in the IParserSettings.
    class ParserWorker<T> where T : class {
        private IParser<T> _parser;
        private IParserSettings _settings;
        private HtmlLoader _loader;
        private bool _isActive;
        
        
        public event Action<Object, T> OnNewData;//T1 = Parser, T2 = Data.
        public event Action<Object> OnComplete;//T1 = Parser
        
        #region Geters/Setters
        public void SetParser(IParser<T> parser){
            _parser = parser;
        }
        public IParser<T> GetParser(){
            return _parser;
        }
        
        public void SetSettings(IParserSettings settings){
            _settings = settings;
            _loader = new HtmlLoader(settings);
        }
        
        public IParserSettings GetSettings(){
            return _settings;
        }
        
        public Boolean IsActive(){
            return _isActive;
        }
        #endregion
        
        #region .ctors
        public ParserWorker(IParser<T> parser){
            this._parser = parser;
        }
        
        public ParserWorker(IParser<T> parser, IParserSettings settings) : this(parser) {
            this._settings = settings;
            this._loader = new HtmlLoader(settings);
        }
        
        #endregion
        
        public void Start(){
            _isActive = true;
            Task.Run(() => Worker());//1 arg: void(cts.Token,args...), 2arg: cts.Token
        }
        
        public void Abort(){
            _isActive = false;
        }
        
        public String[] GetUrls(){
            if(_settings == null){
                Console.WriteLine("InvalidOperation");
                return null;
            }
            String[] r = new String[_settings.EndPoint];
            Int32 i = 0,j = _settings.StartPoint;
            while(i < r.Length){
                r[i] = $"{_settings.BaseUrl}{_settings.Preffix.Replace("{CurrentId}","")}{j}/";
                i++;
                j++;
            }
            return r;
        }
        
        private async void Worker(){
            for(Int32 i = _settings.StartPoint; i <= _settings.EndPoint; i++){
                if(!_isActive){
                    OnComplete?.Invoke(this);
                    return;
                }
                var s = await _loader.GetSourceByPageId(i);
                var domp = new HtmlParser();//Parse Html_String to IHtmlDocument
                var doc = await domp.ParseDocumentAsync(s);//second arg is CancellationToken for cancellation.
                var r = _parser.Parse(doc);//Parse IHtmlDocument into String.
                
                OnNewData?.Invoke(this,r);
            }
            OnComplete?.Invoke(this);
            _isActive = false;
        }
    }
}