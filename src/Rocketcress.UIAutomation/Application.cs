using System;
using System.Diagnostics;
using System.Linq;

namespace Rocketcress.UIAutomation
{
    // TODO: Add XML Documentation
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Application
    {
        public Process Process { get; set; }

        private Application(Process process, UIAutomationTestContext context)
        {
            Process = process;
            context.Applications.Add(this);
            context.ActiveApplication = this;
        }

        public static Application Launch(string filePath) => Launch(filePath, UIAutomationTestContext.CurrentContext);
        public static Application Launch(string filePath, UIAutomationTestContext context)
        {
            var process = Process.Start(filePath);
            process.WaitForInputIdle();
            return new Application(process, context);
        }

        public static Application Attach(string filePath) => Attach(filePath, UIAutomationTestContext.CurrentContext);
        public static Application Attach(string filePath, UIAutomationTestContext context)
        {
            var process = Process.GetProcesses().FirstOrDefault(x => string.Equals(x.StartInfo.FileName, filePath, StringComparison.OrdinalIgnoreCase));
            if (process == null)
                throw new InvalidOperationException($"No running process with fileName \"{filePath}\" was found.");
            process.WaitForInputIdle();
            return new Application(process, context);
        }

        public static Application Attach(int processId) => Attach(processId, UIAutomationTestContext.CurrentContext);
        public static Application Attach(int processId, UIAutomationTestContext context)
        {
            var process = Process.GetProcessById(processId);
            if (process == null)
                throw new InvalidOperationException($"No running process with id {processId} was found.");
            process.WaitForInputIdle();
            return new Application(process, context);
        }

        public static Application Attach(Process process) => Attach(process, UIAutomationTestContext.CurrentContext);
        public static Application Attach(Process process, UIAutomationTestContext context)
        {
            if (process == null)
                throw new ArgumentNullException("process");
            if (process.HasExited)
                throw new InvalidOperationException("The process is terminated.");
            process.WaitForInputIdle();
            return new Application(process, context);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
