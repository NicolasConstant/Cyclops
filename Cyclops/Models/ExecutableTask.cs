using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cyclops.Annotations;

namespace Cyclops.Models
{
    public class ExecutableTask : INotifyPropertyChanged
    {
        private string _name;
        private string _executableFullPath;
        private string _executableArgs;
        private string _executionFolder;
        private bool _isFailed;
        private string _expectedReturnCode;

        #region Ctor
        public ExecutableTask()
        {
            
        }

        public ExecutableTask(ExecutableTask task)
        {
            Name = task.Name;
            ExecutableFullPathFullPath = task.ExecutableFullPathFullPath;
            ExecutableArgs = task._executableArgs;
            ExecutionFolder = task._executionFolder;
            ExpectedReturnCode = task._expectedReturnCode;
            IsFailed = task.IsFailed;
        }

        public ExecutableTask(string name, string executableFullPath)
        {
            Name = name;
            ExecutableFullPathFullPath = executableFullPath;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string ExecutableFullPathFullPath
        {
            get { return _executableFullPath; }
            set
            {
                _executableFullPath = value;
                OnPropertyChanged();
            }
        }

        public string ExecutableArgs
        {
            get { return _executableArgs; }
            set
            {
                _executableArgs = value;
                OnPropertyChanged();
            }
        }

        public string ExecutionFolder
        {
            get { return _executionFolder; }
            set
            {
                _executionFolder = value;
                OnPropertyChanged();
            }
        }

        public bool IsFailed
        {
            get { return _isFailed; }
            set
            {
                _isFailed = value;
                OnPropertyChanged();
            }
        }

        public string ExpectedReturnCode
        {
            get { return _expectedReturnCode; }
            set
            {
                _expectedReturnCode = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}