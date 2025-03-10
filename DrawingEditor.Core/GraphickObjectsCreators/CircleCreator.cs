using DrawingEditor.Core.Models.Interfaces;
using System.Collections.Generic;
using System.Drawing;

namespace DrawingEditor.Core.GraphickObjectsCreators;

public class CircleCreator : IGraphicObjectCreator
{
    public int GetRequiredPointsCount() => 2;

    public IDrawingGraphicObject? CreateGraphicObject(Color color, int lineThickness, IEnumerable<Point> points)
    {
        List<Point> Points = points.ToList();
        if (points.Count() < GetRequiredPointsCount())
            throw new ArgumentException();
        Point center = Points[0];
        var boundaryPoint = Points[1];
        double radiusSquare = Math.Pow(center.X - boundaryPoint.X, 2) +
                             Math.Pow(center.Y - boundaryPoint.Y, 2);
        int radius = (int)Math.Sqrt(radiusSquare);
        return new Circle(color, lineThickness, center, radius);
    }
}
