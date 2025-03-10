using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;
using System.Reflection;

namespace DrawingEditor.Core.Models;


public class HermiteForm : GraphicObjectBase, IEditableGraphicObject, IConnectable
{
    public IList<Point> Points { get; set; }

    private int numSteps;

    public HermiteForm(Color lineColor, float lineThickness, IEnumerable<Point> points, int numSteps = 100)
        : base(lineColor, lineThickness)
    {
        Points= points.ToList();
        this.numSteps = numSteps;
        if (Points.Count() < 3)
        {
            throw new ArgumentException("Необходимо как минимум три точки для построения кривой.");
        }
    }


    public override IEnumerable<Point> GetPoints()
    {
        int N = Points.Count();
        double step = 1.0 / numSteps;

        // Обычные сегменты
        for (int i = 0; i < N - 2; i++)
            for (double t = 0; t <= 1; t += step)
                yield return CalculatePoint(i, i + 1, i + 2, t);

        // Последний сегмент с периодизацией
        for (double t = 0; t <= 1; t += step)
            yield return CalculatePoint(N - 2, N - 1, 0, t);

        // Сегмент между последней и первой точкой
        //for (double t = 0; t <= 1; t += step)
        //    yield return CalculatePoint(N - 1, 0, 1, t);
    }

    private Point CalculatePoint(int p0, int p1, int p2, double t)
    {
        int x = (int)(
            Points[p0].X * (2 * t * t * t - 3 * t * t + 1) +
            Points[p1].X * (3 * t * t - 2 * t * t * t) +
            (Points[p1].X - Points[p0].X) * (t * t * t - 2 * t * t + t) +
            (Points[p2].X - Points[p1].X) * (t * t * t - t * t)
        );

        int y = (int)(
            Points[p0].Y * (2 * t * t * t - 3 * t * t + 1) +
            Points[p1].Y * (3 * t * t - 2 * t * t * t) +
            (Points[p1].Y - Points[p0].Y) * (t * t * t - 2 * t * t + t) +
            (Points[p2].Y - Points[p1].Y) * (t * t * t - t * t)
        );

        return new Point(x, y);
    }


    public override IEnumerable<Point> GetControlPoints()
    {
        return Points;
    }

    public override IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
    {
        return GetPoints().Select(p => (p, 1.0));
    }

    public void UpdateControlPoint(int index, Point newPoint)
    {
        Points[index] = newPoint;
    }

    public void UpdatePoints(IEnumerable<Point> points)
    {
        Points = points.ToList();
    }


}
