using System.Drawing;

namespace DrawingEditor.Core;

public interface ICreationState
{
    bool IsReadyToCreate();     // Проверяет, достаточно ли точек для создания объекта
    void AddPoint(Point point); // Добавляет точку
    List<Point> GetPoints();    // Возвращает накопленные точки
}

