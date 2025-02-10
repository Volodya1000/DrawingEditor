namespace DrawingEditor.WinForms.ToolBarElements;

internal class ToolGroupButton : ToolButton
{
    private SubToolPanel subToolPanel;
    private ISelectableButton selectedSubToolButton;
    public event EventHandler<ISelectableButton> SubToolSelected;

    public ToolGroupButton(string toolName, Color backColor, Dictionary<string, Action> subTools)
        : base(toolName, backColor)
    {
        subToolPanel = new SubToolPanel(this, subTools);

        subToolPanel.SubToolSelected += (sender, subTool) =>
        {
            selectedSubToolButton = subTool;
            UpdateText((subTool as ToolButton).Text);
            SubToolSelected?.Invoke(this, this);
        };

        this.MouseEnter += OnMouseEnter;
        this.MouseLeave += OnMouseLeave;

        // Нажатие на ToolGroupButton вызывает действие выбранной SubToolButton
        this.Click += (sender, e) => ExecuteSelectedSubTool();
    }

    private void ExecuteSelectedSubTool()
    {
        if (selectedSubToolButton is ToolButton toolButton)
        {
            toolButton.PerformClick(); // Вызов действия выбранной кнопки
        }
    }

    private void OnMouseEnter(object sender, EventArgs e)
    {
        subToolPanel.Show(this);
    }

    private void OnMouseLeave(object sender, EventArgs e)
    {
        subToolPanel.StartCloseTimer();
    }

    public void UpdateText(string text)
    {
        this.Text = text;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        DrawPlusSign(pevent.Graphics, this.Width - 15, this.Height / 2, 6, Pens.Black);
    }

    private void DrawPlusSign(Graphics graphics, int x, int y, int size, Pen pen)
    {
        graphics.DrawLine(pen, x - size / 2, y, x + size / 2, y);
        graphics.DrawLine(pen, x, y - size / 2, x, y + size / 2);
    }
}
