using System;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using Cyclops.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace Cyclops.UI
{
    public class TaskWindowViewModel : BindableBase
    {
        private ExecutableTask _task;
        private ExecutableTask _tempTask;

        #region Ctor
        public TaskWindowViewModel(ExecutableTask task)
        {
            _task = task;
            _tempTask = new ExecutableTask(_task);

            OkCommand = new DelegateCommand(OkExecute);
        }
        #endregion

        public event Action OnRequestClose;

        public ExecutableTask CurrentTask
        {
            get { return _tempTask; }
            set { SetProperty(ref _tempTask, value); }
        }

        public ICommand OkCommand { get; set; }

        private void OkExecute()
        {
            _task.Name = _tempTask.Name;
            _task.ExecutableFullPathFullPath = _tempTask.ExecutableFullPathFullPath;
            _task.ExecutableArgs = _tempTask.ExecutableArgs;
            _task.ExecutionFolder = _tempTask.ExecutionFolder;
            _task.ExpectedReturnCode = _tempTask.ExpectedReturnCode;

            OnRequestClose?.Invoke();
        }
    }
}