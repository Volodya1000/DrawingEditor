using DrawingEditor.Core;

namespace DrawingEditor.WinForms;

public partial class MainForm : Form
{
    private readonly PanelController panelController;
    private BufferedPanel bufferedPanel;
    private ToolMenuStrip toolMenuStrip;
    private Label selectedToolLabel;
    private CheckBox gridCheckBox;

    private GraphicsEditorFacade _graphicsEditorFacade;

    public MainForm()
    {
        InitializeComponent();

        // ������� ToolMenuStrip � ��������� ��� ��� ������� ���� �����
        toolMenuStrip = new ToolMenuStrip();
        toolMenuStrip.ItemSelected += (sender, item) => UpdateLabelFromMenuSelection(item);
        MainMenuStrip = toolMenuStrip;
        Controls.Add(toolMenuStrip);

        // ������ ������������ � FlowLayoutPanel
        var toolPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Left,
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Width = 300, 
            Padding = new Padding(5),
            BorderStyle = BorderStyle.Fixed3D
        };

        // ���������� Label
        selectedToolLabel = new Label
        {
            AutoSize = false,
            Width = toolPanel.Width - 10,
            Height = 60, // ��������� ������
            Font = new Font(Font.FontFamily, 10), // �������� �����
            Text = "����������: ���",
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true // ���������� ��� ������� �������
        };

        toolPanel.Controls.Add(selectedToolLabel);

        // ������� ��� �����
        gridCheckBox = new CheckBox
        {
            Text = "�����",
            Width = toolPanel.Width - 10
        };
        gridCheckBox.CheckedChanged += gridCheckBox_CheckedChanged;
        toolPanel.Controls.Add(gridCheckBox);

        // ��������� ������ ������������ �� �����
        Controls.Add(toolPanel);

        // ������ ��� ���������
        bufferedPanel = new BufferedPanel
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.Fixed3D
        };
        Controls.Add(bufferedPanel);

        panelController = new PanelController(bufferedPanel, 100, 100, 7);

        // ���������, ��� �������� �� �������������
        toolPanel.BringToFront();
    }

    private void gridCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        panelController.GridEnable = !panelController.GridEnable;
        bufferedPanel.Invalidate();
    }

    private void UpdateLabelFromMenuSelection(ToolStripMenuItem selectedItem)
    {
        if (selectedItem == null)
            return;

        string menuPath = GetMenuPath(selectedItem);
        selectedToolLabel.Text = $"����������: {menuPath}";
    }

    private string GetMenuPath(ToolStripMenuItem item)
    {
        string path = item.Text;
        ToolStripItem parent = item.OwnerItem;
        while (parent != null && parent is ToolStripMenuItem)
        {
            path = $"{parent.Text} > \n{path}";
            parent = parent.OwnerItem;
        }
        return path;
    }
}