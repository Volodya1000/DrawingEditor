using System.Drawing;

namespace DrawingEditor.Core
{
    // Интерфейс для состояний графического редактора
    public interface IEditorState
    {
        void HandleMouseMove(GraphicsEditorFacade editor, Color color, int lineThickness, Point point);
        void HandlePoint(GraphicsEditorFacade editor, Color color, int lineThickness, Point point);
    }
}


/*using System.Drawing;
using DrawingEditor.Core.Models.Interfaces;
using DrawingEditor.Core.Algorithms.LineAlgorithms;
using DrawingEditor.Core.GraphickObjectsCreators;

namespace DrawingEditor.Core;

// <<Singleton>> <<Facade>>
public class GraphicsEditorFacade //: IInputHandler
{
    private static GraphicsEditorFacade instance;


    private ICreationState currentState;
    private IGraphicObjectCreator currentCreator;

    private CanvasModel canvasModel;

    private IDrwaingGraphicObject? previewGraphicObject;

    private GraphicsEditorFacade(){}


    public static GraphicsEditorFacade GetInstance(IGraphicObjectCreator initialCreator=null)
    {
        if(instance==null)
        {
            instance = new GraphicsEditorFacade();
            initialCreator = new LineCreator(LineDrawingAlgorithms.CDADraw);
            instance.SetCreator(initialCreator);
            instance.canvasModel = new CanvasModel();
        }
        return instance;
    }

    public void SetCreator(IGraphicObjectCreator creator)
    {
        currentCreator = creator;
        currentState = new CreationState(currentCreator.GetRequiredPointsCount());
    }

    public void HandlePoint(Color color, int lineThickness, Point point)
    {
        currentState.AddPoint(point);

        if (currentState.IsReadyToCreate())
        {
            AddGraphicObject(color, lineThickness);
            currentState = new CreationState(currentCreator.GetRequiredPointsCount());
        }
    }

    public void HandleMouseMove(Color color, int lineThickness, Point point)
    {
        // Удаляем старый предпросматриваемый объект
        previewGraphicObject = null;

        // Если уже есть одна точка — создаем новый предпросмотр
        var currentPoints = currentState.GetPoints().ToList();
        if (currentPoints.Count > 0)
        {
            var previewPoints = new List<Point>(currentPoints) { point };
            previewGraphicObject = currentCreator.CreateGraphicObject(color, lineThickness, previewPoints);
        }
    }

    private void AddGraphicObject(Color color,int lineThickness)
    {
        var graphicObject = currentCreator.CreateGraphicObject(color, lineThickness, currentState.GetPoints());
        if (graphicObject != null)
        {
            canvasModel.AddObject(graphicObject);
        }
    }

    public bool Undo()
    {
        return canvasModel.Undo();
    }

    public bool Redo()
    {
        return canvasModel.Redo();
    }


    public IDrwaingGraphicObject? GetPreviewObject()
    {
        return previewGraphicObject;
    }

    public IEnumerable<IDrwaingGraphicObject> GetGraphicObjects()=> canvasModel.GetGraphicObjects();

}
*/



