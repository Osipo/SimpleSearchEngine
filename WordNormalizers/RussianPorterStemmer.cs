using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
namespace SimpleTextCrawler.WordNormalizers {
    class RussianPorterStemmer : IStemmer {
        private static String NOUN_ENDING;
        private static String SIMPLE_ENDING;
        private static String SELF_CALL;
        private static String ADJECTIVE_ENDING;
        private static String PARTICIPLE_SUFFIX;
        private static String VERB_ENDING;
        
        private static String PERFECT;
        private static String INHERIT;
        
        private static String DOUBLE_N;
        private static String I;
        private static String SIGN;
        
        private static String DER;
        private static String SUPER;
        
        static RussianPorterStemmer(){
            NOUN_ENDING = "(а|ев|ов|ие|ье|е|иями|ями|ами|еи|ии|и|ией|ей|ой|ий|й|иям|ям|ием|ем|ам|ом|о|у|ах|иях|ях" +
                    "|ь|ию|ью|ю|ия|ья|я|ы)$";
            SIMPLE_ENDING = "^(.*?[аеиоуыэюя])(.*)$";
            SELF_CALL = "(с[яь])$";
            ADJECTIVE_ENDING = "(ее|ие|ые|ое|ими|ыми|ей|ий|ый|ой|ем|им|ым|ом|его|ого|ему|ому|их|ых|ую|юю|ая|яя|ою|ею)$";
            PARTICIPLE_SUFFIX = "((ивш|ывш|ующ)|((?<=[ая])(ем|нн|вш|ющ|щ)))$";
            VERB_ENDING = "((ила|ыла|ена|ейте|уйте|ите|или|ыли|ей|уй|ил|ыл|им|ым|ен|ило|ыло|ено|ят|ует|уют|ит|ыт|" +
                    "ены|ить|ыть|ишь|ую|ю)|((?<=[ая])(ла|на|ете|йте|ли|й|л|ем|н|ло|но|ет|ют|ны|ть|ешь|нно)))$";
                    
            PERFECT = "((ив|ывшись|ивши|ывши|ившись|ыв)|((?<=[ая])(вши|в|вшись)))$";
            INHERIT = ".*[^аеиоуыэюя]+[аеиоуыэюя].*ость?$";
            
            DOUBLE_N = "нн$";
            I = "и$";
            SIGN = "ь$";
            
            DER = "ость?$";
            SUPER  = "(ейш|ейше)$";
        }
        
        // Call this entry to get basic form of word. It's only guess about possible lemma, but can be really good
        // after some improvements.
        public String Stem(String word) {
            word = word.ToLower();
            if(Regex.IsMatch(word,SIMPLE_ENDING)){
                String preffix;
                String rt;
                String ending;
                
                MatchCollection mc = Regex.Matches(word,SIMPLE_ENDING);
                preffix = mc[0].Groups[0].ToString();

                
                rt = "";
                if(mc.Count == 2){
                    rt = mc[1].Groups[0].ToString();
                }
                
                //PERFECT.matcher(rt).replaceFirst("")
                ending = __replaceFirst(rt,PERFECT,"");
                
                if (ending.Equals(rt)) {
                    rt = __replaceFirst(rt,SELF_CALL,"");
                    ending = __replaceFirst(rt,ADJECTIVE_ENDING,"");
                    if (!ending.Equals(rt)) {
                        rt = ending;
                        rt = __replaceFirst(rt,PARTICIPLE_SUFFIX,"");
                    }
                    else{
                        ending = __replaceFirst(rt,VERB_ENDING,"");
                        if (ending.Equals(rt)) {
                            rt = __replaceFirst(rt,NOUN_ENDING,"");
                        }
                        else{
                            rt = ending;
                        }

                    }
                }
                else{
                    rt = ending;
                }
                
                rt = __replaceFirst(rt,I,"");
                if(Regex.IsMatch(rt,INHERIT)){
                    rt = __replaceFirst(rt,DER,"");
                }
                
                ending = __replaceFirst(rt,SIGN,"");
                if(ending.Equals(rt)){
                    rt = __replaceFirst(rt,SUPER,"");
                    rt = __replaceFirst(rt,DOUBLE_N,"н");
                }
                else{
                    rt = ending;
                }
                
                word = rt + preffix;
            }
            
            return word;
        }
        
        private String __replaceFirst(String input, String pattern, String replacement){
            String result;
            MatchCollection m = Regex.Matches(input,pattern);
            if(m.Count > 0){
                String t1 = Regex.Replace(m[0].Value,pattern,replacement);//replaceFirst
                result = t1;
                Int32 i = 1;
                while(i < m.Count){
                    result += m[i].Value;
                    i++;
                }
            }
            else{
                result = input;
            }
            return result;    
        }
    }
}