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
    class ExcelIO
    {
        XSSFWorkbook wb; 
        public ExcelIO()
        { 
        
        }

        public void excelRead(string path, BasicBase bb)
        {

            try
            {
                wb = new XSSFWorkbook();

                wb.Clear();
                bb.getOrgLine().Clear();

                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    wb = new XSSFWorkbook(fs);
                    fs.Close();
                }

                foreach (XSSFSheet sh in wb)
                {
                    readRows(sh.SheetName,bb);
                    

                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void readRows(string sheetName, BasicBase bb)
        {
            try
            {
                ISheet sheet = wb.GetSheet(sheetName);
                IRow headerRow = sheet.GetRow(0);
                IEnumerator rows = sheet.GetRowEnumerator();

                int colCount = headerRow.LastCellNum;
                int rowCount = sheet.LastRowNum;

                for (int c = 0; c < colCount; c++)
                { 
                }

                while (rows.MoveNext())
                {
                    if (rows.Current.Equals(headerRow))
                        continue;

                    IRow row = (XSSFRow)rows.Current;
                    string tempRow = "";
                    if (row.GetCell(1).ToString().Equals("安顺"))
                    {

                        for (int i = 0; i < colCount; i++)
                        {
                            ICell cell = row.GetCell(i);

                            if (cell != null)
                            {
                                string tempCell = cell.ToString();
                                switch (i)
                                {
                                    
                                    //case 1: tempRow += tempCell; break;
                                    case 2: tempRow += "单位名称," + tempCell; break;
                                    //case 3: tempRow += "," + tempCell; break;
                                    //case 4: String[] temp = tempCell.Split('|');
                                    //    foreach (string s in temp)
                                    //    {
                                    //        tempRow += "," + s;
                                    //    }
                                    //    break;
                                    case 7: tempRow += ",地址," + tempCell; break;
                                    default: break;
                                }



                            }



                        }
                        if (!bb.getOrgLine().Contains(tempRow))
                            bb.getOrgLine().Add(tempRow);
                    }
                }

            }
            catch (Exception ex)
            {
                
                Console.WriteLine("{0} Exception caught.", ex);
            }
        }

        public DataTable genDT(string sheetName)
        {

            XSSFWorkbook workbook = wb;

            ISheet sheet = workbook.GetSheet(sheetName);

            DataTable dt = new DataTable();
            try
            {
                IRow headerRow = sheet.GetRow(0);
                IEnumerator rows = sheet.GetRowEnumerator();

                int colCount = headerRow.LastCellNum;
                int rowCount = sheet.LastRowNum;

                for (int c = 0; c < colCount; c++)
                    dt.Columns.Add(headerRow.GetCell(c).ToString());

                while (rows.MoveNext())
                {
                    if (rows.Current.Equals(headerRow))
                        continue;

                    IRow row = (XSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();

                    for (int i = 0; i < colCount; i++)
                    {
                        ICell cell = row.GetCell(i);

                        if (cell != null)
                        {
                            dr[i] = cell.ToString();

                        }


                    }
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }
            return dt;
        }

    }
}
