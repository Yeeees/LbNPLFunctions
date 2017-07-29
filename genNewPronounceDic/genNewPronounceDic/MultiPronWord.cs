using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genNewPronounceDic
{
    class MultiPronWord
    {
        public int countNum;
        public List<string> multiPronChars = new List<string>();
        public MultiPronWord(string chara)
        { 
            countNum = 1;
            multiPronChars.Add(chara);
        }
    }
}
