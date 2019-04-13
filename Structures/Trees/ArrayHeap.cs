using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using STACK2 = SimpleTextCrawler.Structures.LinkedStack<System.Int32>;
namespace SimpleTextCrawler.Structures.Trees {
    
    //Binary Tree which implements Priority Queue.
    //Based on Array.
    //Uses the presentation of the Heap.
    class ArrayHeap<T> : ITree<T> {
        private NodeHeap<T>[] _cellspace;
        private Int32 _count;
        private Int32 _capacity; //maxsize of array
        private Int32 _last;//last element of array
        private Int32 _h;
        private Func<T,Double> _p;
        
        private IVisitor<T> _visitor;
        #region .ctors
        public ArrayHeap(Func<T,Double> p) : this(1000,p){}  
        public ArrayHeap(Int32 capacity, Func<T,Double> p){
            this._capacity = capacity;
            this._cellspace = new NodeHeap<T>[capacity];
            this._count = 0;
            this._h = 0;
            this._last = 0;
            this._visitor = new NRVisitor<T>();
            this._p = p;
        }
        #endregion
        
        public bool IsEmpty(){
            return _count == 0;
        }
        
        //MAKENULL
        public void Clear(){
            for(Int32 i = 1; i < _cellspace.Length; i++){
                _cellspace[i] = null;
            }
            this._count = 0;
            this._h = 0;
            this._last = 0;
        }
        
        public void Add(T item){
            NodeHeap<T> n = new NodeHeap<T>();
            n.Value = item;
            Insert(n);
        }
        
        private void __Copy(){
            NodeHeap<T>[] nb = new NodeHeap<T>[_capacity * 2];
            this._capacity *= 2;
            for(Int32 i = 0; i < _cellspace.Length; i++){
                nb[i] = _cellspace[i];
            }
            _cellspace = nb; 
        }
        
        public void Insert(NodeHeap<T> n){//MAKE PRIVATE.
            Int32 i;
            NodeHeap<T> temp;
            if(_last >= _capacity - 1){
                __Copy();
                //Console.WriteLine("Queue is filled");
                //return;
            }
            this._last = this._last + 1;
            n.Idx = _last;
            _cellspace[_last] = n;
            i = _last;
            
            while(i > 1 && (_p(_cellspace[i].Value) < _p(_cellspace[i/2].Value))){
                //Move n up
                temp = _cellspace[i];
                _cellspace[i] = _cellspace[i/2];
                _cellspace[i].Idx = i;
                _cellspace[i/2] = temp;
                _cellspace[i/2].Idx = i/2;
                i = i/2;
            }
            _count = _count + 1;
            __ComputeH();
        }
        
        
        public T DeleteMin(){
            Int32 i,j;
            NodeHeap<T> temp;
            NodeHeap<T> minimum;
            if(_last == 0){
                Console.WriteLine("Error. Queue is empty");
                return default(T);
            }
            i = 1;
            minimum = _cellspace[i];
            _cellspace[1] = _cellspace[_last];
            this._last = this._last - 1;
            i = 1;
            while(i <= _last/2){
                if((_p(_cellspace[2*i].Value) < _p(_cellspace[2*i + 1].Value)) || (2*i == _last) ){
                    j = 2*i;
                }
                else{
                    j = 2*i + 1;
                }
                if(_p(_cellspace[i].Value) > _p(_cellspace[j].Value)){
                    //exchange last old element with child, which has the least priority.
                    temp = _cellspace[i];
                    _cellspace[i] = _cellspace[j];
                    _cellspace[i].Idx = i;
                    _cellspace[j] = temp;
                    _cellspace[j].Idx = j;
                    i = j;
                }
                else{
                    _count = _count - 1;
                    __ComputeH();
                    return minimum.Value;
                }
            }
            _count = _count - 1;
            __ComputeH();
            return minimum.Value;
        }
        
        private void __ComputeH(){
            Int32 h = 0;
            Int32 i = 1;
            while(_cellspace[i*2] != null && i*2 < _last+1){
                i*=2;
                h++;
            }
            
            this._h = h;
        }
        
        #region ITree
        public Node<T> Root(){
            return _cellspace[1];
        }
        
        public Node<T> Parent(Node<T> node){
            NodeHeap<T> np = node as NodeHeap<T>;
            if(np == null || np.Idx == 1){
                return null;
            }
            return _cellspace[np.Idx/2];
        }
        
        public Node<T> LeftMostChild(Node<T> node){
            NodeHeap<T> np = node as NodeHeap<T>;
            if(np == null || np.Idx*2 > _last){
                return null;
            }
            return _cellspace[np.Idx*2];
        }
        
        public Node<T> RightSibling(Node<T> node){
            NodeHeap<T> np = node as NodeHeap<T>;
            if(np == null || np.Idx == 1 || np.Idx+1 > _last){
                return null;
            }
            return _cellspace[np.Idx + 1];
        }
        
        public T Value(Node<T> node){
            return node.Value;
        }
        
        public void SetVisitor(IVisitor<T> visitor){
            this._visitor = visitor;
        }
        
        public Int32 GetCount(){
            return this._count;
        }
        #endregion
        
        #region ArrayHeap
        public void PrintContent(){
            for(Int32 i = 0; i < _last;i++){
                if(_cellspace[i] == null)
                    continue;
                Console.Write(_cellspace[i].Value.ToString()+" : {0}; ",i);
            }
            Console.WriteLine("");
        }
        
        
        public Int32 Height{
            get{
                return _h;
            }
        }
        #endregion
    }
}