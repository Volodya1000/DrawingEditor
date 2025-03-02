using System.Drawing;

namespace DrawingEditor.Core.Models.Interfaces;

// Расширяем исходный интерфейс для поддержки редактирования опорных точек
public interface IEditableGraphicObject : IDrwaingGraphicObject
{
    // Метод для обновления позиции опорной точки с заданным индексом
    void UpdateControlPoint(int index, Point newPoint);
}