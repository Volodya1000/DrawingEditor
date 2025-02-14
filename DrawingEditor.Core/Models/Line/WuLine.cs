using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

public class WuLine : GraphicObjectBase
{
    public Point Start { get; }
    public Point End { get; }

    // Делегат для хранения алгоритма рисования Ву (возвращает точки с интенсивностью)
    private readonly Func<Point, Point, IEnumerable<(Point point, double intensity)>> _drawAlgorithm;

    public WuLine(Color lineColor, float lineThickness, 
                  Point start, Point end, 
                  Func<Point, Point, IEnumerable<(Point point, double intensity)>> drawAlgorithm)
        : base(lineColor, lineThickness)
    {
        Start = start;
        End = end;
        _drawAlgorithm = drawAlgorithm ?? throw new ArgumentNullException(nameof(drawAlgorithm));
    }

    public override IEnumerable<Point> GetPoints() =>
        _drawAlgorithm(Start, End).Select(p => p.point);

    public override List<Point> GetControlPoints() =>
        new List<Point> { Start, End };

    public override IEnumerable<(Point point, double intensity)> GetPointsWithIntensity() =>
        _drawAlgorithm(Start, End);
}

