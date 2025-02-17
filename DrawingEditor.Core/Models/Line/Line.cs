using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;


//public class Line : GraphicObjectBase
//{
//    public Point Start { get; }
//    public Point End { get; }

//    private readonly Func<Point, Point, IEnumerable<(Point point, double intensity)>> _drawAlgorithm;
//    private readonly ThickLineDrawer _thickLineDrawer;

//    public Line(Color lineColor, float lineThickness, Point start, Point end,
//                Func<Point, Point, IEnumerable<(Point point, double intensity)>> drawAlgorithm)
//        : base(lineColor, lineThickness)
//    {
//        Start = start;
//        End = end;
//        _drawAlgorithm = drawAlgorithm ?? throw new ArgumentNullException(nameof(drawAlgorithm));
//        _thickLineDrawer = new ThickLineDrawer(_drawAlgorithm);
//    }

//    public override IEnumerable<Point> GetPoints()
//    {
//        if (LineThickness > 1)
//        {
//            return _thickLineDrawer.DrawThickLine(Start, End, LineThickness).Select(p => p.point);
//        }
//        else
//        {
//            return _drawAlgorithm(Start, End).Select(p => p.point);
//        }
//    }

//    public override List<Point> GetControlPoints() => new List<Point> { Start, End };

//    public override IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
//    {
//        if (LineThickness > 1)
//        {
//            return _thickLineDrawer.DrawThickLine(Start, End, LineThickness);
//        }
//        else
//        {
//            return _drawAlgorithm(Start, End);
//        }
//    }
//}

//public class ThickLineDrawer
//{
//    private readonly Func<Point, Point, IEnumerable<(Point point, double intensity)>> _baseDrawAlgorithm;

//    public ThickLineDrawer(Func<Point, Point, IEnumerable<(Point point, double intensity)>> baseDrawAlgorithm)
//    {
//        _baseDrawAlgorithm = baseDrawAlgorithm;
//    }

//    public IEnumerable<(Point point, double intensity)> DrawThickLine(Point start, Point end, float thickness)
//    {
//        var basePoints = _baseDrawAlgorithm(start, end);

//        // Реализация рисования толстой линии
//        foreach (var (point, intensity) in basePoints)
//        {
//            int halfThickness = (int)Math.Ceiling(thickness / 2);

//            // Рисуем "квадрат" вокруг каждой точки для создания эффекта толщины
//            for (int dx = -halfThickness; dx <= halfThickness; dx++)
//            {
//                for (int dy = -halfThickness; dy <= halfThickness; dy++)
//                {
//                    var newPoint = new Point(point.X + dx, point.Y + dy);
//                    // Интенсивность может быть уменьшена в зависимости от расстояния от центра
//                    double distance = Math.Sqrt(dx * dx + dy * dy);
//                    double newIntensity = intensity * Math.Max(0, 1 - distance / halfThickness);
//                    yield return (newPoint, newIntensity);
//                }
//            }
//        }
//    }
//}

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
