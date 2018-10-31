using System;
using System.Threading;
using System.Threading.Tasks;
using Listen.Models.Tasks;
using UIKit;
using Xamarin.Forms;

namespace Listen.iOS.Tasks
{
    public class LongRunningTask
    {
        nint _taskId;
        CancellationTokenSource _cts;

        ILongRunningTask longTask;

        public LongRunningTask(ILongRunningTask task)
        {
            longTask = task;
        }

        public async Task Start()
        {
            _cts = new CancellationTokenSource();

            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("LongRunningTask", OnExpiration);

            try
            {
                //INVOKE THE SHARED CODE
                //var counter = new TaskCounter();
                //await counter.RunCounter(_cts.Token);

                if (longTask != null)
                {
                    await longTask.Run(_cts.Token);
                }

                //await Task.Run(() => {

                //    _cts.Token.ThrowIfCancellationRequested();

                //    LongTask.Start();

                //        //Device.BeginInvokeOnMainThread(() => {
                //        //    MessagingCenter.Send<TickedMessage>(message, "TickedMessage");
                //        //});

                //}, _cts.Token);

            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (_cts.IsCancellationRequested)
                {
                    //var message = new CancelledMessage();
                    //Device.BeginInvokeOnMainThread(
                    //    () => MessagingCenter.Send(message, "CancelledMessage")
                    //);
                }
            }

            UIApplication.SharedApplication.EndBackgroundTask(_taskId);
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        void OnExpiration()
        {
            _cts.Cancel();
        }
    }
}
