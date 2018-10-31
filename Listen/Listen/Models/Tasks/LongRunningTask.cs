using System;
namespace Listen.Models.Tasks
{
    public class StartLongRunningTaskMessage { }

    public class StopLongRunningTaskMessage { }

    public class TickedMessage
    {
        public string Message { get; set; }
    }

}
