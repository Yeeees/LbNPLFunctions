using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace genDicWithWeight
{
    class TextIO
    {
        public TextIO()
        {

        }
        public void orgCharDicReader(string txtPath, BasicBase bb)
        {
            using (StreamReader sr = new StreamReader(txtPath, Encoding.GetEncoding("gb2312")))
            {
                string s = String.Empty;

                while ((s = sr.ReadLine()) != null)
                {
                    string[] line = s.Split('\t');
                    //Console.Write(line[0]);
                    //Console.WriteLine(line[1]);

                    if (bb.getOrgCharDic().ContainsKey(line[0]))
                        bb.getOrgCharDic()[line[0]].Add(line[3].Trim());
                    else
                    {
                        List<string> tempList = new List<string>();
                        tempList.Add(line[3]);
                        bb.getOrgCharDic().Add(line[0].Trim(), tempList);

                    }
                }
                sr.Close();
            }
        }

        public void orgWordDicReader(string txtPath, BasicBase bb)
        {
            using (StreamReader sr = new StreamReader(txtPath, Encoding.GetEncoding("gb2312")))
            {
                string s = String.Empty;

                while ((s = sr.ReadLine()) != null)
                {
                    string[] line = s.Split('\t');
                    //Console.Write(line[0]);
                    //Console.WriteLine(line[1]);

                    if (bb.getOrgWordDic().ContainsKey(line[0]))
                        bb.getOrgWordDic()[line[0]].Add(line[3].Trim());
                    else
                    {
                        List<string> tempList = new List<string>();
                        tempList.Add(line[3]);
                        bb.getOrgWordDic().Add(line[0].Trim(), tempList);

                    }
                    bb.getOrgWord().Add(s);
                }
                sr.Close();
            }
        }

        public void newDicReader(string txtPath, BasicBase bb)
        {
            using (StreamReader sr = new StreamReader(txtPath))
            {
                string s = String.Empty;

                while ((s = sr.ReadLine()) != null)
                {
                    string[] line = s.Split('\t');
                    //Console.Write(line[0]);
                    //Console.WriteLine(line[1]);

                    if (bb.getNewDic().ContainsKey(line[0]))
                        bb.getNewDic()[line[0]].Add(line[1].Trim());
                    else
                    {
                        List<string> tempList = new List<string>();
                        tempList.Add(line[1]);
                        bb.getNewDic().Add(line[0].Trim(), tempList);

                    }
                }
                sr.Close();
            }
        }

        
    }
}
