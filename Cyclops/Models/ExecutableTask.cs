using Prism.Mvvm;

namespace Cyclops.Models
{
    public class ExecutableTask : BindableBase
    {
        private string _name;
        private string _executableFullPath;
        private string _executableArgs;
        private string _executionFolder;
        private bool _isFailed;

        #region Ctor
        public ExecutableTask()
        {
            
        }

        public ExecutableTask(string name, string executableFullPath)
        {
            Name = name;
            ExecutableFullPathFullPath = executableFullPath;
        }
        #endregion

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string ExecutableFullPathFullPath
        {
            get { return _executableFullPath; }
            set { SetProperty(ref _executableFullPath, value); }
        }

        public string ExecutableArgs
        {
            get { return _executableArgs; }
            set { SetProperty(ref _executableArgs, value); }
        }

        public string ExecutionFolder
        {
            get { return _executionFolder; }
            set { SetProperty(ref _executionFolder, value); }
        }

        public bool IsFailed
        {
            get { return _isFailed; }
            set { SetProperty(ref _isFailed, value); }
        }
    }
}