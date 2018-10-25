using System;
using System.ComponentModel;
using RectangleF = CoreGraphics.CGRect;
using SizeF = CoreGraphics.CGSize;
using Foundation;

using UIKit;
using NativeLabel = UIKit.UILabel;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

//[assembly: ExportRenderer(typeof(Label), typeof(Listen.iOS.Renderers.LabelRenderer))]
namespace Listen.iOS.Renderers
{
    public class LabelRenderer : ViewRenderer<Label, NativeLabel>
    {
        SizeRequest _perfectSize;

        bool _perfectSizeValid;

        FormattedString _formatted;

        bool IsTextFormatted => _formatted != null;

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (!_perfectSizeValid)
            {
                _perfectSize = base.GetDesiredSize(double.PositiveInfinity, double.PositiveInfinity);
                _perfectSize.Minimum = new Size(Math.Min(10, _perfectSize.Request.Width), _perfectSize.Request.Height);
                _perfectSizeValid = true;
            }

            var widthFits = widthConstraint >= _perfectSize.Request.Width;
            var heightFits = heightConstraint >= _perfectSize.Request.Height;

            if (widthFits && heightFits)
                return _perfectSize;

            var result = base.GetDesiredSize(widthConstraint, heightConstraint);
            var tinyWidth = Math.Min(10, result.Request.Width);
            result.Minimum = new Size(tinyWidth, result.Request.Height);

            if (widthFits || Element.LineBreakMode == LineBreakMode.NoWrap)
                return result;

            bool containerIsNotInfinitelyWide = !double.IsInfinity(widthConstraint);

            if (containerIsNotInfinitelyWide)
            {
                bool textCouldHaveWrapped = Element.LineBreakMode == LineBreakMode.WordWrap || Element.LineBreakMode == LineBreakMode.CharacterWrap;
                bool textExceedsContainer = result.Request.Width > widthConstraint;

                if (textExceedsContainer || textCouldHaveWrapped)
                {
                    var expandedWidth = Math.Max(tinyWidth, widthConstraint);
                    result.Request = new Size(expandedWidth, result.Request.Height);
                }
            }

            return result;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (Control == null)
                return;

            SizeF fitSize;
            nfloat labelHeight;
            switch (Element.VerticalTextAlignment)
            {
                case TextAlignment.Start:
                    fitSize = Control.SizeThatFits(Element.Bounds.Size.ToSizeF());
                    labelHeight = (nfloat)Math.Min(Bounds.Height, fitSize.Height);
                    Control.Frame = new RectangleF(0, 0, (nfloat)Element.Width, labelHeight);
                    break;
                case TextAlignment.Center:
                    Control.Frame = new RectangleF(0, 0, (nfloat)Element.Width, (nfloat)Element.Height);
                    break;
                case TextAlignment.End:
                    fitSize = Control.SizeThatFits(Element.Bounds.Size.ToSizeF());
                    labelHeight = (nfloat)Math.Min(Bounds.Height, fitSize.Height);
                    nfloat yOffset = 0;
                    yOffset = (nfloat)(Element.Height - labelHeight);
                    Control.Frame = new RectangleF(0, yOffset, (nfloat)Element.Width, labelHeight);
                    break;
            }

            Control.RecalculateSpanPositions(Element);

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    SetNativeControl(new NativeLabel(RectangleF.Empty));
                }

                UpdateLineBreakMode();
                UpdateAlignment();
                UpdateText();
                UpdateTextDecorations();
                UpdateTextColor();
                UpdateFont();
                UpdateMaxLines();
            }

            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.HorizontalTextAlignmentProperty.PropertyName)
                UpdateAlignment();
            else if (e.PropertyName == Label.VerticalTextAlignmentProperty.PropertyName)
                UpdateLayout();
            else if (e.PropertyName == Label.TextColorProperty.PropertyName)
                UpdateTextColor();
            else if (e.PropertyName == Label.FontProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Label.TextProperty.PropertyName)
                UpdateText();
            else if (e.PropertyName == Label.TextDecorationsProperty.PropertyName)
                UpdateTextDecorations();
            else if (e.PropertyName == Label.FormattedTextProperty.PropertyName)
                UpdateText();
            else if (e.PropertyName == Label.LineBreakModeProperty.PropertyName)
                UpdateLineBreakMode();
            else if (e.PropertyName == VisualElement.FlowDirectionProperty.PropertyName)
                UpdateAlignment();
            else if (e.PropertyName == Label.LineHeightProperty.PropertyName)
                UpdateText();
            else if (e.PropertyName == Label.MaxLinesProperty.PropertyName)
                UpdateMaxLines();
        }

        void UpdateTextDecorations()
        {
            if (!Element.IsSet(Label.TextDecorationsProperty))
                return;

            var textDecorations = Element.TextDecorations;

            var newAttributedText = new NSMutableAttributedString(Control.AttributedText);
            var strikeThroughStyleKey = UIStringAttributeKey.StrikethroughStyle;
            var underlineStyleKey = UIStringAttributeKey.UnderlineStyle;

            var range = new NSRange(0, newAttributedText.Length);

            if ((textDecorations & TextDecorations.Strikethrough) == 0)
                newAttributedText.RemoveAttribute(strikeThroughStyleKey, range);
            else
                newAttributedText.AddAttribute(strikeThroughStyleKey, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);

            if ((textDecorations & TextDecorations.Underline) == 0)
                newAttributedText.RemoveAttribute(underlineStyleKey, range);
            else
                newAttributedText.AddAttribute(underlineStyleKey, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);
                
            Control.AttributedText = newAttributedText;

        }

        protected override void SetAccessibilityLabel()
        {
            // If we have not specified an AccessibilityLabel and the AccessibiltyLabel is current bound to the Text,
            // exit this method so we don't set the AccessibilityLabel value and break the binding.
            // This may pose a problem for users who want to explicitly set the AccessibilityLabel to null, but this
            // will prevent us from inadvertently breaking UI Tests that are using Query.Marked to get the dynamic Text 
            // of the Label.

            var elemValue = (string)Element?.GetValue(AutomationProperties.NameProperty);
            if (string.IsNullOrWhiteSpace(elemValue) && Control?.AccessibilityLabel == Control?.Text)
                return;

            base.SetAccessibilityLabel();
        }

        protected override void SetBackgroundColor(Color color)
        {
            if (color == Color.Default)
                BackgroundColor = UIColor.Clear;
            else
                BackgroundColor = color.ToUIColor();
        }

        void UpdateAlignment()
        {
            Control.TextAlignment = Element.HorizontalTextAlignment.ToNativeTextAlignment(((IVisualElementController)Element).EffectiveFlowDirection);
        }

        void UpdateLineBreakMode()
        {
            _perfectSizeValid = false;
            switch (Element.LineBreakMode)
            {
                case LineBreakMode.NoWrap:
                    Control.LineBreakMode = UILineBreakMode.Clip;
                    break;
                case LineBreakMode.WordWrap:
                    Control.LineBreakMode = UILineBreakMode.WordWrap;
                    break;
                case LineBreakMode.CharacterWrap:
                    Control.LineBreakMode = UILineBreakMode.CharacterWrap;
                    break;
                case LineBreakMode.HeadTruncation:
                    Control.LineBreakMode = UILineBreakMode.HeadTruncation;
                    break;
                case LineBreakMode.MiddleTruncation:
                    Control.LineBreakMode = UILineBreakMode.MiddleTruncation;
                    break;
                case LineBreakMode.TailTruncation:
                    Control.LineBreakMode = UILineBreakMode.TailTruncation;
                    break;
            }
        }

        void UpdateText()
        {
            _perfectSizeValid = false;
            var values = Element.GetValues(Label.FormattedTextProperty, Label.TextProperty);

            _formatted = values[0] as FormattedString;
            if (_formatted == null && Element.LineHeight >= 0)
                _formatted = (string)values[1];

            if (IsTextFormatted)
            {
                UpdateFormattedText();
            }
            else
            {
                Control.Text = (string)values[1];
            }
            UpdateLayout();
        }

        void UpdateFormattedText()
        {
            Control.AttributedText = _formatted.ToAttributed(Element, Element.TextColor, Element.LineHeight);
        }

        void UpdateFont()
        {
            if (IsTextFormatted)
            {
                UpdateFormattedText();
                return;
            }
            _perfectSizeValid = false;

            Control.Font = Element.ToUIFont();
            UpdateLayout();
        }

        void UpdateTextColor()
        {
            if (IsTextFormatted)
            {
                UpdateFormattedText();
                return;
            }

            _perfectSizeValid = false;

            var textColor = (Color)Element.GetValue(Label.TextColorProperty);

            // default value of color documented to be black in iOS docs
            Control.TextColor = textColor.ToUIColor(ColorExtensions.Black);
            UpdateLayout();
        }
        void UpdateLayout()
        {
            LayoutSubviews();
        }

        void UpdateMaxLines()
        {
            if (Element.MaxLines >= 0)
            {
                Control.Lines = Element.MaxLines;

                LayoutSubviews();
            }
            else
            {
                switch (Element.LineBreakMode)
                {
                    case LineBreakMode.WordWrap:
                    case LineBreakMode.CharacterWrap:
                        Control.Lines = 0;
                        break;
                    case LineBreakMode.NoWrap:
                    case LineBreakMode.HeadTruncation:
                    case LineBreakMode.MiddleTruncation:
                    case LineBreakMode.TailTruncation:
                        Control.Lines = 1;
                        break;
                }

                LayoutSubviews();
            }
        }
    }
}
