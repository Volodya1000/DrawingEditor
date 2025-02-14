namespace DrawingEditor.Core.History;

// Класс "Хранитель" (Caretaker), управляющий снимками состояний
// Класс "Хранитель" (Caretaker)
internal class DrawingHistory
{
    private Stack<DrawingState> undoHistory = new Stack<DrawingState>();
    private Stack<DrawingState> redoHistory = new Stack<DrawingState>();

    // Свойства для проверки возможности отката/повтора
    public bool CanUndo => undoHistory.Count > 0;
    public bool CanRedo => redoHistory.Count > 0;

    // Сохраняем состояние перед изменением и очищаем redo-историю
    public void SaveState(DrawingState state)
    {
        undoHistory.Push(state);
        redoHistory.Clear();
    }

    // Метод отмены: если возможно, возвращает предыдущее состояние,
    // при этом текущее состояние сохраняется в redo-стеке
    public DrawingState? Undo(DrawingState currentState)
    {
        if (CanUndo)
        {
            var state = undoHistory.Pop();
            redoHistory.Push(currentState);
            return state;
        }
        return null;
    }

    // Метод повтора: если возможно, возвращает следующее состояние,
    // при этом текущее состояние сохраняется в undo-стеке
    public DrawingState? Redo(DrawingState currentState)
    {
        if (CanRedo)
        {
            var state = redoHistory.Pop();
            undoHistory.Push(currentState);
            return state;
        }
        return null;
    }
}