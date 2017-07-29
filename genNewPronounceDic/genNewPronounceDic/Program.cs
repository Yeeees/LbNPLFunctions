using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml; 
namespace genNewPronounceDic
{
    class Program
    {
        public static BasicBase bb = new BasicBase();
        public static TextIO txtManager = new TextIO();
        
        //1.1现有数据
        public static string charDicPath = @"C:\Users\YLY\Desktop\灵伴\Total_Charac_GBK_weight.dict";//通用领域字典（字-词性-权重-注音）
        public static string XianDai5_weight = @"C:\Users\YLY\Desktop\灵伴\Total_Word_XianDai5_weight.dict";//通用领域词典（词语-词性-权重-注音）
        public static string wordsDicPath = "C:/Users/YLY/Desktop/灵伴/Total_Charac_Word.syl.dict";//专有领域词典（词语-注音）
        public static string orgPinyinPath = "C:/Users/YLY/Desktop/灵伴/PinYin.lst";//标准音节表（合法音节集合）
        //1.2待处理数据
        public static string wordlist_path = @"C:\Users\YLY\Desktop\灵伴\guizhou_002-0120.txt";//待处理词表（词语）
        //1.3输出文件
        public static string genNewDicPath = "C:/Users/YLY/Desktop/灵伴/genNewPronounceDic/新词典（加入贵州）0124 genNewPron.txt";//结果词典（词语-词性-权重-注音）

        //2.注音处理过程
        public static string noneFreqTxtPath = "C:/Users/YLY/Desktop/灵伴/PronounceFilter/未标音列表0124.txt";//无音字词典
        public static string charMultiFreqTxtPath = "C:/Users/YLY/Desktop/灵伴/PronounceFilter/多音字词典0124.txt";//多音字词典（字-词-出现次数-选择序号-选项）
        public static string wordsMultiFreqTxtPath = "C:/Users/YLY/Desktop/灵伴/PronounceFilter/词多音列表0124.txt";//多音词字典（词-出现次数-选择序号-选项）

        public static string newSigWordMultiFreqTxtPath = "C:/Users/YLY/Desktop/灵伴/PronounceFilter/单字多音0120双.txt";
        public static string newMulWordMultiFreqTxtPath = "C:/Users/YLY/Desktop/灵伴/PronounceFilter/多字多音列表0120双.txt";//人工标注多音字词典
        public static string WordsMultiFreqTxtPath = "C:/Users/YLY/Desktop/灵伴/PronounceFilter/词多音列表0120-已标注.txt";//人工标注多音词词典
        public static string nonePronManualTxtPath = "";//人工标注无音词词典

        public static string wrongPronList = "C:/Users/YLY/Desktop/灵伴/PronounceFilter/错误拼音列表0124.txt";//人工标注结果出现的在标准音节表中不存在的拼音
        public static string pronWayList = "C:/Users/YLY/Desktop/灵伴/PronounceFilter/全语调列表0124.txt";//人工标注结果中所有出现的语调
        public static string wrongPronsNum = "C:/Users/YLY/Desktop/灵伴/PronounceFilter/音节数与字数不同0124.txt";//检查音节数量是否与字数相同
        
        public static string charXmlPath = "C:/Users/YLY/Desktop/灵伴/PronounceFilter/字xmlDic.xml";
        public static string wordXmlPath = "C:/Users/YLY/Desktop/灵伴/PronounceFilter/词xmlDic.xml";

        public static string finalWrongPronList = "C:/Users/YLY/Desktop/灵伴/genNewPronounceDic/结果词典错误拼音列表0124.txt";//结果词典出现的在标准音节表中不存在的拼音

        public static string finalWrongPronsNum = "C:/Users/YLY/Desktop/灵伴/genNewPronounceDic/结果词典音节数与字数不同0124.txt";//结果词典检查音节数量是否与字数相同
        
        public static StreamWriter sw = new StreamWriter(genNewDicPath, true);
        static void Main(string[] args)
        {
            txtManager.orgDicReader(wordsDicPath, bb);//读取专有领域词典
            //bb.getOrgDic()["区"].Remove("ou1");//删除ou1读音
            
            txtManager.readCharDic(charDicPath,bb);//读取通用领域字典
            txtManager.readWordsDic(XianDai5_weight, bb);//读取通用领域词典
            txtManager.readOrdDoc(wordlist_path, bb);//读取待处理词表
            txtManager.targetListReader(wordlist_path, bb);//读取待处理词表生成所有词目
            txtManager.orgPinyin(orgPinyinPath, bb);//读取全发音列表



            
            //2.1 a to f
            countMultiAndNone();
            printMultiAndNone();
            Console.WriteLine("待标记多音字个数为: "+bb.getMultiPronList().Keys.Count+" 按回车继续....");
            Console.ReadLine();
            //2.2 人工标注多音词词典、多音字词典及无音字词典并检查错误

            txtManager.manualDicReader(newSigWordMultiFreqTxtPath, bb);
            txtManager.manualWordsDicReader(WordsMultiFreqTxtPath, bb);
            txtManager.manualDicReader(newMulWordMultiFreqTxtPath, bb);//双妹标注后的

            checkManualWork();//检查错误

            //2.3 循环步骤2.1及2.2，直至输出的多音词词典、多音字词典及无音字词典为空


            //2.4 合并通用领域词典和专有领域词典
            txtManager.mergeDic(bb);//整合通用领域词典和专有领域词典
            txtManager.getPros(bb);//得到词中每一个字的发音
            txtManager.readRole(XianDai5_weight, bb);//读取通用领域词典词性
            writeNewDicWithManual();
            sw.Close();

            //writeCharXmlFile(); 生成字xml字典，除非有新的字加入否则不需要
            writeWordXmlFile();//生产词xml词典
            //对最终生成的结果进行差错
            checkFinalWork(genNewDicPath);
            Console.WriteLine("Done!");
            Console.Read();
            
        }

        //<2.1 注音处理过程>
        public static void countMultiAndNone()
        {

            
            foreach (string line in bb.getOrgDoc())
            {
                
                string[] elements = line.Split('\t');
                foreach (string element in elements)
                {
                    string[] wordsList = element.Trim().Split(' ');
                    foreach (string words in wordsList)//遍历条目中每一个词
                    {
                        
                        if (words == null || words.Trim().Length == 0)
                            continue;
                        if (bb.getOrgDic().ContainsKey(words.Trim()))//如果在专有领域词典中存在
                        {
                            if (bb.getOrgDic()[words.Trim()].Count > 1)
                            {


                                if (bb.getMultiPronList().ContainsKey(words.Trim()))
                                    bb.getMultiPronList()[words.Trim()].countNum += 1;//计数器加一
                                else
                                {
                                    MultiPronWord mpw = new MultiPronWord(words.Trim());
                                    bb.getMultiPronList().Add(words.Trim(), mpw);
                                }
                            }

                        }
                        else if (bb.getWordsDic().ContainsKey(words.Trim()))//如果在通用领域词典中存在
                        {
                            if (bb.getWordsDic()[words.Trim()].Count > 1)
                            {
                                if (bb.getMultiPronList().ContainsKey(words.Trim()))
                                    bb.getMultiPronList()[words.Trim()].countNum += 1;//计数器加一
                                else
                                {
                                    MultiPronWord mpw = new MultiPronWord(words.Trim());
                                    bb.getMultiPronList().Add(words.Trim(), mpw);
                                    
                                }
                            }
                            else//如果存在且只有一个读音，则加入专有领域词典
                            {
                                bb.getOrgDic().Add(words, bb.getWordsDic()[words]);
                            }
                        }
                        else//两个词典中都不存在，遍历所有字
                        {
                            bool multiPronFlag = false;//标记这个词是否含有多音字
                            string tempPron = "";//尝试标音
                            foreach (char c in words)
                            {
                                string word = c.ToString();
                                if (bb.getCharDic().ContainsKey(word))//如果在通用领域字典中存在
                                {

                                    if (bb.getCharDic()[word].Count > 1)//如果含有多音字
                                    {
                                        multiPronFlag = true;
                                        if (bb.getMultiPronList().ContainsKey(words))
                                        {
                                            if (!bb.getMultiPronList()[words].multiPronChars.Contains(word))
                                                bb.getMultiPronList()[words].multiPronChars.Add(word);
                                            else
                                                bb.getMultiPronList()[words].countNum += 1;
                                        }
                                        else
                                        {
                                            MultiPronWord mpw = new MultiPronWord(word);
                                            bb.getMultiPronList().Add(words, mpw);
                                        }

                                    }
                                    else
                                    {
                                        tempPron += "_" + bb.getCharDic()[word][0];
                                    }
                                }
                                else
                                {
                                    int currentcode = (int)c;
                                    if (currentcode > 19968 && currentcode < 40869)//如果是汉字且在通用领域字典中不存在
                                    {
                                        Console.WriteLine(word.ToString());

                                        if (bb.getNoneFreqDic().ContainsKey(word.ToString()))
                                        {
                                            bb.getNoneFreqDic()[word.ToString()] += 1;
                                        }
                                        else
                                        {
                                            bb.getNoneFreqDic().Add(word.ToString(), 1);
                                        }
                                    }
                                    else
                                    {
                                        //specFlag = true;
                                    }

                                }
                            }
                            if (!multiPronFlag&&tempPron.Length>1)//如果不包含多音字，则在专有领域词典中加入该词读音
                            {
                                string truePron = tempPron.Substring(1, tempPron.Length - 1);
                                List<string> prons = new List<string>();
                                prons.Add(truePron);
                                string ch = "";
                                foreach (char c in words)
                                {
                                    if (c >= 19968 && c < 40869)//过滤汉字，注意不同编码中区间可能不同
                                    {
                                        ch += c;
                                    }
                                }
                                if(!bb.getOrgDic().ContainsKey(ch))
                                    bb.getOrgDic().Add(ch, prons); 
                            }
                        }
                    }
                }
            }
        }
        public static void printMultiAndNone()//将分类结果排序输出
        {
            StreamWriter swNoneFreq = new StreamWriter(noneFreqTxtPath, true);
            StreamWriter swSWMultiFreq = new StreamWriter(charMultiFreqTxtPath, true);           
            StreamWriter swWSMultiFreq = new StreamWriter(wordsMultiFreqTxtPath, true);
            Dictionary<string, int> nonefreqSorted = bb.getNoneFreqDic().OrderByDescending(o => o.Value).ToDictionary(p => p.Key, o => o.Value);
            foreach (var item in nonefreqSorted)//输出无音列表
            {
                swNoneFreq.WriteLine(item.Key + "\t" + item.Value);
            }

            foreach (var item in bb.getMultiPronList())
            {
                string word = item.Key;
                MultiPronWord mpw = item.Value;
                if (word.Length > 1 && item.Key.Equals(mpw.multiPronChars[0]))//如果为多音词
                {
                    string tempLine = word + "\t" + mpw.countNum + "\t1\t";
                    int num = 1;
                    foreach (string pron in bb.getOrgDic()[word])
                    {
                        if (num == 1)
                            tempLine += num + ":" + pron;
                        else
                            tempLine += "|" + num + ":" + pron;
                        num += 1;
                    }
                    swWSMultiFreq.WriteLine(tempLine);
                }
                else//为含有多音字的词
                {
                    foreach (string chara in mpw.multiPronChars)
                    {
                        string tempLine = chara + "\t" + word + "\t" + mpw.countNum + "\t1\t";
                        int num = 1;
                        foreach (string pron in bb.getCharDic()[chara])
                        {
                            if (num == 1)
                                tempLine += num + ":" + pron;
                            else
                                tempLine += "|" + num + ":" + pron;
                            num += 1;
                        }
                        swSWMultiFreq.WriteLine(tempLine);
                    }

                }
            }
                

             swNoneFreq.Close();
             swSWMultiFreq.Close(); 
             //swMWMultiFreq.Close();
             swWSMultiFreq.Close(); 
            //  Dictionary<string, int> charToWordsSorted = charToWords.OrderByDescending(o => o.Value).ToDictionary(p => p.Key, o => o.Value);
        }
        
        //</2.1 注音处理过程>

        //<2.2 对人工标注进行查错>


        public static void checkManualWork()//检查人工标中的错误
        {
            StreamWriter swWrongPron = new StreamWriter(wrongPronList,true);
            StreamWriter swPronWay = new StreamWriter(pronWayList,true);
            StreamWriter swWrongNum = new StreamWriter(wrongPronsNum,true);
            foreach (var element in bb.getManualDic())
            {
                if (element.Key.Length != element.Value.pros.Count)
                    swWrongNum.WriteLine(element.Key);
                int loc = 1;
                foreach (string pron in element.Value.pros)
                {
                    if (!bb.getPinyinOrg().Contains(pron.Substring(0,pron.Length-1)))
                        swWrongPron.WriteLine(element.Key + "\t" + pron+"\t位于第"+loc+"位");
                    string pronWay = pron.Substring(pron.Length - 1, 1);
                    if (!bb.getPronWay().Contains(pronWay))
                        bb.getPronWay().Add(pronWay);
                    loc += 1;
                }

            }
            foreach (string p in bb.getPronWay())
            {
                swPronWay.WriteLine(p);
            }
            swWrongPron.Close();
            swPronWay.Close();
            swWrongNum.Close();
        }




        public static void writeNewDicWithManual()//整合专有领域词典和人工标注，生成新词典
        {
            //List<string> checkDone = new List<string>();
            Dictionary<string, bool> checkDone = new Dictionary<string, bool>();//记录专有领域已经标注过的词

            foreach (string line in bb.getTargetList().Keys)
            {
                string keyW = "";
                foreach (char currentcode in line)
                {
                    if (currentcode > 19968 && currentcode < 40869)
                    {
                        keyW += currentcode;
                    }
                }
                
                if (keyW.Length <= 1)
                    continue;
                if (checkDone.ContainsKey(keyW))
                    continue;
                checkDone.Add(keyW,true);
                
                string pron = "";

                if (bb.getNewDic().Keys.Contains(keyW))
                {
                    pron += bb.getNewDic()[keyW];
                    sw.WriteLine(keyW + "\t<NULL>\t0\t" + bb.getNewDic()[keyW]);
                }
                else if (bb.getOrgDic().ContainsKey(keyW))
                {
                    pron += bb.getOrgDic()[keyW];
                    sw.WriteLine(keyW + "\t" + bb.getOrgDic()[keyW]);

                }
                else
                {
                    Console.WriteLine("error " + keyW);
                }

                
                XmlItem xitem = new XmlItem();
                    xitem.word = keyW;
                    xitem.partOfSpeech = "<NULL>";
                    xitem.pinyin = pron;
                    xitem.weight = 0.000000;
                if(bb.getXmlList().ContainsKey(keyW))
                    xitem.proId = bb.getXmlList()[keyW].Count+1;
                else
                    xitem.proId = 1;

                if(bb.getXmlList().ContainsKey(keyW))
                    bb.getXmlList()[keyW].Add(xitem);
                else
                {
                    List<XmlItem> tempXmlList = new List<XmlItem>();
                    tempXmlList.Add(xitem);
                    bb.getXmlList().Add(keyW, tempXmlList);
                }

            }


            foreach (var od in bb.getOrgDic())//专有领域词典中没有在待处理语料中出现的词
            {
                //foreach (string odv in od.Value)
                //{
                    if (!checkDone.ContainsKey(od.Key))
                    {
                        string role = "";
                        if (od.Key.Length > 1)//只输出词
                        {

                            if (!bb.getRoles().ContainsKey(od.Key))
                            {
                                foreach (string odv in od.Value)
                                {
                                    role = "<NULL>";
                                    sw.WriteLine(od.Key + "\t" + role + "\t" + 0 + "\t" + odv);
                                }
                            }
                            else
                            {
                                foreach (string s in bb.getRoles()[od.Key])
                                {
                                    sw.WriteLine(s);
                                    string[] tempS = s.Split('\t');

                                    XmlItem xitem = new XmlItem();
                                    xitem.word = tempS[0];
                                    xitem.partOfSpeech = tempS[1];
                                    xitem.pinyin = tempS[3];
                                    xitem.weight = 0.000000;
                                    if (bb.getXmlList().ContainsKey(od.Key))
                                        xitem.proId = bb.getXmlList()[od.Key].Count + 1;
                                    else
                                        xitem.proId = 1;

                                    if (bb.getXmlList().ContainsKey(od.Key))
                                        bb.getXmlList()[od.Key].Add(xitem);
                                    else
                                    {
                                        List<XmlItem> tempXmlList = new List<XmlItem>();
                                        tempXmlList.Add(xitem);
                                        bb.getXmlList().Add(od.Key, tempXmlList);
                                    }
                                }
                            }
                        }
                        
                        //checkDone.Add(od.Key,true);
                    }
                //}
            }
        }

        
        public static void writeWordXmlFile()//输出词xml字典
        {
            XmlDocument xmlDoc = new XmlDocument();
            //创建类型声明节点  
            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "gbk", "");//utf-8
            xmlDoc.AppendChild(node);
            //创建根节点  
            XmlNode root = xmlDoc.CreateElement("Dictionary");
            xmlDoc.AppendChild(root);
            XmlNode header = xmlDoc.CreateNode(XmlNodeType.Element, "DictionaryHeader", null);
            CreateNode(xmlDoc, header, "DictionaryLanguage", "zh-cn");
            CreateNode(xmlDoc, header, "DictionaryName", @"2.addWeight2dict\Total_Word_XianDai5_weight.dict");
            root.AppendChild(header);
            foreach (var item in bb.getXmlList())
            {
                if (item.Key.Length <= 1)
                    continue;
                XmlNode DictionaryEntry = xmlDoc.CreateNode(XmlNodeType.Element, "DictionaryEntry", null);
                CreateNode(xmlDoc, DictionaryEntry, "Word", item.Key);
                foreach (XmlItem e in item.Value)
                {
                    XmlNode Pronunciation = xmlDoc.CreateNode(XmlNodeType.Element, "Pronunciation", null);
                    CreateNode(xmlDoc, Pronunciation, "ProID", e.proId.ToString());
                    if(e.partOfSpeech.Contains("NULL"))
                        CreateNode(xmlDoc, Pronunciation, "PartOfSpeech","");
                    else
                        CreateNode(xmlDoc, Pronunciation, "PartOfSpeech", e.partOfSpeech);
                    CreateNode(xmlDoc, Pronunciation, "Weight", e.weight.ToString());
                    CreateNode(xmlDoc, Pronunciation, "PinYin", e.pinyin);
                    CreateNode(xmlDoc, Pronunciation, "BianDiao", "");
                    DictionaryEntry.AppendChild(Pronunciation);

                }
                root.AppendChild(DictionaryEntry);
            }
            try
            {
                xmlDoc.Save(wordXmlPath);
            }
            catch (Exception e)
            {
                //显示错误信息  
                Console.WriteLine(e.Message);
            }
        }
        public static void CreateNode(XmlDocument xmlDoc, XmlNode parentNode, string name, string value)
        {
            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
            node.InnerText = value;
            parentNode.AppendChild(node);
        }

        public static void checkFinalWork(string finalPath)//检查最终生成词典的错误
        {
            StreamWriter swFinalWrongPron = new StreamWriter(finalWrongPronList, true);
            StreamWriter swFinalWrongNum = new StreamWriter(finalWrongPronsNum, true);

            using (StreamReader sr = new StreamReader(finalPath))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    string[] tempLine = s.Split('\t');
                    string word = tempLine[0];
                    string[] pron = tempLine[3].Split('_');
                    if(word.Length != pron.Length)
                        swFinalWrongNum.WriteLine(word +"\t"+ tempLine[3]);
                    foreach (string p in pron)
                    {
                        if (!bb.getPinyinOrg().Contains(p.Substring(0, p.Length - 1)))
                            swFinalWrongPron.WriteLine(word + "\t" + tempLine[3]);
                    }

                }
            }

            
            swFinalWrongPron.Close();
            swFinalWrongNum.Close();
        }
    }
}
