
using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core.GraphickObjectsCreators;


public class ElipceCreator : IGraphicObjectCreator
{
    public int GetRequiredPointsCount() => 2;

    public IDrawingGraphicObject? CreateGraphicObject(Color color, int lineThickness, IEnumerable<Point> points)
    {
        var pointList = points.ToList();
        if (pointList.Count < GetRequiredPointsCount()) return null;

        Point ab = new Point(Math.Abs(pointList[1].X - pointList[0].X), Math.Abs(pointList[1].Y - pointList[0].Y));

        return new Elipce(color, lineThickness, pointList[0], ab);
    }
}
