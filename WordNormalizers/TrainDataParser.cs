using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using LemmaSharp;
namespace SimpleTextCrawler.WordNormalizers {
    
    // Auxiliary. Class helps to parse data-entries in training data file.
    // It reads entries from file and sends them to train method in the model
    class TrainDataParser {
        
        private IStemmer _stemmer;
        private String _dPath;//path to dictionary of lemmas
        private String _s;//path to result files
        private ILemmatizer lm;
        
        public TrainDataParser() : this(Path.Combine(Directory.GetCurrentDirectory(),"LDict.txt")) {}
        public TrainDataParser(String path){
            this._stemmer = new RussianStemmer(); //RUSSIAN STEMMER
            this._dPath = path;
            this._s = Path.Combine(System.IO.Directory.GetCurrentDirectory(),"LData");
            this.lm = new LemmatizerPrebuiltCompact(LanguagePrebuilt.Russian);//RUSSIAN LEMMATIZER IS BEING USED NOW
        }
        
        // Opens the file and parses it.
        public void LoadData(String dataF,Int32 file_i) {
            
            String[] data = File.ReadAllLines(dataF,Encoding.UTF8);
            for(Int32 i = 0; i < data.Length; i++){
                __ProcessLine(data[i],file_i);//for each line in file.
            }
        }
        
        private void __AddWord(String path,String[] d){
            StringBuilder sb = new StringBuilder();
            
            for(Int32 i = 0; i < d.Length;i++){
                sb.Append(d[i]);
            }
            
            File.AppendAllText(path,sb.ToString()+ Environment.NewLine,Encoding.UTF8);//append new line.
        }
        
        private void __ProcessLine(String line,Int32 fi){//ADD PARAM FOR EACH DOCUMENT
            //String[] words = Regex.Split(line.ToLower().Replace('?','?'),@"[\.,:\-\s]+",RegexOptions.IgnoreCase);//.Split(new Char[]{' '},StringSplitOptions.RemoveEmptyEntries);
            String[] words = line.ToLower().Replace('ё','е').Split(new Char[]{' ' , ',', '.', ';', '-', ':'},StringSplitOptions.RemoveEmptyEntries);
            String[] data = new String[words.Length];
            
            for(Int32 i = 0; i < words.Length; i++){
                //data[i] = Porter.TransformingWord(words[i])+" "; //PORTER STEMMER
                //data[i] = _stemmer.Stem(words[i])+" ";//RUSSIAN STEMMER
                data[i] = lm.Lemmatize(words[i])+" ";// LEMMATIZER FROM DLLS
            }
            
            __AddWord(_s+"\\ltext"+fi+".txt",data);//write to file_i
        }
    }
}