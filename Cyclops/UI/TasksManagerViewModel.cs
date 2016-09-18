using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using Cyclops.Contracts;
using Cyclops.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace Cyclops.UI
{
    public class TasksManagerViewModel : BindableBase
    {
        private readonly IExecutableTaskRepository _repository;
        private ObservableCollection<ExecutableTaskViewModel> _taskList = new ObservableCollection<ExecutableTaskViewModel>();

        #region Ctor
        public TasksManagerViewModel(IExecutableTaskRepository repository)
        {
            _repository = repository;
            AddTaskCommand = new DelegateCommand(AddNewTaskExecute);

            foreach (var task in _repository.GetAllExecutableTasks())
                _taskList.Add(new ExecutableTaskViewModel(task));
        }
        #endregion
        
        public ICommand AddTaskCommand { get; set; }

        public ObservableCollection<ExecutableTaskViewModel> TaskList
        {
            get { return _taskList; }
            set { SetProperty(ref _taskList, value); }
        }

        private void AddNewTaskExecute()
        {
            //TODO open display box
            var newTask = new ExecutableTask();
            var taskWindowViewModel = new TaskWindowViewModel(newTask);
            var taskWindow = new TaskWindowView(taskWindowViewModel);
            taskWindow.Topmost = true;
            taskWindow.ShowDialog();

            if (taskWindowViewModel.IsModificationValidated)
            {
                _repository.AddNewExecutableTask(newTask);
                _taskList.Add(new ExecutableTaskViewModel(newTask));
            }
        }
    }
}