using System;
using System.Linq;
using System.Collections.Generic;
namespace SimpleTextCrawler.Indexer {
    class Counter<T> {
        private IEnumerable<T> _li;
        private Dictionary<T,Int32> d;
        public Counter(IEnumerable<T> list){
            _li = list;
            d = new Dictionary<T,Int32>();
            foreach(var Item in _li){
                d.TryAdd(Item,0);
            }
        }
        
        private (T Elem, Int32 C) _MaxCount(T skeep){
            foreach(var Item in _li){
                if(!(d.ContainsKey(Item)))
                    continue;
                if(skeep != null && skeep.Equals(Item)){
                    d.Remove(Item);
                    continue;
                }
                d[Item] += 1;
            }
            Int32 k = d.Max(x => x.Value);
            T Element = d.FirstOrDefault(x => x.Value == k).Key; //KeyValuePair<T,Int32>.Key
            return (Element, k);
        }
        
       
        private void _Flush(){
            foreach(var Item in _li){
                d[Item] = 0;//HIDDEN:: adding new Key despite of Remove(k)...
            }
        }
        private void _Reset(){
            List<T> _kl = new List<T>(); //buffer.
            
            foreach(var Item in d.Keys){
                _kl.Add(Item);
            }
            foreach(var Item in _kl){
                d[Item] = 0;//set 0 and not add new key.
            }
        }
        
        public List<(T, Int32)> MostCommon(Int32 n){
            if(n <= 0)
                n = 1;
            if(n > d.Count)
                n = d.Count;
            Int32 i = 0;
            List<(T,Int32)> li = new List<(T, Int32)>();
            T skip = default(T);
            while(i < n){
                var v = _MaxCount(skip);
                li.Add(v);
                skip = v.Elem;
                i+=1;
                _Reset();
            }
            _Flush();
            return li;
        }
        
        public List<Int32> Values(){
            List<(T e,Int32 c)> tuples = MostCommon(d.Count);
            List<Int32> v = new List<Int32>();
            foreach(var t in tuples){
                v.Add(t.c);
            }
            return v;
        }
    }
}