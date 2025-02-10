using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

public class WuLine :  IDrwaingGraphicObject
{
    public Point Start { get; }
    public Point End { get; }

    public Color LineColor { get; set; }

    public float LineThickness { get; set; }

    // Делегат для хранения алгоритма рисования
    private readonly Func<Point, Point, IEnumerable<(Point point, double intensity)>> _drawAlgorithm;

    public WuLine(Color color, Point start, Point end, Func<Point, Point, IEnumerable<(Point point, double intensity)>> drawAlgorithm)
    {
        Start = start;
        End = end;
        LineColor = color;

        _drawAlgorithm = drawAlgorithm ?? throw new ArgumentNullException(nameof(drawAlgorithm));
    }

    public IEnumerable<Point> GetPoints() => _drawAlgorithm(Start, End).Select(p=>p.point);

    public List<Point> GetControlPoints() => new List<Point> { Start, End };

    public IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
    {
        return _drawAlgorithm(Start, End);
    }
}
