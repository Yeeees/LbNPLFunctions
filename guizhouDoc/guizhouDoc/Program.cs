using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HPSF;
using NPOI;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Collections;

namespace guizhouDoc
{
    class Program
    {
        public static BasicBase bb = new BasicBase();
        public static ExcelIO excelManager = new ExcelIO();
        public static string readFrom = "C:/Users/YLY/Desktop/灵伴/贵州现网商家全量库信息/贵州现网商家全量库信息.xlsx";
        public static string outPutAt = "C:/Users/YLY/Desktop/灵伴/guizhouDoc/贵州原表提取（安顺）.txt";
        public static StreamWriter sw = new StreamWriter(outPutAt, true);
        static void Main(string[] args)
        {
            excelManager.excelRead(readFrom, bb);
            int i = 1;
            foreach (string s in bb.getOrgLine())
            {
                sw.WriteLine(i+"\t"+s);
                i += 1;
            }
            sw.Close();
        }
    }
}
