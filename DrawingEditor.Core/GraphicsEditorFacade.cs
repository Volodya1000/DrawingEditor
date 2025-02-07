using DrawingEditor.Core.Algorithms.SegmentAlgorithms.Interfaces;
using DrawingEditor.Core;
using DrawingEditor.Core.Models;
using System.Drawing;
using DrawingEditor.Core.Models.Interfaces;
using DrawingEditor.Core.Algorithms.LineAlgorithms;

namespace DrawingEditor.Core;


//public class GraphicsEditorFacade : IDrwaingGraphicObject
//{
//    private readonly CanvasModel _canvasModel;
//    private readonly LineDrawer _lineDrawer;
//    private float _scale = 1.0f; // Масштаб по умолчанию

//    public GraphicsEditorFacade(CanvasModel canvasModel, ILineDrawingAlgorithm initialAlgorithm = null)
//    {
//        _canvasModel = canvasModel;
//        _lineDrawer = new LineDrawer(initialAlgorithm ?? new DDAAlgorithm());
//    }

//    // Метод для изменения масштаба
//    public void SetScale(float scale)
//    {
//        _scale = scale;
//    }

//    // Получить текущий масштаб
//    public float GetScale()
//    {
//        return _scale;
//    }

//    public void SetSegmentAlgorithm(ILineDrawingAlgorithm algorithm)
//    {
//        _lineDrawer.SetAlgorithm(algorithm);
//    }

//    // Масштабирование координат при добавлении линии
//    public void AddLine(Point start, Point end)
//    {
//        var scaledStart = new Point((int)(start.X * _scale), (int)(start.Y * _scale));
//        var scaledEnd = new Point((int)(end.X * _scale), (int)(end.Y * _scale));

//        var segment = _lineDrawer.DrawLine(scaledStart, scaledEnd);
//        _canvasModel.AddObject(segment);
//    }

//    // Получение точек с учетом масштаба
//    public IEnumerable<Point> GetPoints()
//    {
//        var points = _canvasModel.GetPoints();
//        foreach (var point in points)
//        {
//            // Масштабируем точки перед возвратом
//            yield return new Point((int)(point.X * _scale), (int)(point.Y * _scale));
//        }
//    }
//}



public class GraphicsEditorFacade : IDrwaingGraphicObject
{
    //private readonly CanvasModel _canvasModel;
    //private readonly LineDrawer _lineDrawer;

    private readonly List<IDrwaingGraphicObject> _DrwaingGraphicObject = new();



    private Func<Point, Point, IEnumerable<Point>> _currentLineAlgorithm;

    public GraphicsEditorFacade(Func<Point, Point, IEnumerable<Point>> initialAlgorithm=null)
    {
        _currentLineAlgorithm = initialAlgorithm ?? LineDrawingAlgorithms.CDADraw;
    }

    // Метод для добавления линии
    public void AddLine(Point start, Point end)
    {
        var line = new Line(start, end, _currentLineAlgorithm);
        _DrwaingGraphicObject.Add(line);
    }

    // Возвращает все точки всех линий
    public IEnumerable<Point> GetPoints()
    {
        return _DrwaingGraphicObject.SelectMany(line => line.GetPoints());
    }

    // Позволяет сменить алгоритм для добавления новых линий
    public void SetLineAlgorithm(Func<Point, Point, IEnumerable<Point>> newAlgorithm)
    {
        _currentLineAlgorithm = newAlgorithm ?? throw new ArgumentNullException(nameof(newAlgorithm));
    }

    //public GraphicsEditorFacade(CanvasModel canvasModel, ILineDrawingAlgorithm initialAlgorithm = null)
    //{
    //    _canvasModel = canvasModel;
    //    _lineDrawer = new LineDrawer(initialAlgorithm ?? new DDAAlgorithm());
    //}

    //public void SetSegmentAlgorithm(ILineDrawingAlgorithm algorithm)
    //{
    //    _lineDrawer.SetAlgorithm(algorithm);
    //}

    //public void AddLine(Point start, Point end)
    //{
    //    var segment = _lineDrawer.DrawLine(start, end);
    //    _canvasModel.AddObject(segment);
    //}

    //public IEnumerable<Point> GetPoints() => _canvasModel.GetPoints();
}
