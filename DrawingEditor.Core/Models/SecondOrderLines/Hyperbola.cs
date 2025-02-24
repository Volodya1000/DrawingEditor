using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

public class Hyperbola : GraphicObjectBase
{
    public Point Center { get; set; }
    public double A { get; set; }
    public double B { get; set; }
    public Point LimitPoint { get; set; }

    public Hyperbola(Color lineColor, float lineThickness, Point center, Point coefficientPoint, Point limitPoint)
        : base(lineColor, lineThickness)
    {
        Center = center;
        LimitPoint = limitPoint;
        CalculateHyperbolaParameters(coefficientPoint);
    }

    // Метод для вычисления параметров гиперболы
    private void CalculateHyperbolaParameters(Point coefficientPoint)
    {
        // Рассчитываем расстояние от центра до coefficientPoint
        double distanceToCoefficientPoint = Math.Sqrt(Math.Pow(coefficientPoint.X - Center.X, 2) + Math.Pow(coefficientPoint.Y - Center.Y, 2));

        // Если расстояние равно нулю, задаем минимальные значения для A и B
        if (distanceToCoefficientPoint == 0)
        {
            A = 1; // Минимальное значение для A
            B = 1; // Минимальное значение для B
        }
        else
        {
            // Рассчитываем коэффициенты A и B на основе расстояния и угла
            // Мы будем использовать расстояние для масштабирования A и B, а угол для их соотношения

            // Рассчитываем угол между координатами coefficientPoint и осью X
            double angle = Math.Atan2(coefficientPoint.Y - Center.Y, coefficientPoint.X - Center.X);

            // Используем расстояние для масштабирования A и B
            double scaleFactor = distanceToCoefficientPoint / 10; // Масштабируем на основе расстояния

            // Регулируем соотношение A и B на основе угла
            // Если угол близок к 0 или π, гипербола должна быть более широкой (A > B)
            // Если угол близок к π/2 или 3π/2, гипербола должна быть более узкой (B > A)
            if (Math.Abs(angle) < Math.PI / 4 || Math.Abs(angle) > 3 * Math.PI / 4)
            {
                A = scaleFactor * 2; // Сделаем A больше, чтобы гипербола была шире
                B = scaleFactor; // B меньше, чтобы гипербола была шире
            }
            else
            {
                A = scaleFactor; // A меньше, чтобы гипербола была уже
                B = scaleFactor * 2; // B больше, чтобы гипербола была уже
            }

            // Убедимся, что A и B не слишком малы, чтобы гипербола не выродилась
            A = Math.Max(A, 0.1); // Минимальное значение для A
            B = Math.Max(B, 0.1); // Минимальное значение для B
        }
    }




    // Метод для получения точек гиперболы
    public override IEnumerable<Point> GetPoints()
    {
        // Генерируем точки гиперболы
        return GeneratePoints(Center, A, B, LimitPoint);
    }

    // Метод для генерации точек гиперболы
    private IEnumerable<Point> GeneratePoints(Point center, double a, double b, Point limitPoint)
    {
        // Определяем максимальное и минимальное значения X для генерации точек
        // Это делается на основе расстояния от центра до limitPoint и абсолютного значения X центра
        double maxX = Math.Max(Math.Abs(limitPoint.X - center.X), Math.Abs(center.X));
        double minX = Math.Min(Math.Abs(limitPoint.X - center.X), Math.Abs(center.X));

        // Генерируем точки гиперболы в диапазоне от center.X - maxX до center.X + maxX
        for (double x = center.X - maxX; x <= center.X + maxX; x += 0.01)
        {
            // Проверяем, находится ли текущая точка внутри области, ограниченной limitPoint
            if (Math.Abs(x - center.X) <= Math.Abs(limitPoint.X - center.X))
            {
                // Рассчитываем квадрат значения Y для текущей точки X
                // Используем уравнение гиперболы: y^2 = (b^2) * (1 + (x^2 / a^2))
                double ySquared = (b * b) * (1 + ((x - center.X) * (x - center.X)) / (a * a));

                // Проверяем, является ли квадрат значения Y неотрицательным
                if (ySquared >= 0)
                {
                    // Рассчитываем значение Y и генерируем две точки: одну выше центра, другую ниже
                    double y = Math.Sqrt(ySquared) + center.Y;
                    yield return new Point((int)x, (int)y); // Округляем до ближайшего целого
                    yield return new Point((int)x, (int)(center.Y - Math.Sqrt(ySquared))); // Для симметрии
                }
            }
        }
    }


    // Метод для получения контрольных точек
    public override IEnumerable<Point> GetControlPoints()
    {
        return new List<Point> { Center, new Point(Center.X + (int)A, Center.Y), new Point(Center.X, Center.Y + (int)B), LimitPoint };
    }

    // Метод для получения точек с интенсивностью
    public override IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
    {
        return GetPoints().Select(p => (p, 1.0));
    }
}


