using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using Cyclops.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;

namespace Cyclops.UI
{
    public class TaskWindowViewModel : BindableBase
    {
        private ExecutableTask _task;
        private ExecutableTask _uiTask;

        #region Ctor
        public TaskWindowViewModel(ExecutableTask task)
        {
            _task = task;
            _uiTask = new ExecutableTask(_task);

            OkCommand = new DelegateCommand(OkExecute);
            CancelCommand = new DelegateCommand(CancelExecute);
            TestCommand = new DelegateCommand(TestExecute);
            SelectExecutableCommand = new DelegateCommand(SelectExecutableExecute);
            SelectExecutionFolderCommand = new DelegateCommand(SelectExecutionFolderExecute);
        }
        #endregion

        public event Action OnRequestClose;

        public ExecutableTask CurrentTask
        {
            get { return _uiTask; }
            set { SetProperty(ref _uiTask, value); }
        }

        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand TestCommand { get; set; }
        public ICommand SelectExecutableCommand { get; set; }
        public ICommand SelectExecutionFolderCommand { get; set; }

        public bool IsModificationValidated { get; private set; }

        private void OkExecute()
        {
            _task.Name = _uiTask.Name;
            _task.ExecutableFullPath = _uiTask.ExecutableFullPath;
            _task.ExecutableArgs = _uiTask.ExecutableArgs;
            _task.ExecutionFolder = _uiTask.ExecutionFolder;
            _task.ExpectedReturnCode = _uiTask.ExpectedReturnCode;

            IsModificationValidated = true;
            OnRequestClose?.Invoke();
        }

        private void CancelExecute()
        {
            OnRequestClose?.Invoke();
        }

        private void TestExecute()
        {
            throw new NotImplementedException();
        }

        private void SelectExecutableExecute()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false
            };
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                var exeFullPath = dialog.FileName;
                if (File.Exists(exeFullPath))
                {
                    _uiTask.ExecutableFullPath = exeFullPath;
                }
            }
        }

        private void SelectExecutionFolderExecute()
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
            var result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                var dirFullPath = dialog.FileName;
                if (Directory.Exists(dirFullPath))
                {
                    _uiTask.ExecutionFolder = dirFullPath;
                }
            }
        }
    }
}