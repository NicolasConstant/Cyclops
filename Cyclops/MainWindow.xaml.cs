using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using Cyclops.Domain;
using Cyclops.Models;
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
        private TasksManagerView _tasksManagerWindow;
        private readonly TasksManagerViewModel _viewModel;
        private readonly ExecutableTaskRepository _taskRepository;
        private readonly TaskHandler _taskHandler;
        private const string AppName = "Cyclops";

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
            
            var jsonConfigFile = new JsonConfigFileHandler();
            _taskRepository = new ExecutableTaskRepository(jsonConfigFile);
            _taskHandler = new TaskHandler(_taskRepository);
            _taskHandler.TasksGlobalStatusChanged += AppStatusChanged;
            _taskHandler.AppWorkingStatusChanged += AppWorkingStatusChanged;
            
            _viewModel = new TasksManagerViewModel(_taskRepository);

            var fullPath = Assembly.GetExecutingAssembly().Location;
            var autostartManager = new AutoStartManager(AppName, fullPath);
            _sysTrayWrapper = new SysTrayWrapper(autostartManager);
            _sysTrayWrapper.LeftCLickOnTrayIconOccured += LeftClickOnTray;
            _sysTrayWrapper.CloseAction += CloseAction;

            _taskHandler.StartAnalysing();
        }
        
        private AppWorkingStatusEnum _currentWorkingStatus;
        private TasksGlobalStatusEnum _currentErrorStatus;

        private void AppWorkingStatusChanged(AppWorkingStatusEnum status)
        {
            if (status == AppWorkingStatusEnum.IsIdle)
            {
                _currentWorkingStatus = AppWorkingStatusEnum.IsIdle;

                //Only show working animation if not failed
                if(_currentErrorStatus == TasksGlobalStatusEnum.IsOk)
                    _sysTrayWrapper.StopWorkingAnimation();
            }
            else if(status == AppWorkingStatusEnum.IsWorking)
            {
                _currentWorkingStatus = AppWorkingStatusEnum.IsWorking;

                //Only show working animation if not failed
                if (_currentErrorStatus == TasksGlobalStatusEnum.IsOk)
                    _sysTrayWrapper.StartWorkingAnimation();
            }
        }

        private void AppStatusChanged(TasksGlobalStatusEnum status)
        {
            if (status == TasksGlobalStatusEnum.IsOk)
            {
                _currentErrorStatus = TasksGlobalStatusEnum.IsOk;
                _sysTrayWrapper.StopWarningAnimation();
            }
            else if(status == TasksGlobalStatusEnum.IsFailed)
            {
                _currentErrorStatus = TasksGlobalStatusEnum.IsFailed;

                //Desactivate working animation if running
                if (_currentWorkingStatus == AppWorkingStatusEnum.IsWorking)
                    _sysTrayWrapper.StopWorkingAnimation();
                
                _sysTrayWrapper.StartWarningAnimation();
            }
        }
        
        private void CloseAction()
        {
            Application.Current.Shutdown();
        }

        private void LeftClickOnTray()
        {
            if (_tasksManagerWindow == null)
            {
                _tasksManagerWindow = new TasksManagerView(_viewModel);

                var point = System.Windows.Forms.Cursor.Position;
                _tasksManagerWindow.Left = point.X - _tasksManagerWindow.Width / 2;
                _tasksManagerWindow.Top = point.Y - _tasksManagerWindow.Height;

                _tasksManagerWindow.Topmost = true;
                _tasksManagerWindow.ShowInTaskbar = false;
                _tasksManagerWindow.Show();
                _tasksManagerWindow.Activate();
                
                _tasksManagerWindow.Deactivated += Unfocused;
            }

            ////DEBUG
            //var task = _taskRepository.GetAllExecutableTasks().ToArray()[3];
            //Task.Run(() =>
            //{
            //    for (;;)
            //    {
            //        Application.Current.Dispatcher.Invoke(delegate {
            //            task.IsFailed = !task.IsFailed;
            //        });
            //        Thread.Sleep(2000);
            //    }
            //});
        }

        private void Unfocused(object sender, EventArgs e)
        {
            Thread.Sleep(200); //Make sure unfocused event is processed after left click event //TODO change behavior of SysTrayWrapper
            _tasksManagerWindow.Deactivated -= Unfocused;
            _tasksManagerWindow.Close();
            _tasksManagerWindow = null;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            _taskHandler.StopAnalysing();

            _sysTrayWrapper.LeftCLickOnTrayIconOccured -= LeftClickOnTray;
            _sysTrayWrapper.Dispose();
        }
    }
}
