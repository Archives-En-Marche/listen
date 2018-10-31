using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Listen.Helpers
{
    public static class ButtonAnimationHelper
    {
        public static async void Animate(Frame frame)
        {
            var label = frame.Content as Label;
            if (label != null)
            {
                var currentBgColor = frame.BackgroundColor;
                var currentTextColor = label.TextColor;

                frame.BackgroundColor = Color.FromHex("#6c6d7a");
                label.TextColor = Color.White;
                await Task.Delay(100);
                frame.BackgroundColor = currentBgColor;
                label.TextColor = currentTextColor;
            }
        }
    }
}
