using System;
using System.Drawing;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;
using Point = Xamarin.Forms.Point;
using Rectangle = Xamarin.Forms.Rectangle;
using Size = Xamarin.Forms.Size;

namespace Listen.iOS.Renderers
{
    public static class ColorExtensions
    {
        internal static readonly UIColor Black = UIColor.Black;
        internal static readonly UIColor SeventyPercentGrey = new UIColor(0.7f, 0.7f, 0.7f, 1);

        public static CGColor ToCGColor(this Color color)
        {
            return color.ToUIColor().CGColor;
        }

        public static Color ToColor(this UIColor color)
        {
            nfloat red;
            nfloat green;
            nfloat blue;
            nfloat alpha;
            color.GetRGBA(out red, out green, out blue, out alpha);
            return new Color(red, green, blue, alpha);
        }

        public static UIColor ToUIColor(this Color color)
        {
            return new UIColor((float)color.R, (float)color.G, (float)color.B, (float)color.A);
        }

        public static UIColor ToUIColor(this Color color, Color defaultColor)
        {
            if (color.IsDefault)
                return defaultColor.ToUIColor();

            return color.ToUIColor();
        }

        public static UIColor ToUIColor(this Color color, UIColor defaultColor)
        {
            if (color.IsDefault)
                return defaultColor;

            return color.ToUIColor();
        }
    }

    public static class PointExtensions
    {
        public static Point ToPoint(this PointF point)
        {
            return new Point(point.X, point.Y);
        }

        public static PointF ToPointF(this Point point)
        {
            return new PointF((float)point.X, (float)point.Y);
        }
    }

    public static class SizeExtensions
    {
        public static SizeF ToSizeF(this Size size)
        {
            return new SizeF((float)size.Width, (float)size.Height);
        }
    }

    public static class RectangleExtensions
    {
        public static Rectangle ToRectangle(this RectangleF rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static RectangleF ToRectangleF(this Rectangle rect)
        {
            return new RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
        }
    }
}