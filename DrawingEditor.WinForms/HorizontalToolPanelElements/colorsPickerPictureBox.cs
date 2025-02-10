using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;

namespace DrawingEditor.WinForms;
internal class colorsPickerPictureBox
{
    /*
    private void color_picker_MouseClick(object sender, HouseEventArgs e)
    {
        Point point = set_point(color_picker, e.Location);
        //pic_color.BackColor=((Bitmap) color_picker. Image).GetPixel(point.X, point.Y);
        new_color = pic_color.BackColor;
        p.Color = pic_color.BackColor;
    }


    private void validate(Bitmap bm, Stack<Point> sp, int x, int y, Color old_color, Color new_color)
    {
        Color cx = bm.GetPixel(x, y);
        if (cx == old_color)
        {
            sp.Push(new Point(x, y));
            bm.SetPixel(x, y, new_color);
        }
    }

    public void Fill(Bitmap bm, int x, int y, Color new_clr)
    {
        Color old_color = bm.GetPixel(x, y);
        Stack<Point> pixel = new Stack<Point>();
        pixel.Push(new Point(x, y));
        bm.SetPixel(x, y, new_clr);
        if (old_color == new_clr) return;
        while (pixel.Count > 0)
        {
            Point pt = (Point)pixel.Pop();
            validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
            validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr); 
            validate(bm, pixel, pt.X + 1, pt.Y, old_color, new_clr);
            validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);
        }
    }
    */
}
