using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

public class Circle: GraphicObjectBase
{
    public Point Center { get; set; }

    public int  Radius { get; set; }

    public Circle(Color lineColor, float lineThickness,Point center, int radius) 
        : base(lineColor, lineThickness)
    {
        Center=center;
        Radius=radius;
    }

    public override IEnumerable<Point> GetPoints()
    {
        return BresenhamCircle(Center.X, Center.Y, Radius);
       
    }

    

    public override IEnumerable<Point> GetControlPoints()
    {
        return GetPoints();
    }

    public override IEnumerable<(Point point, double intensity)> GetPointsWithIntensity()
    {
        return GetPoints().Select(p => (p, 1.0));
    }

    private IEnumerable<Point> BresenhamCircle(int _x, int _y, int radius)
    {
        int x = 0, y = radius, gap = 0, delta = (2 - 2 * radius);
        while (y >= 0)
        {
            yield return new Point(_x + x, _y + y);
            yield return new Point(_x + x, _y - y);
            yield return new Point(_x - x, _y - y);
            yield return new Point(_x - x, _y + y);
            gap = 2 * (delta + y) - 1;
            if (delta < 0 && gap <= 0)
            {
                x++;
                delta += 2 * x + 1;
                continue;
            }
            if (delta > 0 && gap > 0)
            {
                y--;
                delta -= 2 * y + 1;
                continue;
            }
            x++;
            delta += 2 * (x - y);
            y--;
        }
    }

}


/*
 
public IEnumerable<Point> BresenhamCircle(int _x, int _y)
    {
        void DioganalStep(ref int x, ref int y, ref int delta)
        {
            x = x + 1;
            y = y - 1;
            delta = delta + 2 * x - 2 * y + 2;
        }

        var points = new List<Point>();
        int x = 0,
            y = Radius,
            gap = 0,
            delta = 2 - 2 * Radius;

        points.Add(new Point(x, y));

        while (y > delta)
        {
            if (delta < 0)
            {
                gap = 2 * delta + 2 * y - 1;
                if (gap > 0)
                {
                    DioganalStep(ref x, ref y, ref delta);
                }
                else
                {
                    x = x + 1;
                    delta = delta + 2 * x + 1;
                }
            }
            else if (delta > 0)
            {
                gap = 2 * delta - 2 * x - 1;
                if (gap > 0)
                {
                    y = y - 1;
                    delta = delta - 2 * y + 1;
                }
                else
                {
                    DioganalStep(ref x, ref y, ref delta);
                }
            }
            else
            {
                DioganalStep(ref x, ref y, ref delta);
            }
            points.Add(new Point(x, y));
        }

        return points;
    }

 */