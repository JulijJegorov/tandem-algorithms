using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Tandem.Excel.UI.Wizard;

namespace Tandem.Excel.UI
{
    [ComVisible(true)]
    public partial class WzBrowserHost : UserControl
    {
        public Label label;
        public WzBrowserControl browser = new WzBrowserControl();

        public WzBrowserHost()
        {
            ElementHost host = new ElementHost()
            {
                Child = browser,
                Dock = DockStyle.Fill,
            };
            label = new Label();
            Controls.Add(host);
        }
    }
}
