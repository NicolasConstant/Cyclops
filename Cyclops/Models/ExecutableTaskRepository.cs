using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var listCopy = new List<ExecutableTask>();
            foreach (var executableTask in _list)
            {
                listCopy.Add(executableTask.Clone() as ExecutableTask);
            }
            return listCopy;
        }

        public void AddNewExecutableTask(ExecutableTask newTask)
        {
            //Add to repository list
            newTask.Id = _list.Count;
            _list.Add(newTask);

            //Serialize
            _configFileHandler.SaveTasks(_list);
        }

        public void SaveModifiedTask(ExecutableTask newTask)
        {
            //Update repository list
            var oldTask = _list.Where(x => x.Id == newTask.Id).Select(x => x).FirstOrDefault();
            _list.Remove(oldTask);
            _list.Add(newTask);
            
            //Serialize
            _configFileHandler.SaveTasks(_list);
        }
    }
}