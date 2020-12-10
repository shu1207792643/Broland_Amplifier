using System;
using System.Data;
using System.Windows;

namespace Broland_Amplifier_Wpf.Helper
{
    public class ExcelHelper
    {
        #region 保存到excel表 会根据datatable表 自动生成表头

        public void ToExcel(DataTable dt)
        {
            #region 初始化 生成Worksheet
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook excelWB = excelApp.Workbooks.Add(System.Type.Missing);    //创建工作簿（WorkBook：即Excel文件主体本身）  
            Microsoft.Office.Interop.Excel.Worksheet excelWS = (Microsoft.Office.Interop.Excel.Worksheet)excelWB.Worksheets[1];   //创建工作表（即Excel里的子表sheet） 1表示在子表sheet1里进行数据导出  
            #endregion

            #region 新增一行用于保存标题
            DataRow dr = dt.NewRow();
            //sdt表中有的数据是int类型的这样当插入标题行的时候会提示类型不同 
            //所以在这里只是插入一个空行 然后标题列在excel表里设置
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    //    MessageBox.Show(dr[i].GetType().ToString());
            //    //dr[i] = 1;
            //}
            dt.Rows.InsertAt(dr, 0);
            #endregion

            #region 把datatable表中数据写入到 Worksheet中
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    excelWS.Cells[i + 1, j + 1] = dt.Rows[i][j].ToString();   //Excel单元格第一个从索引1开始  
                }
            }
            #endregion

            #region 通过excel的方法编辑标题行
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                excelWS.Cells[1, i + 1] = dt.Columns[i].ColumnName; ; //Excel单元格赋值
            }
            #endregion

            #region 打开保存框
            //打开 SaveFileDialog 框
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //定义导出的文件名称
            String FlName = "报名信息_"
                + DateTime.Now.Year.ToString() + "-"
                + DateTime.Now.Month.ToString() + "-"
                + DateTime.Now.Day.ToString() + "_"
                + DateTime.Now.Hour.ToString() + "-"
                + DateTime.Now.Minute.ToString();
            dlg.FileName = FlName;
            dlg.DefaultExt = ".xlsx"; // Default file extension
            dlg.Filter = "Excel documents (.xlsx)|*.xlsx"; // Filter files by extension
            #endregion

            #region 保存到文件
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;

                if (dlg.FileName == "")
                {
                    MessageBox.Show("请输入保存文件名！");
                }
                else
                {
                    excelWB.SaveAs(filename);  //将其进行保存到指定的路径  
                    excelWB.Close();
                    excelApp.Quit();

                    //释放可能还没释放的进程  
                    //该方法会导致 如果你已经打开一个excel啦 执行该方法时 会导致你打开的这个excel关闭。
                    //KillAllExcel(excelApp);

                    //显示保存的地址
                    MessageBox.Show("您需要的Excel文件已经保存到" + dlg.FileName);
                }
            }
            #endregion
        }
        #endregion

        #region 释放Excel进程 目前没调用该方法 会关闭所有打开的excel
        public bool KillAllExcel(Microsoft.Office.Interop.Excel.Application excelApp)
        {
            try
            {
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                    //释放COM组件，其实就是将其引用计数减1      
                    //System.Diagnostics.Process theProc;      
                    foreach (System.Diagnostics.Process theProc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                    {
                        //先关闭图形窗口。如果关闭失败.有的时候在状态里看不到图形窗口的excel了，      
                        //但是在进程里仍然有EXCEL.EXE的进程存在，那么就需要释放它      
                        if (theProc.CloseMainWindow() == false)
                        {
                            theProc.Kill();
                        }
                    }
                    excelApp = null;
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

    }
}
