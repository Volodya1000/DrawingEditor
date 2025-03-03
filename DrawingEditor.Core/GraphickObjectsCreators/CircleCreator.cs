using DrawingEditor.Core.Models.Interfaces;
using System.Collections.Generic;
using System.Drawing;

namespace DrawingEditor.Core.GraphickObjectsCreators;

public class CircleCreator : IGraphicObjectCreator
{
    public int GetRequiredPointsCount() => 2;

    public IDrwaingGraphicObject? CreateGraphicObject(Color color, int lineThickness, IEnumerable<Point> points)
    {
        var pointList = points.ToList();
        if (pointList.Count < GetRequiredPointsCount()) return null;
        return new Circle(color, lineThickness,  points.ToList());
    }
}
