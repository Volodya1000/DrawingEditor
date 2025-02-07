using System.Drawing;

namespace DrawingEditor.Core.Algorithms.LineAlgorithms;

public static class LineDrawingAlgorithms
{
    public static IEnumerable<Point> CDADraw(Point start, Point end)
    {
        // Простой пример ЦДА
        int dx = end.X - start.X;
        int dy = end.Y - start.Y;
        int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

        float xIncrement = dx / (float)steps;
        float yIncrement = dy / (float)steps;

        float x = start.X;
        float y = start.Y;

        for (int i = 0; i <= steps; i++)
        {
            yield return new Point((int)Math.Round(x), (int)Math.Round(y));
            x += xIncrement;
            y += yIncrement;
        }
    }

    public static IEnumerable<Point> BresenhamDraw(Point start, Point end)
    {
        // Алгоритм Брезенхема
        int x = start.X;
        int y = start.Y;
        int dx = Math.Abs(end.X - start.X);
        int dy = Math.Abs(end.Y - start.Y);
        int sx = start.X < end.X ? 1 : -1;
        int sy = start.Y < end.Y ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            yield return new Point(x, y);
            if (x == end.X && y == end.Y) break;
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x += sx; }
            if (e2 < dx) { err += dx; y += sy; }
        }
    }
}
