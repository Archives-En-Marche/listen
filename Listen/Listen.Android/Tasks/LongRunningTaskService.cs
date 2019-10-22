using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Listen.Models.Tasks;
using Xamarin.Forms;

namespace Listen.Droid.Tasks
{
    [Service(Name = "fr.edecision.listen.LongRunningTaskService", Permission= "android.permission.BIND_JOB_SERVICE", Exported =true)]
    public class LongRunningTaskService : JobIntentService
    {
        CancellationTokenSource _cts;

        ILongRunningTask longTask;

        public LongRunningTaskService()
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
            Console.WriteLine("Job Execution Started");
        }

        public static int JOB_ID = 1;

        public static void EnqueueWork(Context context, Intent work)
        {   
            Java.Lang.Class cls = Java.Lang.Class.FromType(typeof(LongRunningTaskService));
            try
            {
                EnqueueWork(context, cls, JOB_ID, work);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
        }


        //public override IBinder OnBind(Intent intent)
        //{
        //    return null;
        //}

        //public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        //{

        //    var parcelable = (UploadLongRunningTaskParcelable)intent.GetParcelableExtra("task");

        //    longTask = parcelable.Task;

        //    _cts = new CancellationTokenSource();

        //    Task.Run(async () =>
        //    {
        //        try
        //        {
        //            //INVOKE THE SHARED CODE
        //            //var counter = new TaskCounter();
        //            //counter.RunCounter(_cts.Token).Wait();
        //            if (longTask != null)
        //            {
        //                await longTask.Run(_cts.Token);
        //            }
        //        }
        //        catch (System.OperationCanceledException ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //        finally
        //        {
        //            if (_cts.IsCancellationRequested)
        //            {
        //                //var message = new CancelledMessage();
        //                //Device.BeginInvokeOnMainThread(
        //                //    () => MessagingCenter.Send(message, "CancelledMessage")
        //                //);
        //            }
        //        }

        //    }, _cts.Token);

        //    return StartCommandResult.Sticky;
        //}

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();

                _cts.Cancel();
            }
            base.OnDestroy();
            Console.WriteLine("Job Execution Finished");
        }

        protected override void OnHandleWork(Intent p0)
        {
            var parcelable = (UploadLongRunningTaskParcelable)p0.GetParcelableExtra("task");

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
        }
    }

}
