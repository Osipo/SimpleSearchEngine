using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace SimpleTextCrawler.Structures.Trees {
    interface IVisitor<T> {
        void PreOrder(ITree<T> n, Action<Node<T>> act = null);
        void PreOrder(ITree<T> t, Node<T> n, Action<Node<T>> act = null);
        
        void InOrder(ITree<T> t,Action<Node<T>> act = null);
        
        void PostOrder(ITree<T> n, Action<Node<T>> act = null);
        void PostOrder(ITree<T> t, Node<T> n, Action<Node<T>> act = null);
    }
}