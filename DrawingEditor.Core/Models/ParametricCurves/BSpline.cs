
using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

public class BSpline : GraphicObjectBase, IEditableGraphicObject, IConnectable
{
    public IList<Point> Points { get; set; }

    private int numSteps;

    public BSpline(Color lineColor, float lineThickness, IEnumerable<Point> points, int numSteps = 100)
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
        return DrawBsplineCurve();
    }

    public IEnumerable<Point> DrawBsplineCurve()
    {
        if (Points.Count < 4) // Минимум 4 точки для кубического B-сплайна
            return Enumerable.Empty<Point>();

        List<Point> curvePoints = new List<Point>();

        // Берем исходные точки без расширения
        List<Point> controlPoints = new List<Point>(Points);

        for (int i = 0; i <= controlPoints.Count - 4; i++)
        {
            Point p0 = controlPoints[i];
            Point p1 = controlPoints[i + 1];
            Point p2 = controlPoints[i + 2];
            Point p3 = controlPoints[i + 3];

            double[,] mBspline = {
            { -1/6.0,  3/6.0, -3/6.0, 1/6.0 },
            {  3/6.0, -6/6.0,  3/6.0, 0.0   },
            { -3/6.0,  0.0,    3/6.0, 0.0   },
            {  1/6.0,  4/6.0,  1/6.0, 0.0   }
        };

            foreach (double t in Linspace(0, 1, 100))
            {
                double t3 = t * t * t;
                double t2 = t * t;
                double[] tVector = { t3, t2, t, 1 };

                double x = CalculateCoordinate(mBspline, p0.X, p1.X, p2.X, p3.X, tVector);
                double y = CalculateCoordinate(mBspline, p0.Y, p1.Y, p2.Y, p3.Y, tVector);

                curvePoints.Add(new Point((int)x, (int)y));
            }
        }

        return curvePoints;
    }


    // Вспомогательные методы
    private double CalculateCoordinate(double[,] matrix, double p0, double p1, double p2, double p3, double[] tVector)
    {
        double[] coefficients = new double[4];
        for (int i = 0; i < 4; i++)
        {
            coefficients[i] = matrix[i, 0] * p0 +
                             matrix[i, 1] * p1 +
                             matrix[i, 2] * p2 +
                             matrix[i, 3] * p3;
        }

        return tVector[0] * coefficients[0] +
               tVector[1] * coefficients[1] +
               tVector[2] * coefficients[2] +
               tVector[3] * coefficients[3];
    }

    private IEnumerable<double> Linspace(double start, double end, int count)
    {
        double step = (end - start) / (count - 1);
        for (int i = 0; i < count; i++)
        {
            yield return start + step * i;
        }
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
