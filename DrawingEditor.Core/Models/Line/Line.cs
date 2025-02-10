using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;


public class Line : IDrwaingGraphicObject
{
    public Point Start { get; }
    public Point End { get; }

    public Color LineColor { get; set; }

    public float LineThickness { get; set; }

    // Делегат для хранения алгоритма рисования
    private readonly Func<Point, Point, IEnumerable<Point>> _drawAlgorithm;

    public Line(Color color, Point start, Point end, Func<Point, Point, IEnumerable<Point>> drawAlgorithm) 
    {
        Start = start;
        End = end;
        LineColor = color;

        _drawAlgorithm = drawAlgorithm ?? throw new ArgumentNullException(nameof(drawAlgorithm));
    }

    public IEnumerable<Point> GetPoints() => _drawAlgorithm(Start, End);

    public List<Point> GetControlPoints() => new List<Point>{Start,End};

    public IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
    {
        return GetPoints().Select(point=> (point, 1.0));
    }
}