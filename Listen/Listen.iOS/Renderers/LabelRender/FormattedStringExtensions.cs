using Foundation;
using System;
using Xamarin.Forms.Internals;
using UIKit;
using Xamarin.Forms;

namespace Listen.iOS.Renderers
{
    public static class FormattedStringExtensions
    {
        public static NSAttributedString ToAttributed(this Span span, Font defaultFont, Color defaultForegroundColor)
        {
            if (span == null)
                return null;

#pragma warning disable 0618 //retaining legacy call to obsolete code
            var font = span.Font != Font.Default ? span.Font : defaultFont;
#pragma warning restore 0618
            var fgcolor = span.TextColor;
            if (fgcolor.IsDefault)
                fgcolor = defaultForegroundColor;
            if (fgcolor.IsDefault)
                fgcolor = Color.Black; // as defined by apple docs      
                
            return new NSAttributedString(span.Text, font == Font.Default ? null : font.ToUIFont(), fgcolor.ToUIColor(), span.BackgroundColor.ToUIColor());
        }

        public static NSAttributedString ToAttributed(this FormattedString formattedString, Font defaultFont,
            Color defaultForegroundColor)
        {
            if (formattedString == null)
                return null;
            var attributed = new NSMutableAttributedString();
            for (int i = 0; i < formattedString.Spans.Count; i++)
            {
                Span span = formattedString.Spans[i];
                if (span.Text == null)
                    continue;

                attributed.Append(span.ToAttributed(defaultFont, defaultForegroundColor));
            }

            return attributed;
        }

        internal static NSAttributedString ToAttributed(this Span span, Element owner, Color defaultForegroundColor, double lineHeight = -1.0)
        {
            if (span == null)
                return null;

            var text = span.Text;
            if (text == null)
                return null;

            NSMutableParagraphStyle style = null;
            lineHeight = span.LineHeight >= 0 ? span.LineHeight : lineHeight;
            if (lineHeight >= 0)
            {
                style = new NSMutableParagraphStyle();
                style.LineHeightMultiple = new nfloat(lineHeight);
            }

            UIFont targetFont;
            if (span.IsDefault())
                targetFont = ((IFontElement)owner).ToUIFont();
            else
                targetFont = span.ToUIFont();

            var fgcolor = span.TextColor;
            if (fgcolor.IsDefault)
                fgcolor = defaultForegroundColor;
            if (fgcolor.IsDefault)
                fgcolor = Color.Black; // as defined by apple docs

            UIColor spanFgColor;
            UIColor spanBgColor;
            spanFgColor = fgcolor.ToUIColor();
            spanBgColor = span.BackgroundColor.ToUIColor();

            bool hasUnderline = false;
            bool hasStrikethrough = false;
            if (span.IsSet(Span.TextDecorationsProperty))
            {
                var textDecorations = span.TextDecorations;
                hasUnderline = (textDecorations & TextDecorations.Underline) != 0;
                hasStrikethrough = (textDecorations & TextDecorations.Strikethrough) != 0;
            }

            var attrString = new NSAttributedString(text, targetFont, spanFgColor, spanBgColor,
                underlineStyle: hasUnderline ? NSUnderlineStyle.Single : NSUnderlineStyle.None,
                strikethroughStyle: hasStrikethrough ? NSUnderlineStyle.Single : NSUnderlineStyle.None, paragraphStyle: style);

            return attrString;
        }

        internal static NSAttributedString ToAttributed(this FormattedString formattedString, Element owner,
            Color defaultForegroundColor, double lineHeight = -1.0)
        {
            if (formattedString == null)
                return null;
            var attributed = new NSMutableAttributedString();

            for (int i = 0; i < formattedString.Spans.Count; i++)
            {
                Span span = formattedString.Spans[i];
                var attributedString = span.ToAttributed(owner, defaultForegroundColor, lineHeight);
                if (attributedString == null)
                    continue;

                attributed.Append(attributedString);
            }

            return attributed;
        }
    }
}
