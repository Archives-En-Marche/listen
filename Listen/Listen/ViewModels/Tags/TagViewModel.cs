using System;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.ViewModels.Tags
{
    public class TagViewModel : ViewModelBase
    {
        public object Parameters { get; set; }

        public string Id { get; set; }
        public string Text { get; set; }
        public int Height { get; set; } = 50;

        Color _backgroundColor;
        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                Set(() => BackgroundColor, ref _backgroundColor, value);
            }
        }

        Color _textColor;
        public Color TextColor
        {
            get
            {
                return _textColor;
            }
            set
            {
                Set(() => TextColor, ref _textColor, value);
            }
        }
    }
}
