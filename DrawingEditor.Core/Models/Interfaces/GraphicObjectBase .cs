
using System.Drawing;

namespace DrawingEditor.Core.Models.Interfaces;

public abstract class GraphicObjectBase : IDrwaingGraphicObject
{
    public Color LineColor { get; set; }
    public float LineThickness { get; set; }

    protected GraphicObjectBase(Color lineColor, float lineThickness)
    {
        LineColor = lineColor;
        LineThickness = lineThickness;
    }

    // Абстрактные методы, специфичные для каждой фигуры
    public abstract IEnumerable<Point> GetPoints();
    public abstract IEnumerable<Point> GetControlPoints();
    public abstract IEnumerable<(Point point, double intensity)> GetPointsWithIntensity();
}
