using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SimpleTextCrawler.Structures;
namespace SimpleTextCrawler.Indexer{
    class IndexBuilder {
        
        private String _s;//Path to lem_files.
        private Double[] len;
        public IndexBuilder(){
            this._s = Path.Combine(System.IO.Directory.GetCurrentDirectory(),"LData"); 
            this.len = new Double[100];
        }
        
        public IndexBuilder(String dir){
            this._s = dir;
            this.len = new Double[100];
        }
        
        public Double[] GetLens(){
            return len;
        }
        
        // Opens the file and parses it.
        private List<String> __LoadData(String dataF) {
            
            List<String> result = new List<String>();//IEnumerable for one file.
            
            String[] data = File.ReadAllLines(dataF,Encoding.UTF8);
            for(Int32 i = 0; i < data.Length; i++){
                String[] words = data[i].ToLower().Replace('ё','е').Split(new Char[]{' ' , ',', '.', ';', '-', ':','?','!','\"'},StringSplitOptions.RemoveEmptyEntries);
                Int32 j = 0;
                while(j < words.Length){
                    result.Add(words[j]);
                    j++;
                }
            }
            
            return result;
        }
        
        public String Source{
            get{
                return _s;
            }
        }
        
        //Return Index As Dictionary<Term,IndexEntry>
        public LinkedDictionary<String,IndexEntry> ReadData(){
            LinkedDictionary<String,IndexEntry> result = new LinkedDictionary<String,IndexEntry>();
            result.OverrideValues = true;
            Int32 i = 1;
            while(i < 101){
                String path = _s+"\\ltext"+i+".txt";
                List<String> ie = __LoadData(path);//Get all terms from file_i
                
                
                Counter<String> c = new Counter<String>(ie);
                Int32 count = ie.Count;//count of words.
                List<(String e,Int32 c)> tuples = c.MostCommon(count);//count frequencies of all terms
                
                IndexEntry entry;
                foreach(var tuple in tuples){
                    if(result.TryGetValue(tuple.e,out entry)){
                        entry.TCount += tuple.c;
                        entry.Freqs.Add(tuple.c);//AddLast(new LinkedListNode<Int32>(tuple.c));
                        entry.Ids.Add(i);//AddLast(new LinkedListNode<Int32>(i));
                        entry.Probs.Add(tuple.c/(double)count);//TF
                        len[i - 1] += (tuple.c/(double)count)*(tuple.c/(double)count);  //OR Math.Pow(tuple.c/(double)count,2d);
                        result.Add(tuple.e, entry);
                    }
                    else{
                        IndexEntry ne = new IndexEntry();
                        ne.Ids.Add(i);//AddLast(new LinkedListNode<Int32>(i));
                        ne.Freqs.Add(tuple.c);//AddLast(new LinkedListNode<Int32>(tuple.c));
                        ne.TCount += tuple.c;
                        ne.Probs.Add(tuple.c/(double)count);//TF
                        len[i - 1] += (tuple.c/(double)count)*(tuple.c/(double)count);  //OR Math.Pow(tuple.c/(double)count,2d);
                        result.Add(tuple.e, ne);
                    }
                }
                i++;
            }
            //FULL LENGTH OF THE VECTORS OF TERMS WHERE VECTOR IS A DOCUMENT D(t1,t2...tn)
            
            for(Int32 k = 0; k < 100; k++){
                len[k] = Math.Sqrt(len[k]);
            }
            return result;
        }
    }
}