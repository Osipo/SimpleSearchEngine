using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace SimpleTextCrawler.Structures {
    class LinkedDictionary<K,V> : IDictionary<K,V>, ICollection<KeyValuePair<K,V>>, IEnumerable<KeyValuePair<K,V>>, IEnumerable {
        private Int32 _count;
        private IList<KeyValuePair<K,V>> _entries;
        private Boolean _overrideVals;
        
        public LinkedDictionary(){
            this._count = 0;
            this._entries = new SimpleTextCrawler.Structures.LinkedList<KeyValuePair<K,V>>();
            this._overrideVals = false;
        }
        
        public Boolean OverrideValues {
            get{
                return _overrideVals;
            }
            set{
                _overrideVals = value;
            }
        }
        
        //MAKENULL
        public void Clear(){
            this._count = 0;
            this._entries.Clear();
        }
        
        public Int32 IndexOfKey(K key){//HANDLE PARAM
            Int32 idx = 1;
            foreach(KeyValuePair<K,V> entry in _entries){
                if(entry.Key.Equals(key)){
                    return idx;
                }
                idx+=1;
            }
            return -1;
        }
        
        //CONTAINS(M,key): BOOLEAN
        public bool ContainsKey(K key){ //HANDLE PARAM
            return IndexOfKey(key) != -1;
        }
        
        public bool Contains(KeyValuePair<K,V> item){
            if(item.Equals(default(KeyValuePair<K,V>))){
                Console.WriteLine("Error. ArgumentNullException");
                return false;
            }
            return ContainsKey(item.Key);
        }
        
        public void Add(K key, V value){
            Int32 idx = IndexOfKey(key);
            if(idx == -1){
                _entries.Add(new KeyValuePair<K,V>(key,value));
                _count += 1;
            }
            else if(_overrideVals){
                _entries[idx] = new KeyValuePair<K,V>(key,value);//same key but new value.
            }
        }
        
        public void Add(KeyValuePair<K,V> item){
            if(item.Equals(default(KeyValuePair<K,V>))){
                Console.WriteLine("Error. ArgumentNullException");
                return;
            }
            Add(item.Key,item.Value);
        }
        
        //REMOVE pair.
        public bool Remove(K key){
            Int32 idx = IndexOfKey(key);
            if(idx == -1){
                Console.WriteLine("Error. Key not found.");
                return false;
            }
            _entries.RemoveAt(idx);
            _count -= 1;
            return true;
        }
        
        public Boolean Remove(KeyValuePair<K,V> item){
            if(item.Equals(default(KeyValuePair<K,V>))){
                Console.WriteLine("Error. ArgumentNullException");
                return false;
            }
            return Remove(item.Key);
        }
        
        
        public V GetValue(K key){
            foreach(KeyValuePair<K,V> entry in _entries){
                if(entry.Key.Equals(key)){
                    return entry.Value;
                }
            }
            return default(V);
        }
        
        public bool TryGetValue(K key, out V value){
            foreach(KeyValuePair<K,V> entry in _entries){
                if(entry.Key.Equals(key)){
                    value = entry.Value;
                    return true;
                }
            }
            value = default(V);
            return false;
        }
        
        //RETRIEVE(M,k)
        public V this[K key]{
            get{
                return GetValue(key);
            }
            set{
                Add(key,value);
            }
        }
        
        public System.Collections.Generic.ICollection<K> Keys{
            get{
                SimpleTextCrawler.Structures.LinkedList<K> lks = new SimpleTextCrawler.Structures.LinkedList<K>();
                foreach(KeyValuePair<K,V> entry in _entries){
                    lks.Add(entry.Key);
                }
                return lks;
                
                //return ((IList<K>) ((IEnumerable<KeyValuePair<K,V>>) _entries).Select(x => x.Key));
            }
        }
        
        public System.Collections.Generic.ICollection<V> Values {
            get{
                SimpleTextCrawler.Structures.LinkedList<V> lks = new SimpleTextCrawler.Structures.LinkedList<V>();
                foreach(KeyValuePair<K,V> entry in _entries){
                    lks.Add(entry.Value);
                }
                return lks;
                
                //return ((IList<V>) ((IEnumerable<KeyValuePair<K,V>>) _entries).Select(x => x.Value));
            }
        }
        
        public void CopyTo(KeyValuePair<K,V>[] array,Int32 arrayIndex){
            if(arrayIndex < 0 || arrayIndex >= array.Length){
                Console.WriteLine("arrayIndex is out of range");
                return;
            }
            Int32 i = arrayIndex;
            foreach(KeyValuePair<K,V> entry in _entries){
                array[i] = entry;
                i+=1;
                if(i == array.Length)
                    break;
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }
        
        public IEnumerator<KeyValuePair<K,V>> GetEnumerator(){
            return ((IEnumerable<KeyValuePair<K,V>>) _entries).GetEnumerator();
        }
        
        //EMPTY(M): BOOLEAN
        public Boolean IsEmpty(){
            return _count == 0;
        }
        
        public Int32 Count{
            get{
                return _count;
            }
        }
        
        public Boolean IsReadOnly{
            get{
                return false;
            }
        }
    }
}