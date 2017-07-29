using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace guizhouDoc
{
    class BasicBase
    {
        private List<string> orgLine = new List<string>();
        public BasicBase()
        { 
        
        }
        public List<string> getOrgLine()
        {
            return orgLine;
        }
    }
}
