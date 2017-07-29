using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace genDicWithWeight
{
    class Program
    {
        public static BasicBase bb = new BasicBase();
        public static TextIO txtManager = new TextIO();
        public static string txtOrgCharPath = "C:/Users/YLY/Desktop/灵伴/Total_Charac_GBK_weight.dict";
        public static string txtOrgWordPath = "C:/Users/YLY/Desktop/灵伴/Total_Word_XianDai5_weight.dict";
        public static string newDicPath = "C:/Users/YLY/Desktop/灵伴/genNewPronounceDic/新词典（加入贵州）0118.txt";
        public static string genNewChar = "C:/Users/YLY/Desktop/灵伴/genDicWithWeight/Total_Charac_GBK_weight(加入贵州)0119.txt";
        public static string genNewDicPath = "C:/Users/YLY/Desktop/灵伴/genDicWithWeight/Total_Word_XianDai5_weight(加入贵州)0119.txt";
        public static StreamWriter swChar = new StreamWriter(genNewChar, true);
        public static StreamWriter swWord = new StreamWriter(genNewDicPath, true);
        static void Main(string[] args)
        {
            txtManager.orgCharDicReader(txtOrgCharPath,bb);
            txtManager.orgWordDicReader(txtOrgWordPath,bb);
            txtManager.newDicReader(newDicPath,bb);
            writeNewDic();
            //testChar();
            //swChar.Close();
            swWord.Close();
            //Console.Read();
        }

        public static void testChar()
        {
            foreach (var w in bb.getNewDic())
            {
                if (w.Key.Length == 1)
                {
                    if (!bb.getOrgCharDic().ContainsKey(w.Key))
                        Console.WriteLine(w.Key);
                }
            }
        }

        public static void writeNewDic()
        {
            
            
            foreach (var word in bb.getNewDic())
            {
                if (word.Key.Length > 1)
                {
                    if (!bb.getOrgWordDic().ContainsKey(word.Key))
                    {
                        bb.getOrgWordDic().Add(word.Key, word.Value);
                        bb.getDicToAppend().Add(word.Key, word.Value);
                    }
                }
            }

            foreach (string s in bb.getOrgWord())
            {
                swWord.WriteLine(s);
            }

            foreach (var w in bb.getDicToAppend())
            {
                foreach (string s in bb.getDicToAppend()[w.Key])
                {
                    swWord.WriteLine(w.Key + "\t" + "<NULL>" + "\t" + "0" + "\t" + s);
                }
            }


        }
    }
}
