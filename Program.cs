using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using SimpleTextCrawler.Parsers;
using SimpleTextCrawler.Parsers.Habr;
using SimpleTextCrawler.Parsers.Nekdo;
using SimpleTextCrawler.Parsers.BoolSyntax;
using SimpleTextCrawler.WordNormalizers;
using SimpleTextCrawler.Indexer;
using SimpleTextCrawler.Structures;
using SimpleTextCrawler.Structures.Trees;
using LemmaSharp;
namespace SimpleTextCrawler
{
    class Program
    {
        private static Int32 i = 0;//for html-parsers
        private static DirectoryInfo dir = null;//represent directory with source files.
        private static String dsFiles = ""; //PATH to source files
        
        //SHOW MESSAGE THAT PARSER HAS FINISHED ITS JOB.
        private static void Complete(object sender){
            Console.WriteLine("---COMPLETED---");
        }
       
        
        //data are represent one page.
        //create new file and put them to it.
        private static void NewData(object sender, String[] data){
            i++;
            String path = dir.ToString()+"\\ptext"+i+".txt";
            using(StreamWriter sw = File.CreateText(path)){//OR FileStream -> BufferedStream -> StreamWriter.
                foreach(String d in data){
                    sw.WriteLine(d);
                }
            }
        }
        
        
        private static void CreateIndexF(String[] data){
            String path = dir.ToString()+"\\pindex.txt";
            Int32 i = 1;//FIRSTOF(doc_id).
            using(StreamWriter sw = File.CreateText(path)){
                foreach(String d in data){
                    sw.WriteLine(i+": "+d);
                    i++;
                }
            }
        }
       
       
        
        private static void Lemmatization(TrainDataParser worker){
            for(Int32 i = 1; i < 101; i++){
                String path = dsFiles+"\\ptext"+i+".txt";
                worker.LoadData(path,i);
            }
        }
        
        static void Main(string[] args)
        {
            //Console.WriteLine("Input link: ");
            //String s = Console.ReadLine();
            //Console.WriteLine("Input Directory to save: ");
            //String d = Console.ReadLine();
            
            String startupPath = System.IO.Directory.GetCurrentDirectory();
            //startupPath = Path.Combine(startupPath, d);
            
            
            dsFiles = Path.Combine(startupPath,"Data");//Path to source files.
            //String dict = Path.Combine(startupPath,"LDict.txt");//Dictionary of lemmas
            
            /* STEP 1. PARSE HTML
            ParserWorker<String[]> parser = new ParserWorker<String[]>(new NekdoParser());
            
            parser.SetSettings(new NekdoSettings(1,100));
            parser.OnNewData += NewData;
            parser.OnComplete += Complete;
            dir = new DirectoryInfo(startupPath);
            try{
                dir.Create();
            }
            catch(IOException){
                Console.WriteLine("This directory has already exist. Continue work with this directory");
            }
            parser.Start();
            while(parser.IsActive()){//awaiting parser...
                
            }
            
            
            
            CreateIndexF(parser.GetUrls());
            */
            
            
            //STEP 2 STEMMING
            /*
            TrainDataParser TDP = new TrainDataParser();
            
            
            Lemmatization(TDP);
            
            Console.WriteLine("");
            */
            //STEP 3 CREATING INDEX.
            String indexFileP = Path.Combine(startupPath,"Indexer","inventIndex.txt");
            
            Console.WriteLine("===STEP 3 ===");
            
            IndexBuilder builder = new IndexBuilder();
            Console.WriteLine("Source: {0} ",builder.Source);
            Console.WriteLine("Dest: {0}",indexFileP);
            
            
            LinkedDictionary<String,IndexEntry> indexer = builder.ReadData();//INDEX
            
            
            // UNCOMMENT FOR VECTOR RETRIEVAL (STEP 5)
            
            foreach(KeyValuePair<String,IndexEntry> p in indexer){
                Double I = Math.Round(100.0/p.Value.Ids.Count,5);
                p.Value.IDF = I;//Math.Log(100.0/p.Value.Ids.Count, 10.0);
                
                foreach(Double prob in p.Value.Probs){
                    p.Value.Weights.Add(prob*I); //tf(t,d)*idf(t,D) = tf-idf(t,d,D)
                }
                //String data = p.Key +" : "+ p.Value;
                //__CreateIFile(indexFileP, data);//read Data from indexer to file.
            }
            
            Console.WriteLine("Done.");
            
            
            
            IStemmer stem = new RussianStemmer();//STEMMER
            BoolSyntaxParser bp = new BoolSyntaxParser();//PARSER OF BOOLEAN EXPRESSIONS
            ILemmatizer lemmer = new LemmatizerPrebuiltCompact(LanguagePrebuilt.Russian);//LEMMATIZER.
            
            
            //STEP 4. BOOLEAN SEARCH BY(indexer)
            /*
            while(true){
                Console.WriteLine("Input search str...");
                String ui = Console.ReadLine();
                
                String[] u = ui.ToLower().Replace('ё','е').Split(new Char[]{' ' , ',', '.', ';', '-', ':','?','!','\"'},StringSplitOptions.RemoveEmptyEntries);
                LinkedStack<String> ui_w =  bp.GetInput(u);//GET EXPRESSION IN POLISH NOTATION
                
                String[] ui_wa = ui_w.ToArray();//SAVE IT INTO ARRAY
                foreach(String it2 in ui_wa){
                    Console.WriteLine(it2);
                }
                SimpleTextCrawler.Structures.LinkedList<Int32> idsOf = __GetIds(lemmer, indexer, ui_wa);
                __FindLinks(idsOf);
               
            }*/
            

            //STEP 5 Vector SEARCH BY(indexer).
            
            ArrayHeap<HeapEntry> PQ = new ArrayHeap<HeapEntry>(x => x.Relevance);//HEAP SORT.
            Console.WriteLine("VECTOR SEARCH...\n");
            while(true){
                PQ.Clear();
                Console.WriteLine("Input search str...");
                String ui = Console.ReadLine();
                Double[] score = new Double[101];
                //Double[] lengths = new Double[101];//ST_C
                Double[] lengths = builder.GetLens();//ST_UC
                Double q_w = 0.0;
                String[] u = ui.ToLower().Replace('ё','е').Split(new Char[]{' ' , ',', '.', ';', '-', ':','?','!','\"'},StringSplitOptions.RemoveEmptyEntries);
                foreach(String t in u){
                    IndexEntry te;
                    if(indexer.TryGetValue(lemmer.Lemmatize(t),out te)){
                        q_w += te.IDF*te.IDF;
                        Int32 i = 1;
                        foreach(Int32 id in te.Ids){
                            score[id] += te.Weights[i];
                            //lengths[id] += te.Probs[i]*te.Probs[i];//ST_C
                            i++;
                        }
                        
                    }
                }
                q_w = Math.Sqrt(q_w);
                if(q_w == 0.0){
                    Console.WriteLine("NOT FOUND");
                }
                else{
                    for(Int32 k = 1; k < 101; k++){
                        if(lengths[k - 1] == 0)//ST_C
                            continue;//ST_C
                        //lengths[k] = lengths[k] > 0 ? Math.Sqrt(lengths[k]) : 1;//ST_C
                        //score[k] = score[k]/(lengths[k]*q_w);//ST_C
                        score[k] = score[k]/(lengths[k - 1]*q_w);// 0 /1 => 0.
                        if(score[k] == 0.0)
                            continue;
                        PQ.Add(new HeapEntry(){Relevance = 1d/score[k], Id = k});//ASC ORDER
                    }
                    SimpleTextCrawler.Structures.LinkedList<Int32> docIds = new SimpleTextCrawler.Structures.LinkedList<Int32>();
                    Int32 KM = 5;
                    while(!PQ.IsEmpty() && KM > 0){
                        HeapEntry et = PQ.DeleteMin();
                        Console.WriteLine("{0} : {1} ",et.Id,1d/et.Relevance);
                        docIds.Add(et.Id);
                        KM--;
                    }
                    Console.WriteLine("");
                    __FindLinksV(docIds);
                }
            }
        }
        
        
        //WRITE DATA FROM INDEX INTO FILE
        private static void __CreateIFile(String path,String data){
            File.AppendAllText(path,data + Environment.NewLine,Encoding.UTF8);//append new line.
        }
        
        #region BoolSearch_procs
        //FOR BOOL SEARCH
        private static bool __isOperator(String c)
        {
            switch (c)
            {
                case "and":case "и": return true;
                case "&": return true;
                case "|": return true;
                case "or": case "или": return true;
                default: return false;
            }
        }
        
        private static bool __isUnOperator(String c){
            switch(c){
                case "~":  return true;
                case "not":case "не": return true;
                default: return false;
            }
        }
        
        //ADD NEW VECTOR
        private static Boolean[] __GetBVector(LinkedDictionary<String,IndexEntry> idx, String term){
            IndexEntry t1;
            Boolean[] v_i = new Boolean[101];
            if(idx.TryGetValue(term,out t1)){
                foreach(Int32 id in t1.Ids){
                    v_i[id] = true;
                }
                return v_i;
            }
            return v_i;//ZERO VECTOR
        }
        
        //COMPUTE BY VECTORS AND OPERATIONS IDS OF DOCUMENTS.
        private static SimpleTextCrawler.Structures.LinkedList<Int32> __GetIds(ILemmatizer lemmer, LinkedDictionary<String,IndexEntry> indexer, String[] expr){
            SimpleTextCrawler.Structures.LinkedList<Int32> IDS = new SimpleTextCrawler.Structures.LinkedList<Int32>();
            LinkedStack<Boolean[]> V = new LinkedStack<Boolean[]>();
            Int32 i = 0;
            while(i < expr.Length){
                if(__isUnOperator(expr[i])){
                    if(V.IsEmpty()){
                        Console.WriteLine("Error in Expression");
                        return IDS;
                    }
                    Boolean[] vi = V.Top();
                    V.Pop();
                    for(Int32 j = 1; j < 101; j++){
                        vi[j] = !(vi[j]);
                    }
                    V.Push(vi);
                }
                else if(__isOperator(expr[i])){
                    if(V.Count < 2){
                        Console.WriteLine("Error in Expression");
                        return IDS;
                    }
                    Boolean[] o1 = V.Top();
                    V.Pop();
                    Boolean[] o2 = V.Top();
                    V.Pop();
                    Boolean[] r = new Boolean[101];
                    switch(expr[i]){
                        case "and":{
                            for(Int32 j = 1; j < 101; j++){
                                r[j] = o1[j] && o2[j];
                            }
                            break;
                        }
                        case "or":{
                            for(Int32 j = 1; j < 101; j++){
                                r[j] = o1[j] || o2[j];
                            }
                            break;
                        }
                        default:{
                            for(Int32 j = 1; j < 101; j++){
                                r[j] = o1[j] && o2[j];
                            }
                            break;
                        }
                    }       
                    V.Push(r);
                }
                else{
                    //Console.WriteLine("Lemma: "+lemmer.Lemmatize(expr[i]));
                    V.Push(__GetBVector(indexer,lemmer.Lemmatize(expr[i])));
                    //Console.WriteLine("added");
                }
                i++;
            }
            if(V.IsEmpty()){
                Console.WriteLine("Error in Expression");
                return IDS;
            }
            Boolean[] r_v = V.Top();
            V.Pop();
            for(Int32 d = 1; d < 101; d++){
                if(r_v[d])
                    IDS.Add(d);
            }
            return IDS;
        }
        
        
        private static void __FindLinksV(SimpleTextCrawler.Structures.LinkedList<Int32> docIds){
            String path = Path.Combine(System.IO.Directory.GetCurrentDirectory(),"Data","pindex.txt");
            List<String> result = new List<String>();//IEnumerable result
            String[] data = File.ReadAllLines(path,Encoding.UTF8);
            Int32 k = 1;
            while(k < docIds.Count + 1){
                Int32 i = 0;
                while(docIds[k] != i)
                    i++;
                if(i == 101)
                    continue;
                String[] words = data[i - 1].ToLower().Split(new Char[]{' '},StringSplitOptions.RemoveEmptyEntries);
                if(docIds[k] == Convert.ToInt32(words[0].Substring(0,words[0].Length - 1))){
                    result.Add(words[1]);
                }
                k++;
            }
            if(result.Count == 0){
                Console.WriteLine("NOT FOUND");
                return;
            }
            Console.WriteLine("Links:\n");
            foreach(String l in result){
                Console.WriteLine(l);
            }
        }
        
        //GET LINKS FROM docIds
        private static void __FindLinks(SimpleTextCrawler.Structures.LinkedList<Int32> docIds){
            String path = Path.Combine(System.IO.Directory.GetCurrentDirectory(),"Data","pindex.txt");
            Console.WriteLine(path);
            docIds = new SimpleTextCrawler.Structures.LinkedList<Int32>(((IEnumerable<Int32>) docIds).OrderBy(x => x));
            
            List<String> result = new List<String>();//IEnumerable result
            
            Int32 j = 1;
            
            String[] data = File.ReadAllLines(path,Encoding.UTF8);
            for(Int32 i = 0; i < data.Length && j <= docIds.Count; i++){
                String[] words = data[i].ToLower().Split(new Char[]{' '},StringSplitOptions.RemoveEmptyEntries);
                if(docIds[j] == Convert.ToInt32(words[0].Substring(0,words[0].Length - 1))){
                    result.Add(words[1]);
                    j++;
                }
            }
            if(result.Count == 0){
                Console.WriteLine("NOT FOUND");
                return;
            }
            Console.WriteLine("Links:\n");
            foreach(String l in result){
                Console.WriteLine(l);
            }
        }
        #endregion
    }
}
