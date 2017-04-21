using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using Tandem.Excel.UI;
using InteropExcel = Microsoft.Office.Interop.Excel;

namespace Tandem.Excel
{
    class TdmExcelAddIn : IExcelAddIn
    {
        public void AutoOpen()
        {
            LoadCommandBars();
            ExcelAsyncUtil.Initialize();
            ExcelIntegration.RegisterUnhandledExceptionHandler(
                delegate(object ex) { return string.Format("tdmError: {0}", ex.ToString()); });
        }

        public void AutoClose()
        {
            ExcelAsyncUtil.Uninitialize();
        }

        public void LoadCommandBars()
        {
            ExcelCommandBarUtil.LoadCommandBars(
                    @"<commandBars xmlns='http://schemas.excel-dna.net/office/2003/01/commandbars' >
                          <commandBar name='Cell'>
                            <button before='1' caption='tdmBrowser' faceId='25' enabled='true'  onAction='OnShowWizardContext' ></button>
                          </commandBar>
                        </commandBars>");
        }
    }

    public static class TestMacros
    {
        public static void OnShowWizardContext()
        {
            InteropExcel.Application app = (InteropExcel.Application)ExcelDnaUtil.Application;
            InteropExcel.Range activeCell = app.ActiveCell;
            string origFormula = (string)app.ActiveCell.Value;

            CTPManager.ShowWzBrowser();
        }
    }
}
