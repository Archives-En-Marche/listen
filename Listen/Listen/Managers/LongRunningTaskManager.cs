using System;
using Listen.Models.Tasks;
using Xamarin.Forms;

namespace Listen.Managers
{
    public class LongRunningTaskManager
    {
        private static readonly Lazy<LongRunningTaskManager> lazy = new Lazy<LongRunningTaskManager>(() => new LongRunningTaskManager());

        public LongRunningTaskManager()
        {
        }

        public static LongRunningTaskManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public void StartLongRunningTask()
        {
            var message = new StartLongRunningTaskMessage();
            MessagingCenter.Send(message, "StartLongRunningTaskMessage");
        }

        public void StopLongRunningTask()
        {
            var message = new StopLongRunningTaskMessage();
            MessagingCenter.Send(message, "StopLongRunningTaskMessage");
        }
    }
}
