using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Tandem.Elements;

namespace Tandem.Excel.UI.Wizard
{
    /// <summary>
    /// Interaction logic for Wizard.xaml
    /// </summary>
    public partial class WizardControl : UserControl, INotifyPropertyChanged
    {
        private readonly TdmContainerPool _tdmContainerPool = TdmContainerPool.Instance;
        private readonly UserControlUtils _userControlUtils;
        private ObservableCollection<TdmElementContainer> _elements;

        public WizardControl()
        {
            InitializeComponent();
            _userControlUtils = new UserControlUtils();
            _elements = new ObservableCollection<TdmElementContainer>(_tdmContainerPool.PoolTable.Values);
            _tdmContainerPool.CollectionChanged += new NotifyCollectionChangedEventHandler(tdmPool_CollectionChanged);
        }

        public ObservableCollection<TdmElementContainer> Containers 
        { 
            get { return _elements; }
            set {
                this._elements = value;
                NotifyPropertyChanged("Containers");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public void tdmPool_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                string name = e.OldItems[0].ToString();
                _delete(name);
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                string name = e.NewItems[0].ToString();
                _update(name);
            }
        }

        private void _update(string name)
        {
            var tdmContainer = _tdmContainerPool.Get(name);

            if (!_elements.Any(o => o.Name.ToUpper() == name.ToUpper()))
            {
                _elements.Add(tdmContainer);
            }
        }

        private void _delete(string name)
        {
            var toUpdate = _elements.Where(o => o.Name.ToUpper() == name.ToUpper()).ToArray();
            foreach (var item in toUpdate)
            {
                _elements.Remove(item);
            }
        }

        private void tdmItemSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _elementList.Items.Filter = new Predicate<object>(NameFilter);
        }

        public bool NameFilter(object item)
        {
            var tdmContainer = item as TdmElementContainer;
            return (tdmContainer.Name.ToLower().Contains(_itemSearch.Text.ToLower()));
        }

        private void _elementList_DeleteKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var tdmContainer = ((ListBox)sender).SelectedItem as TdmElementContainer;
                if (tdmContainer != null)
                {
                    _tdmContainerPool.Remove(tdmContainer.Name);
                }
            }
        }

        private void _elementList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tdmContainer = ((ListBox)sender).SelectedItem as TdmElementContainer;
            if (tdmContainer != null)
            {
                try
                {
                    _elementViewer.Value = _userControlUtils._getWzElement(tdmContainer.Name).WzElements;
                }
                catch
                {
                }
            }  
        }
    }
}
