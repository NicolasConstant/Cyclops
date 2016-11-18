using System.Collections.Generic;
using Cyclops.Models;

namespace Cyclops.Contracts
{
    public interface IExecutableTaskRepository
    {
        IEnumerable<ExecutableTask> GetAllExecutableTasks();
        void AddNewExecutableTask(ExecutableTask newTask);
        void SaveModifiedTask(ExecutableTask selectedTask);
    }
}