using RetryAfterAdapter.Common;
using RetryAfterAdapter.Interfaces;
using System.Net.Http.Headers;

namespace RetryAfterAdapter.InMemoryProcessor
{
    public class InMemoryTaskRunner : ITaskRunner
    {
        private static readonly InMemoryTaskRunner instance = new InMemoryTaskRunner();
        private static Dictionary<string, bool> tasksRunning = new Dictionary<string, bool>();

        private InMemoryTaskRunner() { }

        public static InMemoryTaskRunner Instance => instance;

        public async Task RunTask(Task task, string taskName)
        {
            if (tasksRunning.ContainsKey(taskName) && tasksRunning[taskName])
                return;

            lock (tasksRunning)
            {
                if (tasksRunning.ContainsKey(taskName))
                {
                    tasksRunning[taskName] = true;
                }
                else
                {
                    tasksRunning.Add(taskName, true);
                }
            }

            try
            {
                await task;
                FinalizeTask(taskName);
            }
            catch (Exception)
            {
                RemoveTask(taskName);
            }
        }

        public void FinalizeTask(string taskName)
        {
            if (!Exists(taskName)) return;

            lock (tasksRunning)
            {
                tasksRunning[taskName] = false;
            }
        }

        public void RemoveTask(string taskName)
        {
            lock (tasksRunning)
            {
                tasksRunning.Remove(taskName);
            }
        }

        public bool Exists(string name)
        {
            return tasksRunning.ContainsKey(name);
        }

        public bool IsRunning(string name)
        {
            return tasksRunning[name];
        }
    }
}
