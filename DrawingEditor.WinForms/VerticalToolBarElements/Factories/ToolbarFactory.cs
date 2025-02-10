namespace DrawingEditor.WinForms.ToolBarElements;

internal class ToolbarFactory
{
    public Toolbar CreateToolbar()
    {
        Toolbar toolbar = new Toolbar();
        AddTools(toolbar);
        return toolbar;
    }

    private void AddTools(Toolbar toolbar)
    {
        //toolbar.AddToolButton("Pen", Color.LightBlue, () => MessageBox.Show("Pen selected"));
        //toolbar.AddToolButton("Brush", Color.LightGreen, () => MessageBox.Show("Brush selected"));

        LinesActionsFactory linesFactory = new LinesActionsFactory();
        linesFactory.AddLinesSubMenu(toolbar);

        //toolbar.AddToolGroupButton("Shapes", Color.LightYellow,
        //    new Dictionary<string, Action>
        //    {
        //        { "Circle", () => MessageBox.Show("Circle selected") },
        //        { "Square", () => MessageBox.Show("Square selected") },
        //        { "Triangle", () => MessageBox.Show("Triangle selected") }
        //    });


    }
}