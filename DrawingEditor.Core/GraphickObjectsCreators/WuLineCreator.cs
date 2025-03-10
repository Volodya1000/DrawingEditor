using DrawingEditor.Core.GraphickObjectsCreators;
using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

public class WuLineCreator: IGraphicObjectCreator
{
    private readonly Func<Point, Point, IEnumerable<(Point point, double intensity)>> _lineAlgorithm;

    public WuLineCreator(Func<Point, Point, IEnumerable<(Point point, double intensity)>> lineAlgorithm)
    {
        _lineAlgorithm = lineAlgorithm;
    }

    public int GetRequiredPointsCount() => 2;

    public IDrawingGraphicObject? CreateGraphicObject(Color color, int lineThickness, IEnumerable<Point> points)
    {
        var pointList = points.ToList();
        return pointList.Count >= GetRequiredPointsCount() ? new WuLine(color, lineThickness, pointList[0], pointList[1], _lineAlgorithm) : null;
    }
}
