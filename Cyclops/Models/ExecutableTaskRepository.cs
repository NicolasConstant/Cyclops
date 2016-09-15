using System;
using System.Collections;
using System.Collections.Generic;
using Cyclops.Contracts;

namespace Cyclops.Models
{
    public class ExecutableTaskRepository : IExecutableTaskRepository
    {
        private readonly List<ExecutableTask> _list = new List<ExecutableTask>();

        #region Ctor
        public ExecutableTaskRepository()
        {
            for (var i = 0; i < 25; i++)
                _list.Add(new ExecutableTask($"Test{i}", $@"c:\test{i}.exe"));

            _list[3].IsFailed = true;
        }
        #endregion

        public IEnumerable<ExecutableTask> GetAllExecutableTasks()
        {
            return _list;
        }

        public void AddNewExecutableTask(ExecutableTask newTask)
        {
            throw new NotImplementedException();
        }
    }
}