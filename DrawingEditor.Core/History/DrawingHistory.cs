namespace DrawingEditor.Core.History;

// Класс "Хранитель" (Caretaker), управляющий снимками состояний
internal class DrawingHistory
{
    private Stack<DrawingState> history = new Stack<DrawingState>();

    public void SaveState(DrawingState state)
    {
        history.Push(state);
    }

    // Восстановление состояния
    public DrawingState ?  Undo()
    {
        if (history.Count > 0)
        {
            return history.Pop();
        }
        return null; 
    }
}
