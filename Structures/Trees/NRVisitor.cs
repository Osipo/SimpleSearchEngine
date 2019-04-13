using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace SimpleTextCrawler.Structures.Trees {
    class NRVisitor<T> : IVisitor<T> {
        public void PreOrder(ITree<T> tree, Action<Node<T>> act = null){
            Node<T> m = tree.Root();//ROOT(T)
            
            if(act == null){
                act = (n) => Console.Write(tree.Value(n).ToString()+" ");
            }
            
            SimpleTextCrawler.Structures.LinkedStack<SimpleTextCrawler.Structures.Trees.Node<T>> STACK = 
                new SimpleTextCrawler.Structures.LinkedStack<SimpleTextCrawler.Structures.Trees.Node<T>>();
                
            while(true){
                if(m != null){
                    act(m);//LABEL(node,TREE)
                    STACK.Push(m);
                    m = tree.LeftMostChild(m);//LEFTMOST_CHILD(node,TREE)
                }
                else{
                    if(STACK.IsEmpty()){
                        return;
                    }
                    m = tree.RightSibling(STACK.Top());//RIGHT_SIBLING(TOP(S),TREE) where TOP(S) is node
                    STACK.Pop();//POP(S)
                }
            }
        }
        
        public void PreOrder(ITree<T> tree, Node<T> st, Action<Node<T>> act = null){
            Node<T> m = st;//SUB_TREE(st,T)
            
            if(act == null){
                act = (n) => Console.Write(tree.Value(n).ToString());
            }
            
            SimpleTextCrawler.Structures.LinkedStack<SimpleTextCrawler.Structures.Trees.Node<T>> STACK = 
                new SimpleTextCrawler.Structures.LinkedStack<SimpleTextCrawler.Structures.Trees.Node<T>>();
            while(true){
                if(m != null){
                    act(m);//LABEL(node,TREE)
                    STACK.Push(m);
                    m = tree.LeftMostChild(m);//LEFTMOST_CHILD(node,TREE)
                }
                else{
                    if(STACK.IsEmpty()){
                        return;
                    }
                    if(STACK.Top().Equals(st)){
                        return;//after root exit.
                    }
                    m = tree.RightSibling(STACK.Top());//RIGHT_SIBLING(TOP(S),TREE) where TOP(S) is node
                    STACK.Pop();//POP(S)
                }
            }
        }
        
        
        //OVERRIDE FOR BINARY TREES.
        public virtual void InOrder(ITree<T> tree,Action<Node<T>> act = null){
            Node<T> m = tree.Root();
            if(act == null){
                act = (n) => Console.Write(tree.Value(n).ToString());
            }
            
            
            SimpleTextCrawler.Structures.LinkedStack<SimpleTextCrawler.Structures.Trees.Node<T>> STACK = 
                new SimpleTextCrawler.Structures.LinkedStack<SimpleTextCrawler.Structures.Trees.Node<T>>();
                
            SimpleTextCrawler.Structures.LinkedStack<SimpleTextCrawler.Structures.Trees.Node<T>> STACK2 = 
                new SimpleTextCrawler.Structures.LinkedStack<SimpleTextCrawler.Structures.Trees.Node<T>>();
            
            while(true){
                if(m != null){
                    STACK.Push(m);
                    m = tree.LeftMostChild(m);//LEFTMOST_CHILD(node,TREE) while current != null current = current.leftson
                }
                else{
                    if(STACK.IsEmpty()){
                        return;
                    }
                    //Node<T> c = STACK.Top();
                    //1) 1,2 S: 4. 2)4 3 5 8 S: 4. 3) 4 6 10 S: empty. 
                    act(STACK.Top());
                    m = tree.RightSibling(tree.LeftMostChild(STACK.Top()));//right son of the STACK //current = top.rightson
                    STACK.Pop();
                    if(m != null && tree.RightSibling(m) != null){
                        STACK2.Push(tree.RightSibling(m));//PUSH THIRD CHILD...
                    }
                    
                    if(STACK.IsEmpty() && m == null && !(STACK2.IsEmpty())){
                        //STACK.Push(STACK2.Top());
                        //STACK2.Pop();
                        m = STACK2.Top();
                        STACK2.Pop();
                    }
                }
            }
            
        }
        
        public void PostOrder(ITree<T> tree, Action<Node<T>> act = null){
            Node<T> m = tree.Root();//ROOT(T)
            
            if(act == null){
                act = (n) => Console.Write(tree.Value(n).ToString());
            }
            
           SimpleTextCrawler.Structures.LinkedStack<SimpleTextCrawler.Structures.Trees.Node<T>> STACK = 
                new SimpleTextCrawler.Structures.LinkedStack<SimpleTextCrawler.Structures.Trees.Node<T>>();
            
            while(true){
                if(m != null){
                    //Move action LABEL(node,TREE) to the end.
                    STACK.Push(m);
                    m = tree.LeftMostChild(m);//LEFTMOST_CHILD(node,TREE)
                }
                else{
                    if(STACK.IsEmpty()){
                        return;
                    }
                    act(STACK.Top());//LABEL(node,TREE)
                    m = tree.RightSibling(STACK.Top());//RIGHT_SIBLING(TOP(S),TREE) where TOP(S) is node
                    STACK.Pop();//POP(S)
                }
            }
        }
        
        public void PostOrder(ITree<T> tree, Node<T> st, Action<Node<T>> act = null){
            Node<T> m = st;//SUB_TREE(st,T)
            
            if(act == null){
                act = (n) => Console.Write(tree.Value(n).ToString());
            }
            
            SimpleTextCrawler.Structures.LinkedStack<SimpleTextCrawler.Structures.Trees.Node<T>> STACK = 
                new SimpleTextCrawler.Structures.LinkedStack<SimpleTextCrawler.Structures.Trees.Node<T>>();
            
            while(true){
                if(m != null){
                    //Move action LABEL(node,TREE) to the end.
                    STACK.Push(m);
                    m = tree.LeftMostChild(m);//LEFTMOST_CHILD(node,TREE)
                }
                else{
                    if(STACK.IsEmpty()){
                        return;
                    }
                    act(STACK.Top());//LABEL(node,TREE)
                    if(STACK.Top().Equals(st)){
                        return;//after root exit.
                    }
                    m = tree.RightSibling(STACK.Top());//RIGHT_SIBLING(TOP(S),TREE) where TOP(S) is node
                    STACK.Pop();//POP(S)
                }
            }
        }
        
    }
}