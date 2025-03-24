﻿using DrawingEditor.Core.Models;
using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core.GraphickObjectsCreators;

public class HermiteCreator : IGraphicObjectCreator
{
    public int GetRequiredPointsCount() => int.MaxValue;

    public IDrawingGraphicObject? CreateGraphicObject(Color color, int lineThickness, IEnumerable<Point> points)
    {
        var pointList = points.ToList();
        if (points.Count()<3) return null;


        return new HermiteForm(color, lineThickness, pointList);
    }
}
