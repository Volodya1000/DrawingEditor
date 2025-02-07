namespace DrawingEditor.WinForms;

public class GridDrawer
{
    public void DrawGrid(Graphics g)
    {
        for (int x = -1000; x < 1000; x += 10)
        {
            g.DrawLine(Pens.LightGray, x, -1000, x, 1000);
        }

        for (int y = -1000; y < 1000; y += 10)
        {
            g.DrawLine(Pens.LightGray, -1000, y, 1000, y);
        }
    }

    public void DrawLines(Graphics g, IEnumerable<(Point, Point)> lines)
    {
        foreach (var (start, end) in lines)
        {
            g.DrawLine(Pens.Black, start, end);
        }
    }

    public void DrawCurrentLine(Graphics g, Point? start, Point? end)
    {
        if (start.HasValue && end.HasValue)
        {
            g.DrawLine(Pens.Red, start.Value, end.Value);
        }
    }
}