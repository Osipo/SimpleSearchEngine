using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace SimpleTextCrawler.Structures {
    class ElementType<T> {
        public T Element {get;set;}
        public ElementType<T> Next {get;set;}
    }
}