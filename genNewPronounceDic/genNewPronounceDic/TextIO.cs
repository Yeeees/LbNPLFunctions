using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace genNewPronounceDic
{
    class TextIO
    {
        public TextIO()
        { 
        
        }
        public void orgDicReader(string txtPath, BasicBase bb)//读取专有领域词典
        {
            using (StreamReader sr = new StreamReader(txtPath, Encoding.GetEncoding("gb2312")))
            {
                string s = String.Empty;

                while ((s = sr.ReadLine()) != null)
                {
                    string[] line = s.Split('\t');
                    //Console.Write(line[0]);
                    //Console.WriteLine(line[1]);

                    if (bb.getOrgDic().ContainsKey(line[0]))
                        bb.getOrgDic()[line[0]].Add(line[1].Trim());
                    else
                    {
                        List<string> tempList = new List<string>();
                        tempList.Add(line[1]);
                        bb.getOrgDic().Add(line[0].Trim(), tempList);

                    }
                }
                sr.Close();
            }
        }

        public void readCharDic(string txtPath, BasicBase bb)//读取通用领域字典
        {
            using (StreamReader sr = new StreamReader(txtPath, Encoding.GetEncoding("gb2312")))
            {
                string s = String.Empty;

                while ((s = sr.ReadLine()) != null)
                {
                    string[] line = s.Split('\t');
                    //Console.Write(line[0]);
                    //Console.WriteLine(line[3]);

                    if (bb.getCharDic().ContainsKey(line[0]))
                    {
                        if(!bb.getCharDic()[line[0]].Contains(line[3]))
                            bb.getCharDic()[line[0]].Add(line[3].Trim());

                    }
                    else
                    {
                        List<string> tempList = new List<string>();
                        tempList.Add(line[3]);
                        bb.getCharDic().Add(line[0].Trim(), tempList);

                    }
                }
                sr.Close();
            }
        }

        public void readWordsDic(string txtPath, BasicBase bb)//读取通用领域词典
        {
            using (StreamReader sr = new StreamReader(txtPath, Encoding.GetEncoding("gb2312")))
            {
                string s = String.Empty;

                while ((s = sr.ReadLine()) != null)
                {
                    string[] line = s.Split('\t');
                    //Console.Write(line[0]);
                    //Console.WriteLine(line[3]);

                    if (bb.getWordsDic().ContainsKey(line[0]))
                    {
                        if (!bb.getWordsDic()[line[0]].Contains(line[3]))
                        bb.getWordsDic()[line[0]].Add(line[3].Trim());
                    }
                    else
                    {
                        List<string> tempList = new List<string>();
                        tempList.Add(line[3]);
                        bb.getWordsDic().Add(line[0].Trim(), tempList);

                    }
                }
                sr.Close();
            }
        }

        public void orgPinyin(string txtPath, BasicBase bb)//读取全发音列表
        {
            using (StreamReader sr = new StreamReader(txtPath, Encoding.GetEncoding("gb2312")))
            {
                string s = String.Empty;

                while ((s = sr.ReadLine()) != null)
                {
                    string[] line = s.Split(' ');
                    bb.getPinyinOrg().Add(line[0]);
                    Console.WriteLine("org  "+line[0]);
                }
                sr.Close();
            }
        }

        public void readOrdDoc(string txtPath, BasicBase bb)//读取待处理表
        {
            using (StreamReader sr = new StreamReader(txtPath, Encoding.GetEncoding("gb2312")))
            {
                string s = String.Empty;

                while ((s = sr.ReadLine()) != null)
                {
                    bb.getOrgDoc().Add(s);
                }
                sr.Close();
            }
        }

        public void targetListReader(string txtPath, BasicBase bb)//读取待处理表中的所有词目
        {
            using (StreamReader sr = new StreamReader(txtPath, Encoding.GetEncoding("gb2312")))
            {
                string s = String.Empty;

                while ((s = sr.ReadLine()) != null)
                {
                    string[] line = s.Split('\t');
                    //Console.Write(line[0]);
                    //Console.WriteLine(line[1]);

                    foreach (string str in line)
                    {
                        string[] words = str.Split(' ');
                        foreach (string word in words)
                        {
                            string w = "";
                            foreach (char c in word)
                            {
                                string ch = c.ToString();
                                int currentcode = (int)c;
                                if (currentcode > 19968 && currentcode < 40869)
                                {
                                    w += ch;
                                }
                            }
                            if (bb.getTargetList().ContainsKey(w))
                                continue;
                            bb.getTargetList().Add(w,true);
                        }
                    }
                }
                sr.Close();
            }
        }

        public void mergeDic(BasicBase bb)//结合人工词典和专有领域词典
        {
            foreach (string word in bb.getManualDic().Keys)
            {
                string str = bb.getManualDic()[word].getPor();
                bb.getNewDic().Add(word, str);
            }
        }

        public void getPros(BasicBase bb)//生成所有词中字的的拼音
        {
            foreach (string s in bb.getTargetList().Keys)
            {
                string word = s.Trim();
                if (!bb.getManualDic().ContainsKey(word))
                {
                    if (bb.getOrgDic().ContainsKey(word))
                    {
                        bb.getNewDic().Add(word, bb.getOrgDic()[word][0]);
                    }
                    else
                    {
                        string pro = "";
                        for (int i = 0; i < word.Length; i++)
                        {
                            string tmp = word[i].ToString();
                            if (bb.getOrgDic().ContainsKey(tmp))
                            {
                                pro += "_"+bb.getOrgDic()[tmp][0];
                                //if (bb.getOrgDic()[tmp].Count() == 1)
                                //    bb.getNewDic()[word].addPro(tmp, bb.getOrgDic()[tmp][0]);
                            }
                        }
                        if (pro.Trim().Length > 0)
                        {
                            string finalPro = pro.Substring(1, pro.Length - 1);
                            bb.getNewDic().Add(word, finalPro);
                        }
                    }                    
                }
            }
        }


        

        public void manualDicReader(string txtPath, BasicBase bb)//读取人工标注字字典结果
        {
            using (StreamReader sr = new StreamReader(txtPath, Encoding.GetEncoding("gb2312")))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {

                    string[] tempLine = s.Split('\t');
                    string ch = tempLine[0];
                    string word = tempLine[1];

                    int choose = int.Parse(tempLine[3]);
                    string[] pron = tempLine[4].Split('|');
                    string tempPron = pron[choose - 1];
                    string truePron = tempPron.Substring(2, tempPron.Length - 2);
                    Console.WriteLine("manualDicReader  " + tempLine[1] + " " + truePron);

                    if (bb.getManualDic().ContainsKey(word))
                    {
                        bb.getManualDic()[word].addPro(ch, truePron);
                    }
                    //Console.WriteLine("重复 " + tempLine[1]);
                    else
                    {
                        WordItem item = new WordItem(word);
                        item.addPro(ch, truePron);
                        bb.getManualDic().Add(word, item);
                        for (int i = 0; i < word.Length; i++)
                        {
                            string tmp = word[i].ToString();
                            if (bb.getOrgDic().ContainsKey(tmp))
                            {
                                if (bb.getOrgDic()[tmp].Count() == 1)
                                    bb.getManualDic()[word].addPro(tmp, bb.getOrgDic()[tmp][0]);
                            }
                        }
                    }

                }
            }
        }
        public void manualWordsDicReader(string txtPath, BasicBase bb)//读取人工标注词字典
        {
            using (StreamReader sr = new StreamReader(txtPath, Encoding.GetEncoding("gb2312")))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {

                    string[] tempLine = s.Split('\t');
                    //string ch = tempLine[0];
                    string word = tempLine[0];

                    int choose = int.Parse(tempLine[2]);
                    string[] pron = tempLine[3].Split('|');
                    string tempPron = pron[choose - 1];
                    string truePron = tempPron.Substring(2, tempPron.Length - 2);
                    Console.WriteLine("manualDicReader  " + tempLine[1] + " " + truePron);

                    if (bb.getManualDic().Keys.Contains(word))
                    {
                        //bb.getManualDic()[word].addPro(ch, truePron);
                        //for (int i = 0; i < word.Length; i++)
                        //{
                        //    string tmp = word[i].ToString();
                        //    if (bb.getOrgDic().ContainsKey(tmp))
                        //    {
                        //        if (bb.getOrgDic()[tmp].Count() == 1)
                        //            bb.getManualDic()[word].addPro(tmp, bb.getOrgDic()[tmp][0]);
                        //    }
                        //}
                    }
                    //Console.WriteLine("重复 " + tempLine[1]);
                    else
                    {
                        WordItem item = new WordItem(word);
                        string[] prons = truePron.Split('_');
                        for (int i = 0; i < prons.Length; i++)
                        {
                            item.addPro(word[i].ToString(), prons[i]);
                        }
                    }

                }
            }
        }

        public void readRole(string txtPath, BasicBase bb)//读取通用领域词典中的所有行，为专有领域词典中不存在的词赋予相应的词性
        {
            using (StreamReader sr = new StreamReader(txtPath, Encoding.GetEncoding("gb2312")))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    string[] tempLine = s.Split('\t');
                    string word = tempLine[0];
                    string role = s;//整行
                    if (!bb.getRoles().ContainsKey(word))
                    {
                        List<string> roles = new List<string>();
                        roles.Add(s);
                        bb.getRoles().Add(word, roles);
                    }
                    else
                    {
                        bb.getRoles()[word].Add(s);
                    }

                    
                }
            }
        }




        
    }
}
