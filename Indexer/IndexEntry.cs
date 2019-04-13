using System;
using System.Text;
using SimpleTextCrawler.Structures;
namespace SimpleTextCrawler.Indexer{
    class IndexEntry {
        
        private LinkedList<Int32> _ids;
        private LinkedList<Int32> _freqs;
        private LinkedList<Double> _probs;//tf
        private LinkedList<Double> _tf_idf;
        
        public IndexEntry(){
            _ids = new LinkedList<Int32>();
            _freqs = new LinkedList<Int32>();
            _probs = new LinkedList<Double>();
            _tf_idf = new LinkedList<Double>();
        }
        
        public LinkedList<Int32> Ids {
            get{
                return _ids;
            }
        }
        
        
        
        public LinkedList<Int32> Freqs {
            get{
                return _freqs;
            }
        }
        
        
        public LinkedList<Double> Weights{
            get{
                return _tf_idf;
            }
        }
        
        
        public LinkedList<Double> Probs{
            get{
                return _probs;
            }
        }
        
        public Int32 TCount { get; set; }
        
        public Double IDF { get; set;}
        
        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append("TCount = "+TCount);
            sb.Append("\tIds: "+Ids.ToString());
            sb.Append("\tFreqs: "+Freqs.ToString());
            sb.Append("\n\tTF: "+Probs.ToString());
            sb.Append("\n\tIDF: "+IDF);
            sb.Append("\n\tWeights: "+Weights.ToString());
            sb.Append("\n");
            return sb.ToString();
        }
    }
}