using System.Drawing;

namespace DrawingEditor.Core.Models.Interfaces;

public interface IDrwaingGraphicObject
{
    public IEnumerable<Point> GetPoints();

    public IEnumerable<Point> GetControlPoints();

    public IEnumerable<(Point point, double intensity)> GetPointsWithIntensity();

    public Color LineColor { get; set; }

    public float LineThickness { get; set; }
    //public bool Intersect(Point Point);

    //public void Move(Point Point);

}



