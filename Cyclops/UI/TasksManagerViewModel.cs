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
            OnDoubleClickCommand = new DelegateCommand(OnDoubleClickExecute);

            foreach (var task in _repository.GetAllExecutableTasks())
                _taskList.Add(new ExecutableTaskViewModel(task));
        }
        #endregion
        
        public ICommand AddTaskCommand { get; set; }
        public ICommand OnDoubleClickCommand { get; set; }

        public ObservableCollection<ExecutableTaskViewModel> TaskList
        {
            get { return _taskList; }
            set { SetProperty(ref _taskList, value); }
        }

        public ExecutableTaskViewModel SelectedTask { get; set; }

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

        private void OnDoubleClickExecute()
        {
            var selectedTask = SelectedTask.Model.Clone() as ExecutableTask;

            var taskWindowViewModel = new TaskWindowViewModel(selectedTask);
            var taskWindow = new TaskWindowView(taskWindowViewModel);
            taskWindow.Topmost = true;
            taskWindow.ShowDialog();

            if (taskWindowViewModel.IsModificationValidated)
            {
                var newViewModel = new ExecutableTaskViewModel(selectedTask);
                TaskList.Remove(SelectedTask);
                TaskList.Add(newViewModel);
                _repository.SaveModifiedTask(selectedTask);
            }
        }
    }
}