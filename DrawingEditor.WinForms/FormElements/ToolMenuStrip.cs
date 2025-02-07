namespace DrawingEditor.WinForms;

using System;
using System.Windows.Forms;

public class ToolMenuStrip : MenuStrip
{
    public event EventHandler<ToolStripMenuItem> ItemSelected;

    public ToolMenuStrip()
    {
        InitializeMenu();
    }

    private void InitializeMenu()
    {
        // Построение отрезков
        ToolStripMenuItem lineSegmentsMenu = new ToolStripMenuItem("Отрезки");
        AddMenuItem(lineSegmentsMenu, "ЦДА", OnDDAAlgorithm);
        AddMenuItem(lineSegmentsMenu, "Брезенхема", OnBresenhamAlgorithm);
        AddMenuItem(lineSegmentsMenu, "Ву", OnWuAlgorithm);
        this.Items.Add(lineSegmentsMenu);

        // Построение линий второго порядка
        ToolStripMenuItem secondOrderLinesMenu = new ToolStripMenuItem("Линии 2-го порядка");
        AddMenuItem(secondOrderLinesMenu, "Окружность", OnCircle);
        AddMenuItem(secondOrderLinesMenu, "Эллипс", OnEllipse);
        AddMenuItem(secondOrderLinesMenu, "Гипербола", OnHyperbola);
        AddMenuItem(secondOrderLinesMenu, "Парабола", OnParabola);
        this.Items.Add(secondOrderLinesMenu);

        // Параметрические кривые
        ToolStripMenuItem parametricCurvesMenu = new ToolStripMenuItem("Параметрические кривые");
        AddMenuItem(parametricCurvesMenu, "Эрмита", OnHermiteCurve);
        AddMenuItem(parametricCurvesMenu, "Безье", OnBezierCurve);
        AddMenuItem(parametricCurvesMenu, "B-сплайн", OnBSplineCurve);
        this.Items.Add(parametricCurvesMenu);

        // Геометрические преобразования
        ToolStripMenuItem geometricTransformationsMenu = new ToolStripMenuItem("Геом. преобразования");
        AddMenuItem(geometricTransformationsMenu, "Перемещение", OnTranslation);
        AddMenuItem(geometricTransformationsMenu, "Поворот", OnRotation);
        AddMenuItem(geometricTransformationsMenu, "Масштабирование", OnScaling);
        this.Items.Add(geometricTransformationsMenu);

        // Построение полигонов и выпуклых оболочек
        AddMenuItem(this.Items, "Полигоны", OnPolygonsAndConvexHulls);

        // Заполнение полигонов
        AddMenuItem(this.Items, "Заполнение", OnPolygonFilling);
    }

    private void AddMenuItem(ToolStripMenuItem parentItem, string text, EventHandler handler)
    {
        var item = new ToolStripMenuItem(text);
        item.Click += (sender, e) =>
        {
            handler?.Invoke(sender, e);
            ItemSelected?.Invoke(this, item);
        };
        parentItem.DropDownItems.Add(item);
    }

    private void AddMenuItem(ToolStripItemCollection collection, string text, EventHandler handler)
    {
        var item = new ToolStripMenuItem(text);
        item.Click += (sender, e) =>
        {
            handler?.Invoke(sender, e);
            ItemSelected?.Invoke(this, item);
        };
        collection.Add(item);
    }

    // Отдельные функции для каждого пункта меню
    private void OnDDAAlgorithm(object sender, EventArgs e)
    {
        // Логика для алгоритма ЦДА
    }

    private void OnBresenhamAlgorithm(object sender, EventArgs e)
    {
        // Логика для алгоритма Брезенхема
    }

    private void OnWuAlgorithm(object sender, EventArgs e)
    {
        // Логика для алгоритма Ву
    }

    private void OnCircle(object sender, EventArgs e)
    {
        // Логика для построения окружности
    }

    private void OnEllipse(object sender, EventArgs e)
    {
        // Логика для построения эллипса
    }

    private void OnHyperbola(object sender, EventArgs e)
    {
        // Логика для построения гиперболы
    }

    private void OnParabola(object sender, EventArgs e)
    {
        // Логика для построения параболы
    }

    private void OnHermiteCurve(object sender, EventArgs e)
    {
        // Логика для кривой Эрмита
    }

    private void OnBezierCurve(object sender, EventArgs e)
    {
        // Логика для кривой Безье
    }

    private void OnBSplineCurve(object sender, EventArgs e)
    {
        // Логика для B-сплайна
    }

    private void OnTranslation(object sender, EventArgs e)
    {
        // Логика для перемещения
    }

    private void OnRotation(object sender, EventArgs e)
    {
        // Логика для поворота
    }

    private void OnScaling(object sender, EventArgs e)
    {
        // Логика для масштабирования
    }

    private void OnPolygonsAndConvexHulls(object sender, EventArgs e)
    {
        // Логика для полигонов и выпуклых оболочек
    }

    private void OnPolygonFilling(object sender, EventArgs e)
    {
        // Логика для заполнения полигонов
    }
}

