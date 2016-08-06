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
            return new []
            {
                new ExecutableTask("Test1", @"c:\test1.exe"),
                new ExecutableTask("Test2", @"c:\test2.exe"),
                new ExecutableTask("Test3", @"c:\test3.exe")
            };
        }

        public void AddNewExecutableTask(ExecutableTask newTask)
        {
            throw new NotImplementedException();
        }
    }
}