using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VintageGarmentDescriber
{
    /// <summary>
    /// Interaction logic for FolderBrowserDialog.xaml
    /// </summary>
    public partial class FolderBrowserDialog : Window
    {
        private BrowserViewModel _viewModel;

        public BrowserViewModel ViewModel
        {
            get
            { 
                return _viewModel = _viewModel ?? new BrowserViewModel();
            }
        }

        public string SelectedFolder
        {
            get
            {
                return ViewModel.SelectedFolder;
            }
        }

        public FolderBrowserDialog()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
