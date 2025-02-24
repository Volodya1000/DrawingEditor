using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;
public class Elipce : GraphicObjectBase
{
    public Point Center { get; set; }

    public Point AB { get; set; }

    public Elipce(Color lineColor, float lineThickness, Point center,  Point ab)
        : base(lineColor, lineThickness)
    {
        Center = center;
        AB = ab;
    }

    public override IEnumerable<Point> GetPoints()
    {
        return Ellipse(Center.X, Center.Y, AB.X,AB.Y);
    }

    public override IEnumerable<Point> GetControlPoints()
    {
        return GetPoints();
    }

    public override IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
    {
        return GetPoints().Select(p => (p, 1.0));
    }

    public IEnumerable<Point> Ellipse(int _x, int _y, int a, int b)
    {
        int _x0 = 0, _y0 = b; // Начальная точка на оси Y
        int a_sqr = a * a, b_sqr = b * b; // Квадраты полуосей
        int delta = 4 * b_sqr * (_x0 + 1) * (_x0 + 1) + a_sqr * (2 * _y0 - 1) * (2 * _y0 - 1) - 4 * a_sqr * b_sqr;

        // Метод для симметричного добавления точек эллипса
        IEnumerable<Point> YieldEllipsePoints(int x0, int y0)
        {
            yield return new Point(_x + x0, _y + y0); // Верхний правый квадрант
            yield return new Point(_x + x0, _y - y0); // Нижний правый квадрант
            yield return new Point(_x - x0, _y - y0); // Нижний левый квадрант
            yield return new Point(_x - x0, _y + y0); // Верхний левый квадрант
        }

        // Первая часть дуги (верхняя часть эллипса)
        while (a_sqr * (2 * _y0 - 1) > 2 * b_sqr * (_x0 + 1))
        {
            foreach (var p in YieldEllipsePoints(_x0, _y0))
                yield return p;

            if (delta < 0) // Следующая точка ближе к краю эллипса
            {
                _x0++;
                delta += 4 * b_sqr * (2 * _x0 + 3);
            }
            else // Уменьшаем _y0, так как эллипс сужается
            {
                _x0++;
                _y0--;
                delta += 4 * b_sqr * (2 * _x0 + 3) - 8 * a_sqr * _y0;
            }
        }

        // Вторая часть дуги (нижняя часть эллипса)
        delta = b_sqr * (2 * _x0 + 1) * (2 * _x0 + 1) + 4 * a_sqr * (_y0 + 1) * (_y0 + 1) - 4 * a_sqr * b_sqr;

        while (_y0 >= 0)
        {
            foreach (var p in YieldEllipsePoints(_x0, _y0))
                yield return p;

            if (delta < 0) // Если точка внутри эллипса
            {
                _y0--;
                delta += 4 * a_sqr * (2 * _y0 + 3);
            }
            else // Если точка выходит за границы эллипса
            {
                _y0--;
                _x0++;
                delta += 4 * a_sqr * (2 * _y0 + 3) - 8 * b_sqr * _x0;
            }
        }
    }



}

