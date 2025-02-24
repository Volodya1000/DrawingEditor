using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core.GraphickObjectsCreators;

public class LineCreator : IGraphicObjectCreator
{
    private readonly Func<Point, Point, IEnumerable<Point>> _lineAlgorithm;

    public LineCreator(Func<Point, Point, IEnumerable<Point>> lineAlgorithm)
    {
        _lineAlgorithm = lineAlgorithm;
    }

    public int GetRequiredPointsCount() => 2;

    public IDrwaingGraphicObject? CreateGraphicObject(Color color, int lineThickness,IEnumerable<Point> points)
    {
        var pointList = points.ToList();
        return pointList.Count >= GetRequiredPointsCount() ? new Line(color, lineThickness, pointList[0], pointList[1],  _lineAlgorithm) : null;
    }
}
