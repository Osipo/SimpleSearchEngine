using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TREE = SimpleTextCrawler.Structures.LinkedDictionary<System.Char,System.Object>; //TRIE
namespace SimpleTextCrawler.WordNormalizers {
    class LemmatizerModel {
        private static Regex RUSSIAN_WORD;
        private readonly TREE _tree;
        
        static LemmatizerModel(){
            RUSSIAN_WORD = new Regex(@"[А-Яа-я-]+");
        }
        public LemmatizerModel(){
            _tree = new TREE();//create ROOT.
        }
        
        // Train model entry. Stores the word and its lemma in a compressed model. (Preffix-TREE)
        public void AddWordForm(String wordForm, String lemma) {
            
            // Validation
            if (!(__Is_Valid(wordForm) && __Is_Valid(lemma)))
                return;
            
            TREE node = _tree;//ROOT.
            Int32 tail = wordForm.Length;
            TREE next_node;//CHILD
            
            for(Int32 i = 0; i < tail;i++){
                next_node = (TREE) _tree.GetValue(wordForm[i]);
                if(next_node != null){
                    node = next_node;
                }
                else{
                    TREE child = new TREE();
                    node.Add(wordForm[i], child);
                    
                    node = child;
                }
            }
            
            // Check is there any lemma for this path and add it if it doesn't exist
            List<String> word_lemmas = (List<String>) node.GetValue('#');
            
            if (word_lemmas == null) {
                word_lemmas = new List<String>();
                node.Add('#', word_lemmas);
            }
            
            // Check is lemma in list
            Int32 index = word_lemmas.IndexOf(lemma);
            if (index < 0)
                word_lemmas.Add(lemma);

            // Verbose training output.
            Console.WriteLine("Learning WordForm: " + wordForm + "\n");
        }
        
        // Retrieve lemma by prefix tree search
        public List<String> GetLemmas(String word) {
            
            TREE node = _tree;//ROOT
            TREE next_node;//CHILD
            List<String> word_lemmas = new List<String>();
            Int32 tail = word.Length;
            
            for(Int32 i = 0; i < tail; i++){
                next_node = (TREE) _tree.GetValue(word[i]);
                if(next_node != null){
                    node = next_node;
                }
                else{
                    return null;
                }
            }
            
            // Saving of actual lemmas.
            List<String> l = (List<String>)node.GetValue('#');
            if (l != null) {
                for(Int32 i = 0; i < l.Count;i++){
                    word_lemmas.Add(l[i]);
                }
            }
            else {
                return null;
            }
            
            return word_lemmas;
        }
        
        // Validation for non-empty russian word
        private Boolean __Is_Valid(String line) {
            if (line == null || line.Length < 1)
                return false;
            return RUSSIAN_WORD.IsMatch(line);
        }
    }
}