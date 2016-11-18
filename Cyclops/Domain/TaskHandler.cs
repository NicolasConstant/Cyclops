using System;
using System.Diagnostics;
using System.Threading;
using Cyclops.Models;
using System.Threading.Tasks;

namespace Cyclops.Domain
{
    public class TaskHandler
    {
        private readonly ExecutableTaskRepository _taskRepository;
        private TasksGlobalStatusEnum _globalStatus;
        private Task _taskAnalyserTask;
        private CancellationTokenSource _cancellationTokenSource;

        public event Action<TasksGlobalStatusEnum> TasksGlobalStatusChanged;
        public event Action<AppWorkingStatusEnum> AppWorkingStatusChanged;

        #region Ctor
        public TaskHandler(ExecutableTaskRepository taskRepository)
        {
            _globalStatus = TasksGlobalStatusEnum.IsOk;
            _taskRepository = taskRepository;
        }
        #endregion

        public void StartAnalysing()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _taskAnalyserTask = new Task(() => ExecuteTaskAndChangeStatus(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
            _taskAnalyserTask.Start();
        }

        public void StopAnalysing()
        {
            _cancellationTokenSource.Cancel();
        }

        private void ExecuteTaskAndChangeStatus(CancellationToken token)
        {
            for (;;)
            {
                if (token.IsCancellationRequested) return;

                AppWorkingStatusChanged?.BeginInvoke(AppWorkingStatusEnum.IsWorking, null, null);
                PerformExecution();
                AppWorkingStatusChanged?.BeginInvoke(AppWorkingStatusEnum.IsIdle, null, null);

#if DEBUG
                Thread.Sleep(5 * 1000);
#else
                Thread.Sleep(60*60*1000);
#endif
            }
        }

        private void PerformExecution()
        {
            //Run all tasks to determine current status
            var isGlobalyFailed = false;
            foreach (var executableTask in _taskRepository.GetAllExecutableTasks())
            {
                executableTask.IsFailed = !IsExecutionSuccessful(executableTask);
                if (executableTask.IsFailed)
                {
                    if (_globalStatus == TasksGlobalStatusEnum.IsOk)
                    {
                        TasksGlobalStatusChanged?.BeginInvoke(TasksGlobalStatusEnum.IsFailed, null, null);
                        _globalStatus = TasksGlobalStatusEnum.IsFailed;
                    }

                    isGlobalyFailed = true;
                }
            }

            //If global status came back to normal
            if (!isGlobalyFailed && _globalStatus == TasksGlobalStatusEnum.IsFailed)
            {
                TasksGlobalStatusChanged?.BeginInvoke(TasksGlobalStatusEnum.IsOk, null, null);
                _globalStatus = TasksGlobalStatusEnum.IsOk;
            }
        }

        private bool IsExecutionSuccessful(ExecutableTask taskToRun)
        {
            var pInfo = new ProcessStartInfo
            {
                FileName = taskToRun.ExecutableFullPath,
                Arguments = taskToRun.ExecutableArgs,
                WorkingDirectory = taskToRun.ExecutionFolder,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            //var p = new Process();
            var p = Process.Start(pInfo);
            p?.WaitForExit(2 * 60 * 1000);
            var exitCode = p?.ExitCode;

            return exitCode == taskToRun.ExpectedReturnCode;
        }
    }
}