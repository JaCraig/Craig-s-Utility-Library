using System;
using System.Linq;
using Utilities.IO;
using Utilities.IO.Logging.Enums;
using UtilitiesSplitter.Tasks.Interfaces;

namespace UtilitiesSplitter
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            bool PushToNuget = args.Length > 0 && args[0] == "Push";
            Log.Get().LogMessage("Starting application", MessageType.Info);
            Utilities.IoC.Manager.Bootstrapper.RegisterAll<ITask>();
            var Tasks = Utilities.IoC.Manager.Bootstrapper.ResolveAll<ITask>().OrderBy(x => x.Order);
            foreach (var Task in Tasks)
            {
                try
                {
                    Log.Get().LogMessage("Running task '{0}'", MessageType.Info, Task.Name);
                    if (!Task.Run(PushToNuget))
                        throw new Exception("Error running task " + Task.Name);
                    else
                        Log.Get().LogMessage("Task '{0}' ran successfully", MessageType.Info, Task.Name);
                }
                catch (Exception e)
                {
                    Log.Get().LogMessage("{0}: Exception was thrown: {1}", MessageType.Error, Task.Name, e.ToString());
                }
            }
            Log.Get().LogMessage("Ending application", MessageType.Info);
        }
    }
}