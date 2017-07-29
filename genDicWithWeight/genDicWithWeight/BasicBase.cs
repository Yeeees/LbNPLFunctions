using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genDicWithWeight
{
    class BasicBase
    {
        private Dictionary<string, List<string>> orgCharDic = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> orgWordDic = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> newDic = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> dicToAppend = new Dictionary<string, List<string>>();
        private List<string> orgWord = new List<string>();
        public BasicBase()
        {

        }
        public Dictionary<string, List<string>> getOrgCharDic()
        {
            return orgCharDic;
        }
        public Dictionary<string, List<string>> getOrgWordDic()
        {
            return orgWordDic;
        }
        public Dictionary<string, List<string>> getNewDic()
        {
            return newDic;
        }
        public Dictionary<string, List<string>> getDicToAppend()
        {
            return dicToAppend;
        }
        public List<string> getOrgWord()
        {
            return orgWord;
        }
    }
}
