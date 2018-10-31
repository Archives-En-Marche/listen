using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Listen.ViewModels;
using PopolLib.Extensions;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class EndPage : ContentPage
    {
        public EndPage(ViewModelBase vm)
        {
            BindingContext = vm;
            InitializeComponent();

            //var allCmdAction = new Action(async () =>
            //{
            //    var currentBgColor = allCommand.BackgroundColor;
            //    var currentTextColor = allCommandLabel.TextColor;

            //    allCommand.BackgroundColor = Color.FromHex("#6c6d7a");
            //    allCommandLabel.TextColor = Color.White;
            //    await Task.Delay(100);
            //    allCommand.BackgroundColor = currentBgColor;
            //    allCommandLabel.TextColor = currentTextColor;
            //});

            //var newCmdAction = new Action(async () =>
            //{
            //    var currentBgColor = newCommand.BackgroundColor;
            //    var currentTextColor = newCommandLabel.TextColor;

            //    newCommand.BackgroundColor = Color.FromHex("#6c6d7a");
            //    newCommandLabel.TextColor = Color.White;
            //    await Task.Delay(100);
            //    newCommand.BackgroundColor = currentBgColor;
            //    newCommandLabel.TextColor = currentTextColor;
            //});

            //((EndPageViewModel)vm).AllCommandAnimation = allCmdAction;
            //((EndPageViewModel)vm).NewCommandAnimation = newCmdAction;
        }
    }
}
