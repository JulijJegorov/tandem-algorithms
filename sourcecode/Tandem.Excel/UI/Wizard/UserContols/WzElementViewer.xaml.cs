using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Tandem.Excel.UI.Wizard
{
    /// <summary>
    /// Interaction logic for WzElementViewer.xaml
    /// </summary>
    public partial class WzElementViewer : UserControl
    {

        /// <summary>
        /// Dependecy property of UserControl
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); } 
        }
        
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object),
                                           typeof(WzElementViewer),
                                           new PropertyMetadata(null)
                                           );
   
        public WzElementViewer()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void _dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridTextColumn column = e.Column as DataGridTextColumn;
            Binding binding = column.Binding as Binding;
            binding.Path = new PropertyPath(binding.Path.Path + ".Value");
        }

        private void _elementList_EnterPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _dispalyContent(sender);
            }
        }

        private void _dataGrid_OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _dispalyContent(sender);
        }

        private void _elementList_BackKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (_treeView.SelectedItem != null)
                {
                    var tdmItems = _treeView.Items[0] as IWzElement;
                    _findSelectedNode(tdmItems, tdmItems);
                }
            }
        }

        private void _findSelectedNode(IWzElement childNode, IWzElement parentNode)
        {
            if (childNode.IsSelected)
            {
                childNode.IsSelected = false;
                parentNode.IsSelected = true;
                return;
            }
            foreach (IWzElement treeItem in childNode.WzElements)
            {
                _findSelectedNode(treeItem, childNode);
            }
        }

        private void _dispalyContent(object sender)
        {
            try
            {
                string headerName = ((DataGrid)sender).SelectedCells[0].Column.Header.ToString();
                var selectedItem = ((DataGrid)sender).SelectedCells[0].Item as IWzElement;
                if (headerName == "Value" && selectedItem.IsLinked)
                {
                    var treeSelectedItem = _treeView.SelectedItem as IWzElement;
                    if (!treeSelectedItem.IsExpanded)
                    {
                        treeSelectedItem.IsExpanded = true;
                    }
                    selectedItem.IsExpanded = true;
                    selectedItem.IsSelected = true;
                }
            }
            catch { }
        }
    }
}
