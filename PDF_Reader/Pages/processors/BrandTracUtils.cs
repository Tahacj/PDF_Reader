
using Syncfusion.Drawing;

namespace PDF_Reader.Pages
{
    public class BrandTracUtils
    {
        internal static bool IsIntersected(RectangleF rect1, RectangleF rect2)
        {
            if (rect2.X < rect1.X + rect1.Width && rect1.X < rect2.X + rect2.Width && rect2.Y < rect1.Y + rect1.Height)
                return rect1.Y < rect2.Y + rect2.Height;
            return false;
        }
    }
}
