namespace DrawingEditor.Core.Algorithms.SegmentAlgorithms;

public class BresenhamsAlgorithm
{
    //private void BresenhamAlgorithm(int x1, int y1, int x2, int y2, out string report)
    //{
    //    var repList = new List<BresenhamReport>();
    //    report = string.Empty;
    //    int x = x1, y = y1,
    //        dx = x2 - x1, dy = y2 - y1,
    //        i = 0,
    //        e = 2 * dy - dx;
    //    report += "i: " + i + " e: " + e + " x: " + x + " y: " + y + "\n";

    //    _points.Add((x, y));
    //    i++;

    //    int yAdd = y2 > y1 ? 1 : -1;
    //    int xAdd = x2 > x1 ? 1 : -1;

    //    while (i <= System.Math.Abs(dx))
    //    {
    //        if (e >= 0)
    //        {
    //            y += yAdd;//++;
    //            e -= 2 * dx;
    //        }
    //        x += xAdd; //++;
    //        e += 2 * dy;

    //        _points.Add((x, y));
    //        //repList.Add(new BresenhamReport(i, e, x, y));
    //        report += "i: " + i + " e: " + e + " x: " + x + " y: " + y + "\n";
    //        i++;
    //    }

    //}
}
public record BresenhamReport(int i, int e, int x, int y);