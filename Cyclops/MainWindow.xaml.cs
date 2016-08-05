using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using Cyclops.UI;

namespace Cyclops
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SysTrayWrapper _sysTrayWrapper;
        private TasksManager _tasksManagerWindow;
        private readonly object _tasksManagerLaunchingLock = new object();

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
            _sysTrayWrapper.LeftCLickOnTrayIconOccured += LeftClickOnTray;
        }


        private void LeftClickOnTray()
        {
            lock (_tasksManagerLaunchingLock)
            {
                if (_tasksManagerWindow == null)
                {
                    _tasksManagerWindow = new TasksManager();

                    var point = System.Windows.Forms.Cursor.Position;
                    _tasksManagerWindow.Left = point.X - _tasksManagerWindow.Width / 2;
                    _tasksManagerWindow.Top = point.Y - _tasksManagerWindow.Height;

                    _tasksManagerWindow.Topmost = true;
                    _tasksManagerWindow.ShowInTaskbar = false;
                    _tasksManagerWindow.Show();
                    _tasksManagerWindow.Activate();

                    _tasksManagerWindow.Deactivated += Unfocused;
                }
            }
        }

        private void Unfocused(object sender, EventArgs e)
        {
            _tasksManagerWindow.Deactivated -= Unfocused;
            _tasksManagerWindow.Close();
            _tasksManagerWindow = null;
        }
        
        private void OnClosing(object sender, CancelEventArgs e)
        {
            _sysTrayWrapper.LeftCLickOnTrayIconOccured -= LeftClickOnTray;
            _sysTrayWrapper.Dispose();
        }
    }
}
