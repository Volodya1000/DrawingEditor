using System.Drawing;
using DrawingEditor.Core.Models.Interfaces;
using DrawingEditor.Core.Algorithms.LineAlgorithms;
using DrawingEditor.Core.GraphickObjectsCreators;

namespace DrawingEditor.Core;

// Фасад графического редактора
public class GraphicsEditorFacade
{
    private static GraphicsEditorFacade instance;

    private IEditorState currentState;
    private IGraphicObjectCreator currentCreator;
    public CanvasModel CanvasModel { get; private set; }
    private IDrwaingGraphicObject? previewGraphicObject;

    // Радиус для определения попадания в опорную точку
    public int controlPointClickRadius = 5; // Можно настроить

    private GraphicsEditorFacade() { }

    public static GraphicsEditorFacade GetInstance(IGraphicObjectCreator initialCreator = null)
    {
        if (instance == null)
        {
            instance = new GraphicsEditorFacade();
            initialCreator = initialCreator ?? new LineCreator(LineDrawingAlgorithms.CDADraw);
            instance.SetCreator(initialCreator);
            instance.CanvasModel = new CanvasModel();
            instance.SetEditorState(new IdleState());
        }
        return instance;
    }

    public IGraphicObjectCreator CurrentCreator => currentCreator;

    public void SetCreator(IGraphicObjectCreator creator)
    {
        currentCreator = creator;
        SetEditorState(new IdleState());
    }

    // Основной метод обработки клика мыши
    public void HandlePoint(Color color, int lineThickness, Point point)
    {
        if (currentState is DragControlPointState)
        {
            currentState = new IdleState();
            return;
        }
        var selectedData = TrySelectControlPoint(point);
        if (selectedData != null)
        {
            var data = selectedData.Value;
            if (data.graphicObject is IEditableGraphicObject editable)
            {
                SetEditorState(new DragControlPointState(editable, data.controlPointIndex, controlPointClickRadius));
                return;
            }
        }

        var selectedObject = TrySelectObject(point);
        if (selectedObject != null)
        {
            SetEditorState(new ObjectSelectedState(selectedObject));
            return;
        }

        currentState.HandlePoint(this, color, lineThickness, point);
    }

    // Метод обработки перемещения мыши
    public void HandleMouseMove(Color color, int lineThickness, Point point)
    {
        // Если не в режиме перетаскивания, можно менять курсор при наведении
        if (!(currentState is DragControlPointState))
        {
            if (IsMouseOverAnyObject(point))
            {
                // При наведении на объект курсор можно менять на крест с 4 стрелками
                // Например: Cursor.Current = Cursors.Cross;
            }
            else
            {
                // Иначе вернуть курсор по умолчанию
                // Cursor.Current = Cursors.Default;
            }
        }
        currentState.HandleMouseMove(this, color, lineThickness, point);
    }

    public bool Undo() => CanvasModel.Undo();

    public bool Redo() => CanvasModel.Redo();

    public IDrwaingGraphicObject? GetPreviewObject() => previewGraphicObject;

    public void SetEditorState(IEditorState state)
    {
        currentState = state;
    }

    public void SetPreviewGraphicObject(IDrwaingGraphicObject? previewObject)
    {
        previewGraphicObject = previewObject;
    }

    public IEnumerable<IDrwaingGraphicObject> GetObjectsForRendering()
    {
        IEnumerable<IDrwaingGraphicObject> objects = CanvasModel.GetGraphicObjects();

        if (currentState is DragControlPointState dragState && previewGraphicObject != null)
        {
            objects = objects.Where(obj => !object.ReferenceEquals(obj, dragState.EditingObject));
        }
        var additionalObjects = currentState.GetAdditionalRenderingObjects();
        return objects.Concat(additionalObjects);
    }



    // Метод для проверки попадания клика по опорной точке
    private (IDrwaingGraphicObject graphicObject, int controlPointIndex)? TrySelectControlPoint(Point clickPoint)
    {
        foreach (var obj in CanvasModel.GetGraphicObjects())
        {
            var controlPoints = obj.GetControlPoints().ToList();
            for (int i = 0; i < controlPoints.Count; i++)
            {
                if (Distance(clickPoint, controlPoints[i]) <= controlPointClickRadius)
                {
                    return (obj, i);
                }
            }
        }
        return null;
    }

    // Метод для определения, находится ли мышь над объектом (можно проверить по опорным точкам или по bounding box)
    private bool IsMouseOverAnyObject(Point mousePoint)
    {
        foreach (var obj in CanvasModel.GetGraphicObjects())
        {
            foreach (var point in obj.GetControlPoints())
            {
                if (Distance(mousePoint, point) <= controlPointClickRadius * 2)
                    return true;
            }
        }
        return false;
    }

    public double Distance(Point p1, Point p2)
    {
        int dx = p1.X - p2.X;
        int dy = p1.Y - p2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }


    public IDrwaingGraphicObject? TrySelectObject(Point clickPoint)
    {
        foreach (var obj in CanvasModel.GetGraphicObjects())
        {
            foreach (var point in obj.GetPoints())
            {
                if (Distance(clickPoint, point) <= controlPointClickRadius)
                {
                    return obj;
                }
            }
        }
        return null;
    }
}

