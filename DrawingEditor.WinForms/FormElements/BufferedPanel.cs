namespace DrawingEditor.WinForms;

public class BufferedPanel : Panel
{
    public BufferedPanel()
    {
        DoubleBuffered = true; // Включаем двойную буферизацию для устранения мерцания.
        // Устанавливаем стили для оптимизации рисования и обновления:
        SetStyle(ControlStyles.AllPaintingInWmPaint | // Все рисование в WmPaint.
                 ControlStyles.OptimizedDoubleBuffer | // Оптимизированная двойная буферизация.
                 ControlStyles.ResizeRedraw | // Перерисовка при изменении размера.
                 ControlStyles.UserPaint, true); // Пользовательское управление рисованием.

        UpdateStyles(); // Применяем изменения стилей.
    }
}
