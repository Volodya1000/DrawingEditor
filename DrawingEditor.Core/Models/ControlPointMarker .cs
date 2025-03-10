using System;
using System.Drawing;
using DrawingEditor.Core.Models.Interfaces;

namespace DrawingEditor.Core;

public class ControlPointMarker : IDrawingGraphicObject
{
    private Point location;
    private Color color;
    private float lineThickness;

    public ControlPointMarker(Point location, Color color, float lineThickness)
    {
        this.location = location;
        this.color = color;
        this.lineThickness = lineThickness;
    }

    public IEnumerable<Point> GetPoints()
    {
        // Генерируем точки для квадрата lineThickness * lineThickness
        int halfSize = (int)(lineThickness / 2);
        for (int dx = -halfSize; dx <= halfSize; dx++)
        {
            for (int dy = -halfSize; dy <= halfSize; dy++)
            {
                yield return new Point(location.X + dx, location.Y + dy);
            }
        }
    }

    public IEnumerable<Point> GetControlPoints() => new[] { location };

    public IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
    {
        // Возвращаем центральную точку с интенсивностью 1.0
         return GetPoints().Select(p => (p, 1.0)); 
    }

    public Color LineColor { get => color; set => color = value; }
    public float LineThickness { get => lineThickness; set => lineThickness = value; }
}