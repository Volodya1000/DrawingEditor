using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;


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
