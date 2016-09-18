using System.Collections.Generic;
using Cyclops.Models;

namespace Cyclops.Contracts
{
    public interface IConfigFileHandler
    {
        IEnumerable<ExecutableTask> GetTasks();
        void SaveTasks(IEnumerable<ExecutableTask> tasks);
    }
}