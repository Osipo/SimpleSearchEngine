using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace SimpleTextCrawler.Structures.Trees {
    interface ITree<T> {
        Node<T> Parent(Node<T> node);
        Node<T> LeftMostChild(Node<T> node);
        Node<T> RightSibling(Node<T> node);
        Node<T> Root();
        T Value(Node<T> node);
        void SetVisitor(IVisitor<T> visitor);
        Int32 GetCount();
        void Clear();//FROM ICollection<T>
    }
}