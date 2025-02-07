using DrawingEditor.Core.Algorithms.SegmentAlgorithms.Interfaces;
using DrawingEditor.Core.Models;
using System.Drawing;

namespace DrawingEditor.Core;


// Алгоритм ЦДА

//public class DDAAlgorithm : ILineDrawingAlgorithm
//{
//    public Line DrawLine(Point start, Point end)
//    {
//        var points = new List<Point>();
//        int dx = end.X - start.X;
//        int dy = end.Y - start.Y;
//        int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
//        float xIncrement = dx / (float)steps;
//        float yIncrement = dy / (float)steps;
//        float x = start.X;
//        float y = start.Y;

//        for (int i = 0; i <= steps; i++)
//        {
//            points.Add(new Point((int)Math.Round(x), (int)Math.Round(y)));
//            x += xIncrement;
//            y += yIncrement;
//        }

//        return new Line(points); // Возвращаем объект Line
//    }
//}