using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

internal class ConnectObjectsState : IEditorState
{
    private readonly IDrawingGraphicObject firstSelectedObject;

    private IDrawingGraphicObject? secondSelectedObject;

    bool? firstPointOfFirstObject;
    bool? secondPointOfFirstObject;

    public ConnectObjectsState(IDrawingGraphicObject selectedObject)
    {
        this.firstSelectedObject = selectedObject;
    }

    public void HandleMouseMove(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {

    }

    public void HandlePoint(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {
        if (firstPointOfFirstObject == null)
        {
                var controlPoints = firstSelectedObject.GetControlPoints().ToList();
                for (int i = 0; i < controlPoints.Count; i++)
                {
                    if (editor.Distance(point, controlPoints[i]) <= editor.controlPointClickRadius)
                    {
                        if (i == 0)//первая точка объекта
                            firstPointOfFirstObject = true;
                        else if (i == controlPoints.Count - 1)//Последняя точка объекта
                            firstPointOfFirstObject = false;
                        return;
                    }
                }
                return; // пользователь нажал куда то не туда и никак на это не реагируем
        }
        else if(secondSelectedObject == null)
        {
            // Проверяем клик на другой объект
            var newSelected = editor.TrySelectObject(point);
            if (newSelected != null)
            {
                if(newSelected != firstSelectedObject)
                    secondSelectedObject = newSelected;
                return;
            }
        }
        else if(secondPointOfFirstObject==null)
        {
            var controlPoints = secondSelectedObject.GetControlPoints().ToList();
            for (int i = 0; i < controlPoints.Count; i++)
            {
                if (editor.Distance(point, controlPoints[i]) <= editor.controlPointClickRadius)
                {
                    if (i == 0)//первая точка объекта
                        secondPointOfFirstObject = true;
                    else if (i == controlPoints.Count - 1)//Последняя точка объекта
                        secondPointOfFirstObject = false;
                    else return; // выбрана не крайняя точка второго объекта

                    // объединяем объекты
                    ((IConnectable)firstSelectedObject).UpdatePoints(GetConcatedPoints());

                    //удаляем второй объект так как его точки переходят в первый объекты
                    editor.RemoveObject(secondSelectedObject);

                    editor.SetEditorState(new IdleState());
                    return;
                }
            }
        }
    }

    private IEnumerable<Point> GetConcatedPoints()
    {
        var first = firstSelectedObject.GetControlPoints();
        var second = secondSelectedObject.GetControlPoints();

        if (firstPointOfFirstObject == true)
            first = first.Reverse();

        if (secondPointOfFirstObject == false)
            second = second.Reverse();

        return first.Concat(second);
    }


    public IEnumerable<IDrawingGraphicObject> GetAdditionalRenderingObjects()
    {
        List<IDrawingGraphicObject> result = new List<IDrawingGraphicObject>();

        // Обрабатываем firstSelectedObject
        var firstControlPoints = firstSelectedObject.GetControlPoints().ToList(); // Преобразуем в список для получения длины
        for (int i = 0; i < firstControlPoints.Count; i++)
        {
            var cp = firstControlPoints[i];
            if (firstPointOfFirstObject.HasValue && 
                ((firstPointOfFirstObject.Value && i == 0) || 
                (!firstPointOfFirstObject.Value && i == firstControlPoints.Count - 1)))
                result.Add(new ControlPointMarker(cp, Color.Green, 4.0f));
            else
                result.Add(new ControlPointMarker(cp, Color.Red, 3.0f));
        }

        if (secondSelectedObject != null)
            foreach (var cp in secondSelectedObject.GetControlPoints())
                result.Add(new ControlPointMarker(cp, Color.Red, 3.0f));

        return result;
    }
}
