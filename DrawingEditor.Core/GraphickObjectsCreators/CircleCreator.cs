using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core.GraphickObjectsCreators;

public class CircleCreator : IGraphicObjectCreator
{
    public int GetRequiredPointsCount() => 2;

    public IDrwaingGraphicObject? CreateGraphicObject(Color color, int lineThickness, IEnumerable<Point> points)
    {
        var pointList = points.ToList();
        if (pointList.Count < GetRequiredPointsCount()) return null;

        var center = pointList[0];
        var boundaryPoint = pointList[1];
        double radiusSquare = Math.Pow(center.X - boundaryPoint.X, 2) +
                             Math.Pow(center.Y - boundaryPoint.Y, 2);
        int radius = (int)Math.Sqrt(radiusSquare);

        return new Circle(color, lineThickness, pointList[0], radius);
    }
}
