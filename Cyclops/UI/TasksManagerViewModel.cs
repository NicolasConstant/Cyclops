using System.Collections.ObjectModel;
using Cyclops.Contracts;
using Cyclops.Models;
using Prism.Mvvm;

namespace Cyclops.UI
{
    public class TasksManagerViewModel : BindableBase
    {
        private readonly IExecutableTaskRepository _repository;
        private ObservableCollection<ExecutableTask> _taskList = new ObservableCollection<ExecutableTask>();
        
        #region Ctor
        public TasksManagerViewModel(IExecutableTaskRepository repository)
        {
            _repository = repository;

            foreach (var task in _repository.GetAllExecutableTasks())
                _taskList.Add(task);
        }
        #endregion
        
        public ObservableCollection<ExecutableTask> TaskList
        {
            get { return _taskList; }
            set { SetProperty(ref _taskList, value); }
        }
    }
}