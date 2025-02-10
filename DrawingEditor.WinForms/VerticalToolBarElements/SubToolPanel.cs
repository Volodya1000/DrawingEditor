namespace DrawingEditor.WinForms.ToolBarElements;

internal class SubToolPanel: Panel
{
    private System.Windows.Forms.Timer closePanelTimer;
    private ToolGroupButton parentButton;
    private ISelectableButton selectedSubToolButton;
    public event EventHandler<ISelectableButton> SubToolSelected;

    public SubToolPanel(ToolGroupButton parent, Dictionary<string, Action> subTools)
    {
        parentButton = parent;
        InitializeComponent();
        AddSubTools(subTools);
        SetupTimer();
    }

    private void InitializeComponent()
    {
        this.Size = new Size(110, 120);
        this.BackColor = Color.LightGray;
        this.Visible = false;
        this.MouseEnter += OnMouseEnter;
        this.MouseLeave += OnMouseLeave;
    }

    private void AddSubTools(Dictionary<string, Action> subTools)
    {
        int yOffset = 8;
        foreach (var subTool in subTools)
        {
            var subToolButton = new ToolButton(subTool.Key, Color.White)
            {
                Size = new Size(100, 30),
                Location = new Point(5, yOffset)
            };
            yOffset += 35;

            subToolButton.Click += (s, e) =>
            {
                SelectSubTool(subToolButton);
                subTool.Value();
                SubToolSelected?.Invoke(this, subToolButton);
            };
            this.Controls.Add(subToolButton);
        }

        this.Height = Math.Min(yOffset + 5, 200);
    }

    private void SetupTimer()
    {
        closePanelTimer = new System.Windows.Forms.Timer { Interval = 150 };
        closePanelTimer.Tick += (s, e) =>
        {
            if (!this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)) &&
                !parentButton.ClientRectangle.Contains(parentButton.PointToClient(Cursor.Position)))
            {
                this.Hide();
                closePanelTimer.Stop();
            }
        };
    }

    public void Show(ToolGroupButton button)
    {
        var toolbar = button.Parent as Toolbar;
        if (toolbar != null)
        {
            int topPosition = button.Top;
            Form parentForm = toolbar.FindForm();
            if (parentForm != null)
            {
                int maxY = parentForm.ClientSize.Height - this.Height;
                topPosition = Math.Min(topPosition, maxY);
            }
            this.Location = new Point(toolbar.Right, topPosition);
            toolbar.Parent.Controls.Add(this);
        }
        else
        {
            this.Location = new Point(button.Right, button.Top);
            button.Parent.Controls.Add(this);
        }
        this.Visible = true;
        this.BringToFront();
        closePanelTimer.Stop();
    }

    public void StartCloseTimer()
    {
        closePanelTimer.Start();
    }

    private void OnMouseEnter(object sender, EventArgs e)
    {
        closePanelTimer.Stop();
    }

    private void OnMouseLeave(object sender, EventArgs e)
    {
        closePanelTimer.Start();
    }

    private void SelectSubTool(ISelectableButton subTool)
    {
        if (selectedSubToolButton != null)
        {
            selectedSubToolButton.Deselect();
        }
        selectedSubToolButton = subTool;
        selectedSubToolButton.Select();
    }
}
