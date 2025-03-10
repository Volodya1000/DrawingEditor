using DrawingEditor.Core.Models.Interfaces;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System.Drawing;

namespace DrawingEditor.Core;

public class Circle: GraphicObjectBase, IEditableGraphicObject
{
    public Point Center { get; set; }

    public int  Radius { get; set; }

    public Circle(Color lineColor, float lineThickness,Point center, int radius) 
        : base(lineColor, lineThickness)
    {
        Center=center;
        Radius=radius;
    }

    public static IDrawingGraphicObject? Create(Color lineColor, int lineThickness, IList<Point> points) 
    {
        if (points.Count < GetRequiredPointsCount())
            throw new ArgumentException();
        Point center = points[0];
        var boundaryPoint = points[1];
        double radiusSquare = Math.Pow(center.X - boundaryPoint.X, 2) +
                             Math.Pow(center.Y - boundaryPoint.Y, 2);
        int radius = (int)Math.Sqrt(radiusSquare);
        return new Circle(lineColor, lineThickness, center, radius);
    }


    public static int GetRequiredPointsCount() => 2;

    public override IEnumerable<Point> GetPoints()
    {
        return BresenhamCircle(Center.X, Center.Y, Radius);
       
    }
    

    public override IEnumerable<Point> GetControlPoints()
    {
        // Самая правая точка
        yield return new Point(Center.X + Radius, Center.Y);

        // Самая левая точка
        yield return new Point(Center.X - Radius, Center.Y);

        // Верхняя точка
        yield return new Point(Center.X, Center.Y - Radius);

        // Нижняя точка
        yield return new Point(Center.X, Center.Y + Radius);
    }

    public override IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
    {
        return GetPoints().Select(p => (p, 1.0));
    }

    private IEnumerable<Point> BresenhamCircle(int _x, int _y, int radius)
    {
        int x = 0, y = radius, gap = 0, delta = (2 - 2 * radius);
        while (y >= 0)
        {
            yield return new Point(_x + x, _y + y);
            yield return new Point(_x + x, _y - y);
            yield return new Point(_x - x, _y - y);
            yield return new Point(_x - x, _y + y);
            gap = 2 * (delta + y) - 1;
            if (delta < 0 && gap <= 0)
            {
                x++;
                delta += 2 * x + 1;
                continue;
            }
            if (delta > 0 && gap > 0)
            {
                y--;
                delta -= 2 * y + 1;
                continue;
            }
            x++;
            delta += 2 * (x - y);
            y--;
        }
    }

    public void UpdateControlPoint(int index, Point newPoint)
    {
        // Вычисляем новый радиус как расстояние от центра до новой точки
        int newRadius = (int)Math.Sqrt(Math.Pow(newPoint.X - Center.X, 2) + Math.Pow(newPoint.Y - Center.Y, 2));

        if (newRadius > 0)
            Radius = newRadius;
    }

}

