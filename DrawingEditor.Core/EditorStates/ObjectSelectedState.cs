using System.Drawing;
using DrawingEditor.Core.Models.Interfaces;

namespace DrawingEditor.Core;

internal class ObjectSelectedState : IEditorState
{
    private readonly IDrawingGraphicObject selectedObject;

    public ObjectSelectedState(IDrawingGraphicObject selectedObject)
    {
        this.selectedObject = selectedObject;
    }

    public IDrawingGraphicObject SelectedObject => selectedObject;

    public void HandleMouseMove(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {
        editor.SetPreviewGraphicObject(null);
    }

    public void HandlePoint(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {
        // Проверяем клик на контрольные точки текущего объекта
        var controlPoints = selectedObject.GetControlPoints().ToList();
        for (int i = 0; i < controlPoints.Count; i++)
        {
            if (editor.Distance(point, controlPoints[i]) <= editor.controlPointClickRadius)
            {
                if (selectedObject is IEditableGraphicObject editable)
                {
                    editor.SetEditorState(new DragControlPointState(editable, i, editor.controlPointClickRadius));
                    return;
                }
            }
        }

        // Проверяем клик на другой объект
        var newSelected = editor.TrySelectObject(point);
        if (newSelected != null)
        {
            editor.SetEditorState(new ObjectSelectedState(newSelected));
            return;
        }

        // Клик вне объекта - возврат в Idle
        editor.SetEditorState(new IdleState());
    }

    public IEnumerable<IDrawingGraphicObject> GetAdditionalRenderingObjects()
    {
        foreach (var cp in selectedObject.GetControlPoints())
            yield return new ControlPointMarker(cp, Color.Red, 3.0f);
    }
}
