using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genNewPronounceDic
{
    public class WordItem
    {
        public string word;  // 词语
        public Collection<string> pros = new Collection<string>();  // 按字存放的音

        public WordItem(string word_)
        {
            word = word_;
            for (int i = 0; i < word.Length; i++)
            {
                string temp = "Null";
                pros.Add(temp);
            }
        }

        public void addPro(string ch, string pro)//加入读音
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (!word[i].ToString().Equals(ch))
                    continue;
                pros[i] = pro;
            }
        }

        public string getPor()//从字典中生成拼音
        {
            string str = "";
            int num = 0;
            foreach (string s in pros)
            {
                if (s.Equals("Null"))
                {
                    //Console.Write(word + " " + num);
                    //string pron = Console.ReadLine().ToString();
                    //str += pron + "_";
                    continue;
                }
                else
                {
                    str += s + "_";
                    
                }
                num += 1;
            }
            str = str.Substring(0, str.Length - 1);
            return str;
        }

        public string print()
        {
            string str = "";
            str += word + "\t";
            str += "<NULL>\t";
            str += "0\t";
            int num = 0;
            foreach (string s in pros)
            {

                if (s.Equals("Null"))
                {
                    //Console.Write(word + " " + num);
                    //string pron = Console.ReadLine().ToString();
                    //str += pron + "_";
                    continue;
                }
                else
                {
                    str += s + "_";
                    
                }
                num += 1;
            }
            str = str.Substring(0, str.Length - 1);
            return str;
        }
    }
}
