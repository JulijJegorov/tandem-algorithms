using ExcelDna.Integration;
using System.Windows.Controls;
using Tandem.Elements;
using InteropExcel = Microsoft.Office.Interop.Excel;

namespace Tandem.Excel.UI.Wizard
{
    /// <summary>
    /// Interaction logic for WizardSingle.xaml
    /// </summary>
    public partial class WzBrowserControl : UserControl
    {
        private readonly TdmContainerPool _tdmContainerPool = TdmContainerPool.Instance;
        private readonly UserControlUtils _userControlUtils = new UserControlUtils();

        public WzBrowserControl()
        {
           InitializeComponent();
           InititializeControl();
        }

        public void InititializeControl()
        {
            InteropExcel.Application app = (InteropExcel.Application)ExcelDnaUtil.Application;
            InteropExcel.Range activeCell = app.ActiveCell;

            string name = activeCell.Value.ToString();

            int separator = name.IndexOf(":");
            if (separator != -1)
                name = name.Substring(0, separator);

            _elementViewer.Value = _userControlUtils._getWzElement(name).WzElements; 
        }
    }
}
