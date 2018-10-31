using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Listen.Models.Tasks;
using Xamarin.Forms;

namespace Listen.Droid.Tasks
{
    [Service]
    public class LongRunningTaskService : Service
    {
        CancellationTokenSource _cts;

        ILongRunningTask longTask;

        public LongRunningTaskService()
        {

        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            var parcelable = (UploadLongRunningTaskParcelable)intent.GetParcelableExtra("task");

            longTask = parcelable.Task;

            _cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                try
                {
                    //INVOKE THE SHARED CODE
                    //var counter = new TaskCounter();
                    //counter.RunCounter(_cts.Token).Wait();
                    if (longTask != null)
                    {
                        await longTask.Run(_cts.Token);
                    }
                }
                catch (System.OperationCanceledException ex)
                {
                    Console.WriteLine(ex.Message);
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

            }, _cts.Token);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();

                _cts.Cancel();
            }
            base.OnDestroy();
        }
    }

}
