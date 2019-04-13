using System;
using System.Text;
using System.Text.RegularExpressions;
namespace SimpleTextCrawler.WordNormalizers{
    //STATIC CLASS
    //USE: Porter.TransformingWord(word) INSTEAD OF _stem.Stem(word) IN TrainDataParser
    public static class Porter {

    private static Regex PERFECTIVEGROUND = new Regex("((��|����|������|��|����|������)|((<;=[��])(�|���|�����)))$");

    private static Regex REFLEXIVE = new Regex("(�[��])$");

    private static Regex ADJECTIVE = new Regex("(��|��|��|��|���|���|��|��|��|��|��|��|��|��|���|���|���|���|��|��|��|��|��|��|��|��)$");

    private static Regex PARTICIPLE = new Regex("((���|���|���)|((?<=[��])(��|��|��|��|�)))$");

    private static Regex VERB = new Regex("((���|���|���|����|����|���|���|���|��|��|��|��|��|��|��|���|���|���|��|���|���|��|��|���|���|���|���|��|�)|((?<=[��])(��|��|���|���|��|�|�|��|�|��|��|��|��|��|��|���|���)))$");

    private static Regex NOUN = new Regex("(�|��|��|��|��|�|����|���|���|��|��|�|���|��|��|��|�|���|��|���|��|��|��|�|�|��|���|��|�|�|��|��|�|��|��|�)$");

    private static Regex RVRE = new Regex("^(.*?[���������])(.*)$");

    private static Regex DERIVATIONAL = new Regex(".*[^���������]+[���������].*����?$");

    private static Regex DER = new Regex("����?$");

    private static Regex SUPERLATIVE = new Regex("(����|���)$");

    private static Regex I = new Regex("�$");
    private static Regex P = new Regex("�$");
    private static Regex NN = new Regex("��$");

    public static string TransformingWord(string word)
    {
        word = word.ToLower();
        word = word.Replace('�', '�');
        MatchCollection m = RVRE.Matches(word);
        if (m.Count > 0)
        {
            Match match = m[0]; // only one match in this case 
            GroupCollection groupCollection = match.Groups;
            string pre = groupCollection[1].ToString();
            string rv = groupCollection[2].ToString();

            MatchCollection temp = PERFECTIVEGROUND.Matches(rv);
            string StringTemp = ReplaceFirst(temp, rv);


            if (StringTemp.Equals(rv))
            {
                MatchCollection tempRV = REFLEXIVE.Matches(rv);
                rv = ReplaceFirst(tempRV, rv);
                temp = ADJECTIVE.Matches(rv);
                StringTemp = ReplaceFirst(temp, rv);
                if (!StringTemp.Equals(rv))
                {
                    rv = StringTemp;
                    tempRV = PARTICIPLE.Matches(rv);
                    rv = ReplaceFirst(tempRV, rv);
                }
                else
                {
                    temp = VERB.Matches(rv);
                    StringTemp = ReplaceFirst(temp, rv);
                    if (StringTemp.Equals(rv))
                    {
                        tempRV = NOUN.Matches(rv);
                        rv = ReplaceFirst(tempRV, rv);
                    }
                    else
                    {
                        rv = StringTemp;
                    }
                }

            }
            else
            {
                rv = StringTemp;
            }

            MatchCollection tempRv = I.Matches(rv);
            rv = ReplaceFirst(tempRv, rv);
            if (DERIVATIONAL.Matches(rv).Count > 0)
            {
                tempRv = DER.Matches(rv);
                rv = ReplaceFirst(tempRv, rv);
            }

            temp = P.Matches(rv);
            StringTemp = ReplaceFirst(temp, rv);
            if (StringTemp.Equals(rv))
            {
                tempRv = SUPERLATIVE.Matches(rv);
                rv = ReplaceFirst(tempRv, rv);
                tempRv = NN.Matches(rv);
                rv = ReplaceFirst(tempRv, rv);
            }
            else
            {
                rv = StringTemp;
            }
            word = pre + rv;

        }

        return word;
    }

    public static string ReplaceFirst(MatchCollection collection, string part)
    {
        string StringTemp = "";
        if (collection.Count == 0)
        {
            return part;
        }
        /*else if(collection.Count == 1) 
        { 
        return StringTemp; 
        }*/
        else
        {
            StringTemp = part;
            for (int i = 0; i < collection.Count; i++)
            {
                GroupCollection GroupCollection = collection[i].Groups;
                if (StringTemp.Contains(GroupCollection[i].ToString()))
                {
                    string deletePart = GroupCollection[i].ToString();
                    StringTemp = StringTemp.Replace(deletePart, "");
                }

            }
        }
        return StringTemp;
    }

    }
}