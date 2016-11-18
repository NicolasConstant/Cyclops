using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cyclops.Annotations;

namespace Cyclops.Models
{
    public class ExecutableTask : INotifyPropertyChanged, ICloneable
    {
        private string _name;
        private string _executableFullPath;
        private string _executableArgs;
        private string _executionFolder;
        private bool _isFailed;
        private int _expectedReturnCode;

        #region Ctor
        public ExecutableTask()
        {
            
        }

        public ExecutableTask(ExecutableTask task)
        {
            Name = task.Name;
            ExecutableFullPath = task.ExecutableFullPath;
            ExecutableArgs = task._executableArgs;
            ExecutionFolder = task._executionFolder;
            ExpectedReturnCode = task._expectedReturnCode;
            IsFailed = task.IsFailed;
        }

        public ExecutableTask(string name, string executableFullPath)
        {
            Name = name;
            ExecutableFullPath = executableFullPath;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string ExecutableFullPath
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

        public int ExpectedReturnCode
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

        public object Clone()
        {
            var clonedObject = new ExecutableTask()
            {
                Id = Id,
                Name = Name,
                ExecutableArgs = ExecutableArgs,
                ExecutableFullPath = ExecutableFullPath,
                ExecutionFolder = ExecutionFolder,
                ExpectedReturnCode = ExpectedReturnCode,
                IsFailed = IsFailed
            };
            return clonedObject;
        }
    }
}