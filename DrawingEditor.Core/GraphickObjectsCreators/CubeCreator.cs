using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core.GraphickObjectsCreators;

public class CubeCreator : IGraphicObjectCreator
{
    public int GetRequiredPointsCount() => 2;

    public IDrawingGraphicObject? CreateGraphicObject(Color color, int lineThickness, IEnumerable<Point> points)
    {
        var pointList = points.ToList();
        if (pointList.Count < GetRequiredPointsCount()) return null;


        return new Cube(color, lineThickness, pointList[0], pointList[1]);
    }
}