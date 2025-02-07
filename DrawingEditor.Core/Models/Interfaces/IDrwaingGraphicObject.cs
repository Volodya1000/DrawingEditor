using System.Drawing;

namespace DrawingEditor.Core.Models.Interfaces;

public interface IDrwaingGraphicObject
{
    public IEnumerable<Point> GetPoints(); 

    //public bool Intersect(Point Point);

    //public void Move(Point Point);

}
