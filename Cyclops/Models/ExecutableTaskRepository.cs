using System;
using System.Collections;
using System.Collections.Generic;
using Cyclops.Contracts;

namespace Cyclops.Models
{
    public class ExecutableTaskRepository : IExecutableTaskRepository
    {
        private readonly IConfigFileHandler _configFileHandler;
        private readonly List<ExecutableTask> _list = new List<ExecutableTask>();

        #region Ctor
        public ExecutableTaskRepository(IConfigFileHandler configFileHandler)
        {
            _configFileHandler = configFileHandler;
            _list.AddRange(_configFileHandler.GetTasks());
        }
        #endregion

        public IEnumerable<ExecutableTask> GetAllExecutableTasks()
        {
            return _list;
        }

        public void AddNewExecutableTask(ExecutableTask newTask)
        {
            //Add to repository list
            _list.Add(newTask);

            //Serialize
            _configFileHandler.SaveTasks(_list);
        }
    }
}