using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;


public class Line : GraphicObjectBase
{
    public Point Start { get; }
    public Point End { get; }

    private readonly Func<Point, Point, IEnumerable<Point>> _drawAlgorithm;
    private readonly ThickLineDrawer _thickLineDrawer;

    public Line(Color lineColor, float lineThickness, Point start, Point end,
                Func<Point, Point, IEnumerable<Point>> drawAlgorithm)
        : base(lineColor, lineThickness)
    {
        Start = start;
        End = end;
        _drawAlgorithm = drawAlgorithm ?? throw new ArgumentNullException(nameof(drawAlgorithm));
        _thickLineDrawer = new ThickLineDrawer(_drawAlgorithm);
    }

    public override IEnumerable<Point> GetPoints()
    {
        if (LineThickness > 1)
        {
            return _thickLineDrawer.DrawThickLine(Start, End, LineThickness).Select(p => p.point);
        }
        else
        {
            return _drawAlgorithm(Start, End);
        }
    }

    public override List<Point> GetControlPoints() => new List<Point> { Start, End };

    public override IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
    {
        if (LineThickness > 1)
        {
            return _thickLineDrawer.DrawThickLine(Start, End, LineThickness);
        }
        else
        {
            return _drawAlgorithm(Start, End).Select(p => (p, 1.0)); // Присваиваем каждой точке интенсивность 1.0
        }
    }
}

public class ThickLineDrawer
{
    private readonly Func<Point, Point, IEnumerable<Point>> _baseDrawAlgorithm;

    public ThickLineDrawer(Func<Point, Point, IEnumerable<Point>> baseDrawAlgorithm)
    {
        _baseDrawAlgorithm = baseDrawAlgorithm;
    }

    public IEnumerable<(Point point, double intensity)> DrawThickLine(Point start, Point end, float thickness)
    {
        // 1. Получаем базовые точки
        var basePoints = _baseDrawAlgorithm(start, end);

        // 2. Для каждой базовой точки...
        foreach (var point in basePoints)
        {
            // 3. Вычисляем половину толщины линии
            int halfThickness = (int)Math.Ceiling(thickness / 2);

            // 4. Перебираем пиксели в квадрате вокруг базовой точки
            for (int dx = -halfThickness; dx <= halfThickness; dx++)
            {
                for (int dy = -halfThickness; dy <= halfThickness; dy++)
                {
                    // 5. Вычисляем координаты нового пикселя
                    var newPoint = new Point(point.X + dx, point.Y + dy);

                    // 6. Вычисляем расстояние от нового пикселя до базовой точки
                    double distance = Math.Sqrt(dx * dx + dy * dy);

                    // 7. Вычисляем интенсивность нового пикселя на основе расстояния
                    double newIntensity = Math.Max(0, 1 - distance / halfThickness);

                    // 8. Возвращаем новый пиксель с его интенсивностью
                    yield return (newPoint, newIntensity);
                }
            }
        }
    }

}
/*
public class Line : GraphicObjectBase
{
    public Point Start { get; }
    public Point End { get; }

    // Делегат для алгоритма рисования линии (возвращает точки)
    private readonly Func<Point, Point, IEnumerable<Point>> _drawAlgorithm;

    public Line(Color lineColor, float lineThickness, Point start, Point end,
                Func<Point, Point, IEnumerable<Point>> drawAlgorithm)
        : base(lineColor, lineThickness)
    {
        Start = start;
        End = end;
        _drawAlgorithm = drawAlgorithm ?? throw new ArgumentNullException(nameof(drawAlgorithm));
    }

    public override IEnumerable<Point> GetPoints() => _drawAlgorithm(Start, End);

    public override List<Point> GetControlPoints() => new List<Point> { Start, End };

    public override IEnumerable<(Point point, double intensity)> GetPointsWithIntensity() =>
        _drawAlgorithm(Start, End).Select(p => (p, 1.0));
}
*/