using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Tandem.Excel.UI.Wizard;

namespace Tandem.Excel.UI
{
    [ComVisible(true)]
    public partial class WizardHost : UserControl
    {
        public Label label;
        public WizardControl wizard = new WizardControl();

        public WizardHost()
        {
            ElementHost host = new ElementHost()
            {
                Child = wizard,
                Dock = DockStyle.Fill,
            };

            label = new Label();
            Controls.Add(host);
        }
    }
}
