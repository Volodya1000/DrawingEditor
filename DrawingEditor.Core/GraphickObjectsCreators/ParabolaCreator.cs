using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core.GraphickObjectsCreators;

public class ParabolaCreator : IGraphicObjectCreator
{
    public int GetRequiredPointsCount() => 3;

    public IDrawingGraphicObject? CreateGraphicObject(Color color, int lineThickness, IEnumerable<Point> points)
    {
        var pointList = points.ToList();
        if (pointList.Count == 2)
            pointList.Add(pointList[1]);
        if (pointList.Count < GetRequiredPointsCount()) return null;


        return new Parabola(color, lineThickness, pointList[0], pointList[1], pointList[2]);
    }
}
