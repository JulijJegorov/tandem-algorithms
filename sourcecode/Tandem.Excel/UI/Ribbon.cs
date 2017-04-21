using ExcelDna.Integration.CustomUI;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using Tandem.Elements;
using Tandem.Elements.Utilities;
using YamlDotNet.RepresentationModel;

namespace Tandem.Excel.UI
{
    [ComVisible(true)]
    public class Ribbon : ExcelRibbon
    {
        public void OnShowWizard(IRibbonControl control)
        {
            CTPManager.ShowWizard();
        }

        public void OnClearMemory(IRibbonControl control)
        {
            TdmContainerPool.Instance.RemoveAll();
        }


        public void OnSpaceConnectionChange(IRibbonControl control, string index)
        {
            var connections = (YamlSequenceNode)ConfigFile.Instance.SpaceConnections;

            foreach (YamlMappingNode node in connections)
            {
                if (node.Children[new YamlScalarNode("name")].ToString() == index)
                {
                    string uri = node.Children[new YamlScalarNode("uri")].ToString();
                    Space.Instance.Connect(uri);
                    return;
                }
            }
        }

        public int getItemCount(IRibbonControl control)
        {
            return ConfigFile.Instance.SpaceConnections.Children.Count;
        }

        public int getShowImage(IRibbonControl control)
        {
            return ConfigFile.Instance.SpaceConnections.Children.Count;
        }

        public string getItemLabel(IRibbonControl control, int index)
        {
            var item = (YamlMappingNode)ConfigFile.Instance.SpaceConnections.Children[index];
            return item.Children[new YamlScalarNode("name")].ToString();
        }

        public Bitmap getItemImage(IRibbonControl control, int index)
        {
            var item = (YamlMappingNode)ConfigFile.Instance.SpaceConnections.Children[index];
            string image = item.Children[new YamlScalarNode("image")].ToString();

            string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new Bitmap(path + "\\img\\" + image);
        }

        public string getLabel(IRibbonControl control)
        {
            var connection = (YamlMappingNode)ConfigFile.Instance.SpaceDefaultConnection;
            return connection.Children[new YamlScalarNode("name")].ToString();
        }
    }
}
