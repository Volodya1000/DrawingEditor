using DrawingEditor.Core;
using DrawingEditor.WinForms.ToolBarElements;
using System.Windows.Forms;

namespace DrawingEditor.WinForms;

public partial class MainForm : Form
{
    private readonly PanelController panelController;
    private BufferedPanel bufferedPanel;
    private Toolbar verticalToolbar;
    private FlowLayoutPanel horizontalToolPanel;

    public MainForm()
    {
        InitializeComponent();
        InitializeDrawingPanel();
        InitializeUI();
        panelController = new PanelController(bufferedPanel, 500, 500, 7);
        // Включаем предварительное прослушивание клавиш формы
        KeyPreview = true;
    }

    private void InitializeUI()
    {
        this.Text = "Графический редактор";
        CreateHorizontalToolPanel();
        CreateVerticalToolbar();
    }

    private void InitializeDrawingPanel()
    {
        bufferedPanel = new BufferedPanel
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.Fixed3D
        };
        Controls.Add(bufferedPanel);
    }

    private void CreateHorizontalToolPanel()
    {
        HorizontalToolPanelFactory horizontalToolPanelFactory = new HorizontalToolPanelFactory(bufferedPanel);
        horizontalToolPanel = horizontalToolPanelFactory.CreateToolPanel();
        Controls.Add(horizontalToolPanel);
    }

    private void CreateVerticalToolbar()
    {
        ToolbarFactory toolbarFactory = new ToolbarFactory();
        verticalToolbar = toolbarFactory.CreateToolbar();
        this.Controls.Add(verticalToolbar);
    }

}
