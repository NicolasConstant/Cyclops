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
using System.Windows.Shapes;

namespace Cyclops.UI
{
    /// <summary>
    /// Interaction logic for TaskWindowView.xaml
    /// </summary>
    public partial class TaskWindowView : Window
    {
        public TaskWindowView(TaskWindowViewModel viewModel)
        {
            viewModel.OnRequestClose += Close;
            DataContext = viewModel;
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var taskWindowViewModel = DataContext as TaskWindowViewModel;
            if (taskWindowViewModel != null)
                taskWindowViewModel.OnRequestClose -= Close;
        }
    }
}
