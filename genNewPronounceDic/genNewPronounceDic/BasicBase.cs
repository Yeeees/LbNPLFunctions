using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genNewPronounceDic
{
    class BasicBase
    {
        private List<string> orgDoc = new List<string>();//待处理词表
        private Dictionary<string, List<string>> orgDic = new Dictionary<string, List<string>>();//专有领域词典
        private Dictionary<string, WordItem> manualDic = new Dictionary<string, WordItem>(); // 词到音的映射表 -- 人工标注的结果
        private Dictionary<string,bool> targetList = new Dictionary<string,bool>();//待处理词表中所有出现的词
        private Dictionary<string, List<string>> charDic = new Dictionary<string, List<string>>();//通用领域字典
        private Dictionary<string, List<string>> wordsDic = new Dictionary<string, List<string>>();//通用领域词典
        private Dictionary<string, string> newDic = new Dictionary<string, string>();
        private Dictionary<string, List<string>> roles = new Dictionary<string, List<string>>();//专有领域词典中每行数据，用来添加所有词性
        private List<string> pinyinOrg = new List<string>();//全拼音列表
        private List<string> pronWay = new List<string>();//人工标注结果中所有出现的语调
        private List<string> newPinyin = new List<string>();//人工标注结果中出现的全拼音列表中不存在的发音
        private Dictionary<string, int> noneFreqDic = new Dictionary<string, int>();//无音字计数
        private Dictionary<string, MultiPronWord> multiPronList = new Dictionary<string, MultiPronWord>();//多音字计数
        //private Dictionary<string, int> wordsFreqDic = new Dictionary<string, int>();//多音词计数
        private Dictionary<string, List<string>> charToWords = new Dictionary<string, List<string>>();//字对词
        private Dictionary<string, List<XmlItem>> xmlList = new Dictionary<string, List<XmlItem>>();
        public BasicBase()
        { 
            
        }

        public Dictionary<string, List<XmlItem>> getXmlList()
        {
            return xmlList;
        }
        public List<string> getPinyinOrg()
        {
            return pinyinOrg;
        }
        public List<string> getPronWay()
        {
            return pronWay;
        }
        public List<string> getNewPinyin()
        {
            return newPinyin;
        }
        public List<string> getOrgDoc()
        {
            return orgDoc;
        }
        public Dictionary<string, int> getNoneFreqDic()
        {
            return noneFreqDic;
        }
        public Dictionary<string, MultiPronWord> getMultiPronList()
        {
            return multiPronList;
        }
        //public Dictionary<string, int> getWordsFreqDic()
        //{
        //    return wordsFreqDic;
        //}
        public Dictionary<string, List<string>> getCharToWords()
        {
            return charToWords;
        }
        public Dictionary<string, List<string>> getOrgDic()
        {
            return orgDic;
        }
        public Dictionary<string, string> getNewDic()
        {
            return newDic;
        }
        public Dictionary<string, bool> getTargetList()
        {
            return targetList;
        }
        public Dictionary<string, WordItem> getManualDic()
        {
            return manualDic;
        }
        public Dictionary<string, List<string>> getRoles()
        {
            return roles;
        }
        public Dictionary<string, List<string>> getCharDic()
        {
            return charDic;
        }
        public Dictionary<string, List<string>> getWordsDic()
        {
            return wordsDic;
        }
    }
}
