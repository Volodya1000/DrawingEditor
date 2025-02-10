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

    public IDrwaingGraphicObject? CreateGraphicObject(Color color, IEnumerable<Point> points)
    {
        var pointList = points.ToList();
        return pointList.Count >= 2 ? new WuLine(color, pointList[0], pointList[1], _lineAlgorithm) : null;
    }
}
