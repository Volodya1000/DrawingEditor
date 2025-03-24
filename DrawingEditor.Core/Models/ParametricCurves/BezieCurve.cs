using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core.Models;

public class BezieCurve : GraphicObjectBase, IEditableGraphicObject, IConnectable
{
    public IList<Point> Points { get; set; }

    private int numSteps;

    public BezieCurve(Color lineColor, float lineThickness, IEnumerable<Point> points, int numSteps = 100)
        : base(lineColor, lineThickness)
    {
        Points = points.ToList();
        this.numSteps = numSteps;
        if (Points.Count() < 3)
        {
            throw new ArgumentException("Необходимо как минимум три точки для построения кривой.");
        }
    }


    public override IEnumerable<Point> GetPoints()
    {
        return GenerateBezierCurve();
    }


    public IEnumerable<Point> GenerateBezierCurve()
    {
        List<Point> controlPoints = new List<Point>(Points);

        // Обрабатываем по 4 точки для кубических кривых Безье (3 степень)
        for (int segment = 0; segment <= controlPoints.Count - 4; segment += 3)
        {
            // Сегмент кривой определяется 4 точками:
            Point startPoint = controlPoints[segment];       // P0 (начальная вершина)
            Point control1 = controlPoints[segment + 1];     // P1 (первая контрольная)
            Point control2 = controlPoints[segment + 2];     // P2 (вторая контрольная)
            Point endPoint = controlPoints[segment + 3];     // P3 (конечная вершина)

            // Генерируем точки для плавной кривой
            for (int step = 0; step <= 1000; step++)
            {
                double t = step / 1000.0;
                yield return CalculateBezierPoint(t, startPoint, control1, control2, endPoint);
            }
        }
    }

    private Point CalculateBezierPoint(double t, Point p0, Point p1, Point p2, Point p3)
    {
        double u = 1 - t;

        // 
        double basis0 = u * u * u;          // B0,3(u) = (1-t)^3
        double basis1 = 3 * u * u * t;      // B1,3(u) = 3(1-t)^2 t
        double basis2 = 3 * u * t * t;      // B2,3(u) = 3(1-t) t^2
        double basis3 = t * t * t;          // B3,3(u) = t^3

        // Вычисление координат точки кривой (свойство аффинной инвариантности)
        double x = basis0 * p0.X +
                  basis1 * p1.X +
                  basis2 * p2.X +
                  basis3 * p3.X;

        double y = basis0 * p0.Y +
                  basis1 * p1.Y +
                  basis2 * p2.Y +
                  basis3 * p3.Y;

        // Обеспечиваем прохождение через концевые точки (при t=0 и t=1)
        // и автоматическое соблюдение выпуклой оболочки благодаря природе кривых Безье
        return new Point(
            (int)Math.Round(x, MidpointRounding.AwayFromZero),
            (int)Math.Round(y, MidpointRounding.AwayFromZero)
        );
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

    public void UpdatePoints(IEnumerable<Point> Points)
    {
        Points = Points.ToList();
    }


}
