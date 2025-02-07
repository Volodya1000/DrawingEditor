using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

//public class Line : IDrwaingGraphicObject
//{
//    private readonly List<Point> _points;

//    public Line(IEnumerable<Point> points)
//    {
//        _points = new List<Point>(points);
//    }

//    public IEnumerable<Point> GetPoints() => _points;
//}

public class Line:IDrwaingGraphicObject
{
    public Point Start { get; }
    public Point End   { get; }

    // Делегат для хранения алгоритма рисования
    private readonly Func<Point, Point, IEnumerable<Point>> _drawAlgorithm;

    public Line(Point start, Point end, Func<Point, Point, IEnumerable<Point>> drawAlgorithm)
    {
        Start = start;
        End = end;
        _drawAlgorithm = drawAlgorithm ?? throw new ArgumentNullException(nameof(drawAlgorithm));
    }

    public IEnumerable<Point> GetPoints() => _drawAlgorithm(Start, End);
}