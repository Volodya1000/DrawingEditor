using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;
using System.Reflection;

namespace DrawingEditor.Core;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using System;
using System.Collections.Generic;
using System.Linq;

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



/*
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

    //// Метод для вычисления параметров параболы через 3 точки
    //public (float h, float k, float a) CalculateParabolaParameters()
    //{
    //    // Получаем координаты точек
    //    float x1 = P1.X, y1 = P1.Y;
    //    float x2 = P2.X, y2 = P2.Y;
    //    float x3 = P3.X, y3 = P3.Y;

    //    // Решение системы линейных уравнений для нахождения a, h, k
    //    // y = a * (x - h)^2 + k
    //    // Подставляем 3 точки в это уравнение и решаем систему для a, h, k.

    //    float denominator = (x1 - x2) * (x1 - x3) * (x2 - x3);
    //    float a = ((x2 - x3) * (y1 - y2) + (x3 - x1) * (y2 - y3) + (x1 - x2) * (y3 - y1)) / denominator;
    //    float h = ((x2 - x3) * (y1 - y2) + (x3 - x1) * (y2 - y3) + (x1 - x2) * (y3 - y1)) / denominator;
    //    float k = (x2 * (x3 - x1) * (y1 - y2) + x1 * (x2 - x3) * (y2 - y3) + x3 * (x1 - x2) * (y3 - y1)) / denominator;

    //    return (h, k, a);
    //}

    // Метод для получения точек параболы
    public override IEnumerable<Point> GetPoints()
    {
        return GenerateParabolaPoints(2,100);
    }

    public static IEnumerable<Point> GenerateParabolaPoints(double p, double xLimit)
    {
        for (double x = 0; x <= xLimit; x += 0.1) // Шаг можно регулировать для точности
        {
            double ySquared = 2 * p * x;
            if (ySquared < 0) continue; // Для x, где y^2 < 0, точки не существуют в реальных числах

            double y = Math.Sqrt(ySquared); // Положительная корень
            yield return new Point((int)x, (int)y);  // Возвращаем точку с положительным y

            if (y != 0)
            {
                yield return new Point((int)x, (int)-y); // Возвращаем точку с отрицательным y
            }
        }
    }

    // Метод для получения контрольных точек (можно использовать те же точки P1, P2, P3)
    public override IEnumerable<Point> GetControlPoints()
    {
        return new List<Point> { P1, P2, P3 };
    }

    // Метод для получения точек с интенсивностью
    public override IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
    {
        return GetPoints().Select(p => (p, 1.0));
    }

    // Метод рисования параболы (по аналогии с вашим примером, с параметрами a, h, k)
    public static IEnumerable<PointF> DrawParabola(float h, float k, float a)
    {
        float step = 0.1f; // Шаг для точности
        for (float x = h - 5; x <= h + 5; x += step) // Диапазон по x можно регулировать
        {
            float y = a * (x - h) * (x - h) + k;
            yield return new PointF(x, y);
        }
    }
}
*/