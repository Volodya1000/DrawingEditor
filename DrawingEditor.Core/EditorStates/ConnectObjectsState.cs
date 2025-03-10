using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

internal class ConnectObjectsState : IEditorState
{
    private readonly IDrawingGraphicObject _firstSelectedObject;
    private IDrawingGraphicObject? _secondSelectedObject;

    // true - выбрана первая точка первого объекта , false - последняя, null - никакая не выбрана
    private bool? _isFirstPointSelectedInFirstObject;

    // true - первая точка второго объекта, false - последняя, null - никакая не выбрана
    private bool? _isFirstPointSelectedInSecondObject;

    // Константы для размеров маркеров
    private const float ActiveControlPointSize = 4.0f;
    private const float InactiveControlPointSize = 3.0f;

    private Color ActiveColor = Color.Green;
    private Color InactiveColor = Color.Red;

    public ConnectObjectsState(IDrawingGraphicObject selectedObject)
    {
        _firstSelectedObject = selectedObject;
    }

    public void HandleMouseMove(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {
    }

    public void HandlePoint(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {
        if (!_isFirstPointSelectedInFirstObject.HasValue)
        {
            HandleFirstPointSelection(editor, point);
        }
        else if (_secondSelectedObject == null)
        {
            HandleSecondObjectSelection(editor, point);
        }
        else if (!_isFirstPointSelectedInSecondObject.HasValue)
        {
            HandleSecondPointSelection(editor, point);
        }
    }

    private void HandleFirstPointSelection(GraphicsEditorFacade editor, Point point)
    {
        var controlPoints = _firstSelectedObject.GetControlPoints().ToList();

        for (int i = 0; i < controlPoints.Count; i++)
        {
            if (editor.Distance(point, controlPoints[i]) > editor.controlPointClickRadius)
                continue;

            // Определяем тип выбранной точки
            _isFirstPointSelectedInFirstObject = i switch
            {
                0 => true,                                     // Первая точка
                _ when i == controlPoints.Count - 1 => false,  // Последняя точка
                _ => null
            };
            return;
        }
    }

    private void HandleSecondObjectSelection(GraphicsEditorFacade editor, Point point)
    {
        var newSelected = editor.TrySelectObject(point);
        if (newSelected == null || 
            newSelected == _firstSelectedObject ||
            _firstSelectedObject.GetType()!= newSelected?.GetType())
            return;

        _secondSelectedObject = newSelected;
    }

    private void HandleSecondPointSelection(GraphicsEditorFacade editor, Point point)
    {
        var controlPoints = _secondSelectedObject?.GetControlPoints().ToList();
        if (controlPoints == null)
            return;

        for (int i = 0; i < controlPoints.Count; i++)
        {
            if (editor.Distance(point, controlPoints[i]) > editor.controlPointClickRadius)
                continue;

            if (i == 0)//первая точка объекта
                _isFirstPointSelectedInSecondObject = true;
            else if (i == controlPoints.Count - 1)//Последняя точка объекта
                _isFirstPointSelectedInSecondObject = false;
            else return; // выбрана не крайняя точка второго объекта

            ConnectObjects(editor);
            editor.SetEditorState(new IdleState());
        }
    }

    private void ConnectObjects(GraphicsEditorFacade editor)
    {
        if (_secondSelectedObject == null ||
            !(_firstSelectedObject is IConnectable connectable))
            return;

        var connectedPoints = GetConnectedPoints();
        connectable.UpdatePoints(connectedPoints);

        //удаляем второй объект так как его точки переходят в первый объекты
        editor.RemoveObject(_secondSelectedObject);
    }

    private IEnumerable<Point> GetConnectedPoints()
    {
        var first = _firstSelectedObject.GetControlPoints();
        var second = _secondSelectedObject?.GetControlPoints();
        

        if (_isFirstPointSelectedInFirstObject == true)
            first = first.Reverse();

        if (_isFirstPointSelectedInSecondObject == false)
            second = second?.Reverse();

        return first.Concat(second);
    }

    public IEnumerable<IDrawingGraphicObject> GetAdditionalRenderingObjects()
    {
        List<IDrawingGraphicObject> result = new List<IDrawingGraphicObject>();

        var firstControlPoints = _firstSelectedObject.GetControlPoints().ToList(); // Преобразуем в список для получения длины
        
        var firstObjectFirstPoint = firstControlPoints[0];

        var firsObjectLastPoint = firstControlPoints[firstControlPoints.Count - 1];

        if (_isFirstPointSelectedInFirstObject.HasValue)
        {
            if (_isFirstPointSelectedInFirstObject.Value)
            {
                result.Add(new ControlPointMarker(firstObjectFirstPoint, ActiveColor, ActiveControlPointSize));
                result.Add(new ControlPointMarker(firsObjectLastPoint, InactiveColor, InactiveControlPointSize));
            }
            else
            {
                result.Add(new ControlPointMarker(firstObjectFirstPoint,InactiveColor, InactiveControlPointSize));
                result.Add(new ControlPointMarker(firsObjectLastPoint, ActiveColor, ActiveControlPointSize));
            }
        }
        else
        {
            result.Add(new ControlPointMarker(firstObjectFirstPoint, Color.Red, InactiveControlPointSize));
            result.Add(new ControlPointMarker(firsObjectLastPoint, Color.Red, InactiveControlPointSize));
        }

        if (_secondSelectedObject != null)
        {
            var secondControlPoints = _secondSelectedObject.GetControlPoints().ToList(); // Преобразуем в список для получения длины

            var secondObjectFirstPoint = secondControlPoints[0];

            var secondObjectLastPoint = secondControlPoints[secondControlPoints.Count - 1];

            result.Add(new ControlPointMarker(secondObjectFirstPoint, InactiveColor, InactiveControlPointSize));
            result.Add(new ControlPointMarker(secondObjectLastPoint, InactiveColor, InactiveControlPointSize));
        } 

        return result;
    }
}
