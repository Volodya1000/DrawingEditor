using System.Drawing;
using DrawingEditor.Core.Models.Interfaces;
using DrawingEditor.Core.Algorithms.LineAlgorithms;
using DrawingEditor.Core.GraphickObjectsCreators;

namespace DrawingEditor.Core;


public class GraphicsEditorFacade : IInputHandler
{
    private ICreationState currentState;
    private IGraphicObjectCreator currentCreator;

    private readonly CanvasModel canvasModel;

    private IDrwaingGraphicObject? previewGraphicObject;

    public GraphicsEditorFacade(IGraphicObjectCreator initialCreator=null)
    {
        if (initialCreator == null)
            initialCreator = new LineCreator(LineDrawingAlgorithms.CDADraw);
        SetCreator(initialCreator);

        canvasModel = new CanvasModel();
    }

    public void SetCreator(IGraphicObjectCreator creator)
    {
        currentCreator = creator;
        currentState = new CreationState(currentCreator.GetRequiredPointsCount());
    }

    public void HandlePoint(Point point)
    {
        currentState.AddPoint(point);

        if (currentState.IsReadyToCreate())
        {
            AddGraphicObject();
            currentState = new CreationState(currentCreator.GetRequiredPointsCount());
        }
    }

    public void HandleMouseMove(Point point)
    {
        // Удаляем старый предпросматриваемый объект
        previewGraphicObject = null;

        // Если уже есть одна точка — создаем новый предпросмотр
        var currentPoints = currentState.GetPoints().ToList();
        if (currentPoints.Count > 0)
        {
            var previewPoints = new List<Point>(currentPoints) { point };
            previewGraphicObject = currentCreator.CreateGraphicObject(previewPoints);
        }
    }

    private void AddGraphicObject()
    {
        var graphicObject = currentCreator.CreateGraphicObject(currentState.GetPoints());
        if (graphicObject != null)
        {
            canvasModel.AddObject(graphicObject);
        }
    }

    public IEnumerable<Point> GetPoints()
    {
        return canvasModel.GetPoints();
    }

    public IEnumerable<Point> GetPreviewPoints()
    {
        return previewGraphicObject?.GetPoints() ?? Enumerable.Empty<Point>();
    }
}


public class GraphicPoint(Point point) : IDrwaingGraphicObject
{
    public IEnumerable<Point> GetPoints()
    {
        return new List<Point>() { point };
    }
}
