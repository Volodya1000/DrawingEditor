using System.Drawing;

namespace DrawingEditor.Core;

public interface IInputHandler
{
    void HandlePoint(Point point);

    void HandleMouseMove(Point point);
}
