namespace DrawingEditor.WinForms.ToolBarElements;

internal class Toolbar : FlowLayoutPanel
{
    private ISelectableButton selectedButton;

    public Toolbar()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Dock = DockStyle.Left;
        this.Width = 100;
        this.BackColor = Color.Gray;
        this.FlowDirection = FlowDirection.TopDown;
        this.WrapContents = false;
    }

    public void AddToolButton(string toolName, Color backColor, Action onClick)
    {
        var button = new ToolButton(toolName, backColor);
        button.Click += (sender, e) =>
        {
            SelectButton(button);
            onClick();
        };
        this.Controls.Add(button);
    }

    public void AddToolGroupButton(string toolName, Color backColor, Dictionary<string, Action> subTools)
    {
        var button = new ToolGroupButton(toolName, backColor, subTools);
        button.SubToolSelected += (sender, subTool) => SelectButton(button);
        this.Controls.Add(button);
    }

    private void SelectButton(ISelectableButton button)
    {
        if (selectedButton != null)
        {
            selectedButton.Deselect();
        }
        selectedButton = button;
        selectedButton.Select();
    }
}

