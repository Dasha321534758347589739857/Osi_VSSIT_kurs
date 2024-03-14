using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using System.Collections.ObjectModel;
using Data;


namespace Osi_VSSIT_kurs.service
{
    public interface IExcel
    {
        
        List<InputParameters> ImportFromExcel();

    }
    public class Excel:IExcel
    {
        public string LoadFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Exel files (*.xlsx)|*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "";
        }
        public List<InputParameters> ImportFromExcel()
        {
            List<InputParameters> parameters = new List<InputParameters>();
            var fileName = LoadFile();
            if (fileName == "")
            {
                return parameters;
            }
            Microsoft.Office.Interop.Excel.Application ObjWorkExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(fileName);
            Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1-й лист
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell);//последнюю ячейку
            int lastRow = (int)lastCell.Row;
            List<(string, string, string)> tmpReader = new List<(string, string, string)>();//по всем колонкам
               for (int i = 0; i < lastRow; i++)
            {
                tmpReader.Add(new(ObjWorkSheet.Cells[i + 1, 1].Text.ToString(), ObjWorkSheet.Cells[i + 1, 2].Text.ToString(), ObjWorkSheet.Cells[i + 1, 3].Text.ToString()));
            }
            foreach (var elem in tmpReader)
            {
                try
                {
                    InputParameters parameter = new InputParameters();
                    parameter.Expenditure = int.Parse(elem.Item1);
                    parameter.Level = double.Parse(elem.Item2);
                    parameter.Concentration = double.Parse(elem.Item3);
                    parameters.Add(parameter);
                }
                catch
                {
                    MessageBox.Show(
                        "При считывании данных произошла ошибка, повторите попытку или выберете другой файл.");
                    parameters.Clear();
                    break;
                }
            }
            ObjWorkBook.Close(false, Type.Missing, Type.Missing);
            return parameters;
        }
    }
}

