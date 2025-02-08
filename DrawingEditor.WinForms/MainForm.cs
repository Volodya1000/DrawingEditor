using DrawingEditor.Core;

namespace DrawingEditor.WinForms;

public partial class MainForm : Form
{
    //объявление графических эллементов
    private readonly PanelController panelController;
    private BufferedPanel bufferedPanel;
    private ToolMenuStrip toolMenuStrip;
    private Label selectedToolLabel;
    //private CheckBox gridCheckBox;
    //-----
    
    private GraphicsEditorFacade _graphicsEditorFacade;

    public MainForm()
    {
        InitializeComponent();

        // Добавление панели инструментов 
        toolMenuStrip = new ToolMenuStrip();
        toolMenuStrip.ItemSelected += (sender, item) => UpdateLabelFromMenuSelection(item);
        MainMenuStrip = toolMenuStrip;
        Controls.Add(toolMenuStrip);

        // Создаем и настраиваем Label
        selectedToolLabel = new Label
        {
            Top = toolMenuStrip.Height,
            Left = 0,
            Width = this.Width,
            Height = (int)(this.Height * 0.1),
            Font = new Font(Font.FontFamily, 12),
            Text = "Инструмент: Нет"
        };

        this.Controls.Add(selectedToolLabel);


        //Панель
        bufferedPanel = new BufferedPanel
        { Dock = DockStyle.Fill,
            Top = selectedToolLabel.Height,
            Left = 0,
            Height = (int)(this.Height * 0.9),
            Width = this.Width,
            BorderStyle = BorderStyle.Fixed3D
        };
        Controls.Add(bufferedPanel);

        selectedToolLabel.BringToFront();

        panelController = new PanelController(bufferedPanel, 100, 100, 7);



       
        gridCheckBox.Top = toolMenuStrip.Height;
        gridCheckBox.Left = this.Width- gridCheckBox.Width;
        gridCheckBox.Text = "Сетка";
        gridCheckBox.BringToFront();
        gridCheckBox.Checked = false;
        gridCheckBox.CheckedChanged += gridCheckBox_CheckedChanged;


    }

    private void gridCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        panelController.GridEnable=!panelController.GridEnable;
        bufferedPanel.Invalidate();
    }


    private void UpdateLabelFromMenuSelection(ToolStripMenuItem selectedItem)
    {
        if (selectedItem == null)
            return;

        string menuPath = GetMenuPath(selectedItem);
        selectedToolLabel.Text = $"Выбран инструмент: {menuPath}";
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
