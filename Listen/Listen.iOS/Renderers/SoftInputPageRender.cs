using System;
using CoreGraphics;
using Foundation;
using Listen.iOS.Renderers;
using Listen.VisualElements;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SoftInputPage), typeof(SoftInputPageRender))]
namespace Listen.iOS.Renderers
{
    public class SoftInputPageRender : PageRenderer
    {
        NSObject _keyboardShowObserver;
        NSObject _keyboardHideObserver;
        NSObject _keyboardDidObserver;
        private bool _pageWasShiftedUp;
        private double _activeViewBottom;
        private bool _isKeyboardShown;
        private ScrollView contentScrollView;

        public new static void Init()
        {
            var now = DateTime.Now;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var page = Element as ContentPage;

            if (page != null)
            {
                contentScrollView = page.Content as ScrollView;

                if (contentScrollView != null)
                {
                     //return;
                }

                RegisterForKeyboardNotifications();
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            UnregisterForKeyboardNotifications();
        }

        void RegisterForKeyboardNotifications()
        {
            if (_keyboardDidObserver == null)
                _keyboardDidObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, OnKeyboardDidShow);
            if (_keyboardShowObserver == null)
                _keyboardShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardShow);
            if (_keyboardHideObserver == null)
                _keyboardHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardHide);
        }

        void UnregisterForKeyboardNotifications()
        {
            _isKeyboardShown = false;
            if (_keyboardShowObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardShowObserver);
                _keyboardShowObserver.Dispose();
                _keyboardShowObserver = null;
            }

            if (_keyboardHideObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardHideObserver);
                _keyboardHideObserver.Dispose();
                _keyboardHideObserver = null;
            }
        }

        protected virtual void OnKeyboardDidShow(NSNotification notification)
        {
            if (contentScrollView != null)
            {
                var activeView = View.FindFirstResponder();
                if (activeView != null)
                {
                    contentScrollView.ScrollToAsync(0, 0, false);
                }
            }
        }

        protected virtual void OnKeyboardShow(NSNotification notification)
        {
            if (contentScrollView == null)
            {
                if (!IsViewLoaded || _isKeyboardShown)
                    return;

                _isKeyboardShown = true;
                var activeView = View.FindFirstResponder();

                if (activeView == null)
                    return;

                var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
                var isOverlapping = activeView.IsKeyboardOverlapping(View, keyboardFrame);

                if (!isOverlapping)
                    return;

                if (isOverlapping)
                {
                    _activeViewBottom = activeView.GetViewRelativeBottom(View);
                    ShiftPageUp(keyboardFrame.Height, _activeViewBottom);
                }
            }
            else
            {
                var activeView = View.FindFirstResponder();
                if (activeView != null)
                {
                     contentScrollView.ScrollToAsync(0, 0, false);
                }
            }
        }

        private void OnKeyboardHide(NSNotification notification)
        {
            if (contentScrollView == null)
            {
                if (!IsViewLoaded)
                    return;

                _isKeyboardShown = false;
                var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);

                if (_pageWasShiftedUp)
                {
                    ShiftPageDown(keyboardFrame.Height, _activeViewBottom);
                }
            }
        }

        private void ShiftPageUp(nfloat keyboardHeight, double activeViewBottom)
        {
            var pageFrame = Element.Bounds;

            var newHeight = pageFrame.Height + CalculateShiftByAmount(pageFrame.Height, keyboardHeight, activeViewBottom);

            Element.LayoutTo(new Rectangle(pageFrame.X, pageFrame.Y,
               pageFrame.Width, newHeight));

            _pageWasShiftedUp = true;
        }

        private void ShiftPageDown(nfloat keyboardHeight, double activeViewBottom)
        {
            var pageFrame = Element.Bounds;

            var newHeight = activeViewBottom;

            Element.LayoutTo(new Rectangle(pageFrame.X, pageFrame.Y,
             pageFrame.Width, newHeight));

            _pageWasShiftedUp = false;
        }

        private double CalculateShiftByAmount(double pageHeight, nfloat keyboardHeight, double activeViewBottom)
        {
            return (pageHeight - activeViewBottom) - keyboardHeight;
        }
    }

    public static class ViewExtensions
    {

        public static UIView FindFirstResponder(this UIView view)
        {
            if (view.IsFirstResponder)
            {
                return view;
            }
            foreach (UIView subView in view.Subviews)
            {
                var firstResponder = subView.FindFirstResponder();
                if (firstResponder != null)
                    return firstResponder;
            }
            return null;
        }

        public static double GetViewRelativeBottom(this UIView view, UIView rootView)
        {
            var viewRelativeCoordinates = rootView.ConvertPointFromView(view.Frame.Location, view);
            var activeViewRoundedY = Math.Round(viewRelativeCoordinates.Y, 2);

            return activeViewRoundedY + view.Frame.Height;
        }

        public static double GetViewRelativeTop(this UIView view, UIView rootView)
        {
            var viewRelativeCoordinates = rootView.ConvertPointFromView(view.Frame.Location, view);
            var activeViewRoundedY = Math.Round(viewRelativeCoordinates.Y, 2);

            return activeViewRoundedY;
        }

        public static bool IsKeyboardOverlapping(this UIView activeView, UIView rootView, CGRect keyboardFrame)
        {
            var activeViewBottom = activeView.GetViewRelativeBottom(rootView);
            var pageHeight = rootView.Frame.Height;
            var keyboardHeight = keyboardFrame.Height;

            var isOverlapping = activeViewBottom >= (pageHeight - keyboardHeight);

            return isOverlapping;
        }
    }
}
