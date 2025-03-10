using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;
namespace DrawingEditor.Core;

public class Parabola : GraphicObjectBase
{
    public Point P1 { get; set; }
    public Point P2 { get; set; }
    public Point P3 { get; set; }

    public Parabola(Color lineColor, float lineThickness, Point p1, Point p2, Point p3)
        : base(lineColor, lineThickness)
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;
    }

    // Метод для получения точек параболы
    public override IEnumerable<Point> GetPoints()
    {
        // Вычисляем p на основе P1 и P2
        double p = CalculateP(P1, P2);

        // Если p не удалось вычислить, возвращаем пустое множество
        if (double.IsNaN(p))
        {
            return Enumerable.Empty<Point>();
        }

        // Генерируем точки параболы до точки P3
        return GeneratePoints(p, P1, P3.X);
    }

    // Метод для вычисления p
    private double CalculateP(Point p1, Point p2)
    {
        if (p2.X == p1.X) return double.NaN; // Чтобы избежать деления на ноль
        return (p2.Y - p1.Y) * (p2.Y - p1.Y) / (p2.X - p1.X);
    }

    // Метод для генерации точек параболы
    private IEnumerable<Point> GeneratePoints(double p, Point basePoint, int maxX)
    {
        for (double x = basePoint.X; x <= maxX; x += 0.01) // Используем более мелкий шаг
        {
            double ySquared = p * (x - basePoint.X);
            if (ySquared >= 0)
            {
                double y = Math.Sqrt(ySquared) + basePoint.Y;
                yield return new Point((int)x, (int)y); // Округляем до ближайшего целого
                yield return new Point((int)x, (int)(basePoint.Y - Math.Sqrt(ySquared))); // Для симметрии
            }
        }
    }


    // Метод для получения контрольных точек
    public override IEnumerable<Point> GetControlPoints()
    {
        return new List<Point> { P1, P2, P3 };
    }

    // Метод для получения точек с интенсивностью
    public override IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
    {
        return GetPoints().Select(p => (p, 1.0));
    }
}


