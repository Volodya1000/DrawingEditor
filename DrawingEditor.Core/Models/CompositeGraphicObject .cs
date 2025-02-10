using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core.Models;

//public class CompositeGraphicObject : IDrwaingGraphicObject
//{
//    private readonly List<IDrwaingGraphicObject> _graphicObjects;

//    public CompositeGraphicObject(IEnumerable<IDrwaingGraphicObject> graphicObjects)
//    {
//        _graphicObjects = graphicObjects.ToList();
//    }

//    public IEnumerable<Point> GetPoints()
//    {
//        return _graphicObjects.SelectMany(go => go.GetPoints());
//    }
//}
