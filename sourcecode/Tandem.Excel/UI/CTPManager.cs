using ExcelDna.Integration.CustomUI;
using System.Runtime.InteropServices;

namespace Tandem.Excel.UI
{
    [ComVisible(true)]
    internal static class CTPManager
    {
        static CustomTaskPane ctpWizard;
        static CustomTaskPane ctpWzBrowser;

        public static void ShowWizard()
        {
            if (ctpWizard == null)
            {
                ctpWizard = CustomTaskPaneFactory.CreateCustomTaskPane(typeof(WizardHost), "tdmWizard");
                ctpWizard.Visible = true;

                ctpWizard.DockPosition = MsoCTPDockPosition.msoCTPDockPositionFloating;
                ctpWizard.Height = 350;
                ctpWizard.Width = 800;
            }
            else
            {
                if (ctpWizard.Visible == false)
                {
                    ctpWizard.Visible = true;
                }
            }
        }

        public static void ShowWzBrowser()
        {
            ctpWzBrowser = CustomTaskPaneFactory.CreateCustomTaskPane(typeof(WzBrowserHost), "tdmBrowzer");
            ctpWzBrowser.Visible = true;
            ctpWzBrowser.DockPosition = MsoCTPDockPosition.msoCTPDockPositionFloating;
            ctpWzBrowser.Height = 350;
            ctpWzBrowser.Width = 500;
        }
    }
}
