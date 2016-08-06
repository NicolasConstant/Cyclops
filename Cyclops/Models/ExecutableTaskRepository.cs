using System;
using System.Collections;
using System.Collections.Generic;
using Cyclops.Contracts;

namespace Cyclops.Models
{
    public class ExecutableTaskRepository : IExecutableTaskRepository
    {
        #region Ctor
        public ExecutableTaskRepository()
        {
            
        }
        #endregion

        public IEnumerable<ExecutableTask> GetAllExecutableTasks()
        {
            var list = new List<ExecutableTask>();

            for (var i = 0; i < 25; i++)
                list.Add(new ExecutableTask($"Test{i}", $@"c:\test{i}.exe"));

            return list;
        }

        public void AddNewExecutableTask(ExecutableTask newTask)
        {
            throw new NotImplementedException();
        }
    }
}