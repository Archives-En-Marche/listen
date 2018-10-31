using System;
using System.Threading;
using System.Threading.Tasks;
using Listen.Managers;
using Xamarin.Forms;

namespace Listen.Models.Tasks
{
    public class UploadLongRunningTask : ILongRunningTask
    {
        public async Task Run(CancellationToken token = default(CancellationToken))
        {
            await Task.Run(async () => {

                await ServerManager.Instance.UploadRepliesAsync();

                //for (long i = 0; i < long.MaxValue; i++)
                //{
                //    token.ThrowIfCancellationRequested();

                //    await Task.Delay(250);
                //    var message = new TickedMessage
                //    {
                //        Message = i.ToString()
                //    };

                //    Device.BeginInvokeOnMainThread(() => {
                //        MessagingCenter.Send<TickedMessage>(message, "TickedMessage");
                //    });
                //}
            }, token);
        }
    }
}
