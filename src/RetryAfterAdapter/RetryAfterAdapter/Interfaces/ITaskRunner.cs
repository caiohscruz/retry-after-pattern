using RetryAfterAdapter.Common;

namespace RetryAfterAdapter.Interfaces
{
    public interface ITaskRunner
    {
        public Task RunTask (Task task, string taskName);
        public bool Exists(string taskName);
        public bool IsRunning(string taskName);
        public void RemoveTask(string taskName);
        public void FinalizeTask(string taskName);

    }
}