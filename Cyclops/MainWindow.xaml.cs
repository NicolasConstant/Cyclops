using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Cyclops.Tools;

namespace Cyclops
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SysTrayWrapper _sysTrayWrapper;

        public MainWindow()
        {
            InitializeComponent();

            #region Hide
            WindowState = WindowState.Minimized;
            Visibility = Visibility.Hidden;
            ShowInTaskbar = false;
            #endregion

            #region Events Subscription
            Closing += OnClosing;
            #endregion

            _sysTrayWrapper = new SysTrayWrapper();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            _sysTrayWrapper.Dispose();
        }
    }
}
