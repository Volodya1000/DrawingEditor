using System.Drawing;

namespace DrawingEditor.Core.Models.Interfaces;

public interface IConnectable
{
    void UpdatePoints(IEnumerable<Point> points);

}


