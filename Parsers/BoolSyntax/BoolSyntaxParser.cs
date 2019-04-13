using System;
using System.Text;
using System.Collections.Generic;
using SimpleTextCrawler.Structures;
namespace SimpleTextCrawler.Parsers.BoolSyntax {
    class BoolSyntaxParser {
        private Func<String,String,String,String> _act;
        public BoolSyntaxParser(Func<String,String,String,String> f){
            this._act = f;
        }
        public BoolSyntaxParser(){
            this._act = null;
        }
       
        
        private bool isOperator(String c)
        {
            switch (c)
            {
                case "and":case "и": return true;
                case "&": return true;
                case "|":case "или": return true;
                case "or":return true;
                case "(": return true;
                case ")": return true;
                case "~":return true;
                case "not":case "не": return true;
                default: return false;
            }
        }
        
        /*Priority of operator.*/
        private int getPriority(String c)
        {
            switch (c)
            {
                case "~": case "not": return 3;
                case "&": case "and": return 2;
                case "|": case "or": return 1;
                case "(": case ")": return 0;
                default: return -1;
            }
        }
        
        public LinkedStack<String> GetInput(String[] s){
            LinkedStack<String> ops = new LinkedStack<String>();
            LinkedStack<String> rpn = new LinkedStack<String>();
            for(Int32 i = 0; i < s.Length; i++){
                String tok = s[i];
                if(tok == "("){
                    ops.Push(tok);
                }
                else if(tok == ")"){
                    while(!ops.IsEmpty() && ops.Top() != "("){
                        rpn.Push(ops.Top());
                        ops.Pop();
                    }
                    ops.Pop();
                    /*
                    if(!ops.IsEmpty() && (ops.Top() == "not" || ops.Top() == "~")){
                        rpn.Push(ops.Top());
                        ops.Pop();
                    }*/
                }
                else if(!isOperator(tok)){//isWord
                    rpn.Push(tok);
                }
                else if(isOperator(tok)){
                    while(!ops.IsEmpty() && isOperator(ops.Top()) && getPriority(tok) <= getPriority(ops.Top()) ){
                        rpn.Push(ops.Top());
                        ops.Pop();
                    }
                    ops.Push(tok);
                }
                /*
                else if(tok == "not" || tok == "~"){
                    ops.Push(tok);
                }*/
            }
            while(!ops.IsEmpty()){
                rpn.Push(ops.Top());
                ops.Pop();
            }
            LinkedStack<String> result = new LinkedStack<String>();
            while(!rpn.IsEmpty()){
                result.Push(rpn.Top());
                rpn.Pop();
            }
            return result;
        }
    }
}