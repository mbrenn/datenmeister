using BurnSystems.Collections;
using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.BackgroundWorkerM.Interface;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.BackgroundWorkerM.Logic
{
    /// <summary>
    /// Implements the interface IBackgroundWorker
    /// </summary>
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class BackgroundWorker : IBackgroundWorker, IFlexBgRuntimeModule
    {
        /// <summary>
        /// Stores the worker dictionary
        /// </summary>
        private AutoDictionary<WorkerItem> workers = new AutoDictionary<WorkerItem>();

        /// <summary>
        /// Stores the activation container
        /// </summary>
        private ActivationContainer container;
        /// <summary>
        /// Stores the logger
        /// </summary>
        private ILog logger = new ClassLogger(typeof(BackgroundWorker));

        /// <summary>
        /// Defines the backgroundthread
        /// </summary>
        public Thread backgroundThread;

        /// <summary>
        /// Gets or sets a value whether the thread had been cancelled
        /// </summary>
        private bool isCancelled = false;

        /// <summary>
        /// Stores the synchronisation root
        /// </summary>
        private object syncRoot = new object();

        /// <summary>
        /// This event is signalled, when something has happened. 
        /// Like a timer, that ran up or anything else
        /// </summary>
        private AutoResetEvent actionEvent = new AutoResetEvent(false);

        /// <summary>
        /// Initializes a new instance of the BackgroundWorker
        /// </summary>
        /// <param name="container">Activation container to be used</param>
        public BackgroundWorker(ActivationContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Starts the background worker
        /// </summary>
        public void Start()
        {
            lock (this.syncRoot)
            {
                if (this.backgroundThread != null)
                {
                    throw new InvalidOperationException("BackgroundThread is already set");
                }

                var backgroundThread = new Thread(CallbackThread);
                backgroundThread.Name = "FlexBG BackgroundWorker";
                backgroundThread.Start();

                this.backgroundThread = backgroundThread;

                logger.LogEntry(LogLevel.Message, "BackgroundWorker started");
            }
        }

        /// <summary>
        /// Stops the backgroundworker
        /// </summary>
        public void Shutdown()
        {
            var backgroundThread = this.backgroundThread;

            lock (this.syncRoot)
            {
                if (this.backgroundThread == null)
                {
                    throw new InvalidOperationException("No BackgroundThread is running");
                }

                this.isCancelled = true;
                this.actionEvent.Set();
                this.backgroundThread = null;
            }

            backgroundThread.Join();

            logger.LogEntry(LogLevel.Message, "BackgroundWorker left");
        }

        /// <summary>
        /// Callbackmethod for thread
        /// </summary>
        public void CallbackThread()
        {
            while (true)
            {
                this.actionEvent.WaitOne(TimeSpan.FromSeconds(1.0));

                lock (this.syncRoot)
                {
                    if (this.isCancelled)
                    {
                        logger.LogEntry(LogLevel.Message, "Leaving backgroundworker");
                        break;
                    }
                }

                // Ok, check for all items having the assumed next time not in future
                var workers = GetAllOverDueWorkers().ToList();
                
                // Executes all the workers via task parallel
                Parallel.ForEach(workers, x =>
                {
                    using (var block = new ActivationBlock("BackgroundWorker", this.container))
                    {
                        x.Execute(block);
                    }
                });
            }
        }

        /// <summary>
        /// Gets all workers, which are overdue
        /// </summary>
        private IEnumerable<WorkerItem> GetAllOverDueWorkers()
        {
            var now = DateTime.Now;
            foreach (var worker in this.workers.Where(x => x.Value.AssumedNextTime < now))
            {
                var nextTime = worker.Value.GetNextTime(this.container);
                worker.Value.AssumedNextTime = nextTime;

                if (nextTime < now)
                {
                    yield return worker.Value;
                }
            }
        }

        /// <summary>
        /// Adds a worker to the background thread
        /// </summary>
        /// <param name="id">Id of the worker to be added. This id will also be used for removal</param>
        /// <param name="backgroundTask">Function, which retrieves the next time event. 
        /// If the time has elapsed, this method will be reasked, so action is not called when the timing has changed</param>
        /// <param name="action">Action which shall be called</param>
        public void Add(string id, IBackgroundTask backgroundTask)
        {
            Ensure.That(backgroundTask != null);

            var worker = new WorkerItem();
            worker.Key = id;
            worker.Task = backgroundTask;
            worker.AssumedNextTime = DateTime.MinValue; // We cannot ask now. FlexBG has not started yet

            lock (this.syncRoot)
            {
                this.workers.Add(worker);
            }
        }

        /// <summary>
        /// Removes a specific worker
        /// </summary>
        /// <param name="id">Id of the worker to be removed</param>
        public void Remove(string id)
        {
            lock (this.syncRoot)
            {
                var worker = this.workers[id];
                this.workers.Remove(id);
            }
        }
    }
}
