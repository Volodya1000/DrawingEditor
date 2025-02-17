using System.Drawing;

namespace DrawingEditor.Core.Algorithms.LineAlgorithms;

public static class LineDrawingAlgorithms
{
    //Цифровой дифференциальный анализатор
    public static IEnumerable<Point> CDADraw(Point start, Point end)
    {
        int dx = end.X - start.X;
        int dy = end.Y - start.Y;
        int length = Math.Max(Math.Abs(dx), Math.Abs(dy));

        float xIncrement = dx / (float)length;
        float yIncrement = dy / (float)length;

        float x = start.X;
        float y = start.Y;

        for (int i = 0; i <= length; i++)
        {
            yield return new Point((int)Math.Round(x), (int)Math.Round(y));
            x += xIncrement;
            y += yIncrement;
        }
    }

    // Алгоритм Брезенхема
    public static IEnumerable<Point> BresenhamDraw(Point start, Point end)
    {
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


    //Алгоритм Ву
    #region Wu

    public static IEnumerable<(Point point, double intensity)> WuDraw(Point start, Point end)
    {
        int x0 = start.X;
        int y0 = start.Y;
        int x1 = end.X;
        int y1 = end.Y;

        bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep)
        {
            Swap(ref x0, ref y0);
            Swap(ref x1, ref y1);
        }
        if (x0 > x1)
        {
            Swap(ref x0, ref x1);
            Swap(ref y0, ref y1);
        }

        int dx = x1 - x0;
        int dy = y1 - y0;
        double gradient = (double)dy / dx;

        double y = y0;
        for (int x = x0; x <= x1; x++)
        {
            // Возвращаем точку и интенсивность в зависимости от крутизны
            if (steep)
            {
                yield return (new Point((int)y, x), 1 - FractionalPart(y));
                yield return (new Point((int)y + 1, x), FractionalPart(y));
            }
            else
            {
                yield return (new Point(x, (int)y), 1 - FractionalPart(y));
                yield return (new Point(x, (int)y + 1), FractionalPart(y));
            }
            y += gradient;
        }
    }

    // Вспомогательная функция для обмена значениями
    private static void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    // Вспомогательная функция для вычисления дробной части числа
    private static double FractionalPart(double x)
    {
        return x - (int)x;
    }

    #endregion
}
