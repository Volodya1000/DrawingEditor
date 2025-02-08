using DrawingEditor.Core;

namespace DrawingEditor.WinForms;

public partial class MainForm : Form
{
    private readonly PanelController panelController;
    private BufferedPanel bufferedPanel;
    private ToolMenuStrip toolMenuStrip;
    private Label selectedToolLabel;
    private CheckBox gridCheckBox;
    private ToolTreeView toolTreeView;


    private GraphicsEditorFacade _graphicsEditorFacade;



    public MainForm()
    {
        InitializeComponent();

        // Создаем ToolMenuStrip и назначаем его как главное меню формы
        toolMenuStrip = new ToolMenuStrip();
        toolMenuStrip.ItemSelected += (sender, item) => UpdateLabelFromMenuSelection(item);
        MainMenuStrip = toolMenuStrip;
        Controls.Add(toolMenuStrip);

        // Панель инструментов с FlowLayoutPanel
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

        // Добавление Label
        selectedToolLabel = new Label
        {
            AutoSize = false,
            Width = toolPanel.Width - 10,
            Height = 60,
            Font = new Font(Font.FontFamily, 10),
            Text = "Инструмент: Нет",
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true 
        };

        toolPanel.Controls.Add(selectedToolLabel);

        // Чекбокс для сетки
        gridCheckBox = new CheckBox
        {
            Text = "Сетка",
            Width = toolPanel.Width - 10
        };
        gridCheckBox.CheckedChanged += gridCheckBox_CheckedChanged;
        toolPanel.Controls.Add(gridCheckBox);


        toolTreeView = new ToolTreeView()
        {
            Width = toolPanel.Width - 10,
            Height = 700,
            BorderStyle = BorderStyle.FixedSingle
        }; 
        toolTreeView.ExpandAll(); // Открыть все узлы
        toolTreeView.Height = toolTreeView.CalculateTreeViewHeight() + 4;
        toolTreeView.AfterSelect += (sender, e) => UpdateLabelFromTreeSelection(e.Node);
        toolPanel.Controls.Add(toolTreeView);

        // Добавляем панель инструментов на форму
        Controls.Add(toolPanel);

        // Панель для рисования
        bufferedPanel = new BufferedPanel
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.Fixed3D
        };
        Controls.Add(bufferedPanel);

        panelController = new PanelController(bufferedPanel, 100, 100, 7);

        // Убедитесь, что элементы не перекрываются
        toolPanel.BringToFront();
    }


    private void UpdateLabelFromTreeSelection(TreeNode selectedNode)
    {
        if (selectedNode == null)
            return;

        string path = GetNodePath(selectedNode);
        selectedToolLabel.Text = $"Инструмент: {path}";
    }

    private string GetNodePath(TreeNode node)
    {
        string path = node.Text;
        TreeNode parent = node.Parent;
        while (parent != null)
        {
            path = $"{parent.Text} > {path}";
            parent = parent.Parent;
        }
        return path;
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
        selectedToolLabel.Text = $"Инструмент: {menuPath}";
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