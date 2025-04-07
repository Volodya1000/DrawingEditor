using DrawingEditor.Core.Algorithms.LineAlgorithms;
using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;
using System.Numerics;

namespace DrawingEditor.Core;

internal class Cube : GraphicObjectBase
{
    public Cube(Color lineColor, float lineThickness, Point p1, Point p2) 
        : base(lineColor, lineThickness) 
    {
        // Устанавливаем якорную точку как середину между p1 и p2
        AnchorPoint = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);

        // Рассчитываем расстояние между точками
        double dx = p2.X - p1.X;
        double dy = p2.Y - p1.Y;
        double distance = Math.Sqrt(dx * dx + dy * dy);

        // Устанавливаем углы поворота по умолчанию (30 градусов вокруг X и Y)
        double defaultRotationX =20 * Math.PI / 180;
        double defaultRotationY = 55 * Math.PI / 180;
        double defaultRotationZ = 0;

        // Рассчитываем проекцию пространственной диагонали для единичного куба
        Vector3 spaceDiagonal = new Vector3(1, 1, 1);
        Vector3 rotated = ApplyRotationWithAngles(spaceDiagonal, defaultRotationX, defaultRotationY, defaultRotationZ);
        Point projected = Project(rotated);
        double projectedLength = Math.Sqrt(projected.X * projected.X + projected.Y * projected.Y);

        // Рассчитываем длину ребра
        EdgeLength = distance / projectedLength;

        // Устанавливаем повороты
        RotationX = defaultRotationX;
        RotationY = defaultRotationY;
        RotationZ = defaultRotationZ;
    }

    public double EdgeLength { get; set; }
    public Point AnchorPoint { get; set; }
    public double RotationX { get; set; }
    public double RotationY { get; set; }
    public double RotationZ { get; set; }

    // Метод возвращает вершины куба после трансформаций
    public override IEnumerable<Point> GetPoints()
    {
        //double half = EdgeLength / 2;
        //List<Vector3> vertices = new List<Vector3>(8);

        //// Порядок генерации вершин важен для корректного определения ребер
        //for (int x = -1; x <= 1; x += 2)
        //    for (int y = -1; y <= 1; y += 2)
        //        for (int z = -1; z <= 1; z += 2)
        //        {
        //            var vertex = new Vector3(x * half, y * half, z * half);
        //            Vector3 rotated = ApplyRotation(vertex);
        //            Point projected = Project(rotated);
        //            yield return new Point(
        //                projected.X + AnchorPoint.X,
        //                projected.Y + AnchorPoint.Y
        //            );
        //        }
        var allPoints = new List<Point>();
        foreach(var edge in GetEdges())
        {
            
            allPoints.AddRange(LineDrawingAlgorithms.BresenhamDraw(edge.Start, edge.End));
        }
        return allPoints;
    }

    // Метод возвращает ребра куба как пары точек
    public IEnumerable<(Point Start, Point End)> GetEdges()
    {
        Point[] points = GetControlPoints().ToArray();

        // Индексы вершин, образующих ребра (12 ребер)
        int[][] edges = {
            new[] {0, 4}, new[] {1, 5}, new[] {2, 6}, new[] {3, 7}, // По оси X
            new[] {0, 2}, new[] {1, 3}, new[] {4, 6}, new[] {5, 7}, // По оси Y
            new[] {0, 1}, new[] {2, 3}, new[] {4, 5}, new[] {6, 7}  // По оси Z
        };

        foreach (var edge in edges)
        {
            yield return (points[edge[0]], points[edge[1]]);
        }
    }


    // Новая версия метода для вращения с явным указанием углов
    private static Vector3 ApplyRotationWithAngles(
        Vector3 point,
        double rx,
        double ry,
        double rz)
    {
        Vector3 result = point;

        // Вращение вокруг X
        if (rx != 0)
        {
            double cosX = Math.Cos(rx);
            double sinX = Math.Sin(rx);
            double y = result.Y * cosX - result.Z * sinX;
            double z = result.Y * sinX + result.Z * cosX;
            result = new Vector3(result.X, y, z);
        }

        // Вращение вокруг Y
        if (ry != 0)
        {
            double cosY = Math.Cos(ry);
            double sinY = Math.Sin(ry);
            double x = result.X * cosY + result.Z * sinY;
            double z = -result.X * sinY + result.Z * cosY;
            result = new Vector3(x, result.Y, z);
        }

        // Вращение вокруг Z
        if (rz != 0)
        {
            double cosZ = Math.Cos(rz);
            double sinZ = Math.Sin(rz);
            double x = result.X * cosZ - result.Y * sinZ;
            double y = result.X * sinZ + result.Y * cosZ;
            result = new Vector3(x, y, result.Z);
        }

        return result;
    }

    // Существующий метод вращения использует текущие углы объекта
    private Vector3 ApplyRotation(Vector3 point)
    {
        return ApplyRotationWithAngles(point, RotationX, RotationY, RotationZ);
    }

    private Point Project(Vector3 point)
    {
        // Ортографическая проекция на плоскость XY
        return new Point((int)point.X,(int) point.Y);
    }

    public override IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
    {
        return GetPoints().Select(p => (p, 1.0));
    }

    public override IEnumerable<Point> GetControlPoints()
    {
        double half = EdgeLength / 2;
        List<Vector3> vertices = new List<Vector3>(8);

        // Порядок генерации вершин важен для корректного определения ребер
        for (int x = -1; x <= 1; x += 2)
            for (int y = -1; y <= 1; y += 2)
                for (int z = -1; z <= 1; z += 2)
                {
                    var vertex = new Vector3(x * half, y * half, z * half);
                    Vector3 rotated = ApplyRotation(vertex);
                    Point projected = Project(rotated);
                    yield return new Point(
                        projected.X + AnchorPoint.X,
                        projected.Y + AnchorPoint.Y
                    );
                }
    }
}

public struct Vector3
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public Vector3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}
