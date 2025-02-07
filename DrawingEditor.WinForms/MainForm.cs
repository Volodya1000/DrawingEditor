using DrawingEditor.Core;

namespace DrawingEditor.WinForms
{
    public partial class MainForm : Form
    {
        private readonly GraphicsEditorFacade _graphicsEditorFacade;
        private Panel _canvasPanel;

        private ToolMenuStrip toolMenuStrip;
        private Label selectedToolLabel;

        public MainForm()
        {
            InitializeComponent();

            this.ClientSize = new Size(800, 600);
            this.Text = "����������� ��������";

            // ���������� ������ ������������ 
            toolMenuStrip = new ToolMenuStrip();
            toolMenuStrip.ItemSelected += (sender, item) => UpdateLabelFromMenuSelection(item);
            this.MainMenuStrip = toolMenuStrip;
            this.Controls.Add(toolMenuStrip);

            // ������� � ����������� Label
            selectedToolLabel = new Label
            {
                Top = toolMenuStrip.Height,
                Left = 0,
                Width = this.Width,
                Height = (int)(this.Height * 0.1),
                Font = new Font(Font.FontFamily, 12),
                Text = "����������: ���"
            };

            this.Controls.Add(selectedToolLabel);

            // ������� ������ ��� ���������
            _canvasPanel = new Panel
            {
                Top = selectedToolLabel.Height,
                Left = 0,
                Height = (int)(this.Height * 0.9),
                Width = this.Width,
                BorderStyle = BorderStyle.Fixed3D
            };
            this.Controls.Add(_canvasPanel);

            selectedToolLabel.BringToFront();

            var canvasModel = new CanvasModel();
            _graphicsEditorFacade = new GraphicsEditorFacade();//canvasModel);

            // �������������� ���������� ������ ���������
            var canvasHandler = new CanvasPanelHandler(_canvasPanel, _graphicsEditorFacade);
        }

        private void UpdateLabelFromMenuSelection(ToolStripMenuItem selectedItem)
        {
            if (selectedItem == null)
                return;

            string menuPath = GetMenuPath(selectedItem);
            selectedToolLabel.Text = $"������ ����������: {menuPath}";
        }

        private string GetMenuPath(ToolStripMenuItem item)
        {
            string path = item.Text;
            ToolStripItem parent = item.OwnerItem;
            while (parent != null && parent is ToolStripMenuItem)
            {
                path = $"{parent.Text} > {path}";
                parent = parent.OwnerItem;
            }
            return path;
        }
    }
}



//using DrawingEditor.Core;

//namespace DrawingEditor.WinForms;

//public partial class MainForm : Form
//{

//    private readonly GraphicsEditorFacade _graphicsEditorFacade;
//    private Panel _canvasPanel;

//    private ToolMenuStrip toolMenuStrip;
//    private Label selectedToolLabel;

//    private Point? _startPoint = null;  

//    public MainForm()
//    {
//        InitializeComponent();

//        this.ClientSize = new Size(800, 600);
//        this.Text = "����������� ��������";
//        //���������� ������ ������������ 
//        toolMenuStrip = new ToolMenuStrip();
//        toolMenuStrip.ItemSelected += (sender, item) => UpdateLabelFromMenuSelection(item);
//        this.MainMenuStrip = toolMenuStrip;
//        this.Controls.Add(toolMenuStrip);


//        // ������� � ����������� Label
//        selectedToolLabel = new Label
//        {
//            Top = toolMenuStrip.Height,
//            Left = 0,
//            Width = this.Width,
//            Height = (int)(this.Height * 0.1),
//            Font = new Font(Font.FontFamily, 12),
//            Text = "����������: ���"
//        };

//        this.Controls.Add(selectedToolLabel);


//        // ������� ������ ��� ���������
//        _canvasPanel = new Panel
//        {
//            Top = selectedToolLabel.Height,
//            Left = 0,
//            Height = (int)(this.Height * 0.9),
//            Width = this.Width,
//            BorderStyle = BorderStyle.Fixed3D
//        };
//        this.Controls.Add(_canvasPanel);


//        selectedToolLabel.BringToFront();



//        var canvasModel = new CanvasModel();
//        _graphicsEditorFacade = new GraphicsEditorFacade(canvasModel);

//        // ����������� ����������� �������
//        _canvasPanel.Paint += CanvasPanel_Paint;
//        _canvasPanel.MouseClick += CanvasPanel_MouseClick;




//    }

//    private void CanvasPanel_Paint(object sender, PaintEventArgs e)
//    {
//        foreach (var point in _graphicsEditorFacade.GetPoints()) 
//            e.Graphics.FillRectangle(Brushes.Black, point.X, point.Y, 1, 1);
//    }

//    // ���������� ������ ���� ��� ��������� �����
//    private void CanvasPanel_MouseClick(object sender, MouseEventArgs e)
//    {
//        if (!_startPoint.HasValue)
//        {
//            // ���� ��������� ����� ��� �� �������, ��������� ������ �����
//            _startPoint = e.Location;
//        }
//        else// ���� ��������� ����� ��� �������, ������ �����
//        {

//            var endPoint = e.Location;

//            _graphicsEditorFacade.AddLine(_startPoint.Value, endPoint);

//            // �������������� ������, ����� ���������� �����
//            _canvasPanel.Invalidate();

//            // ���������� ��������� ����� ��� ��������� �����
//            _startPoint = null;
//        }
//    }

//    private void UpdateLabelFromMenuSelection(ToolStripMenuItem selectedItem)
//    {
//        if (selectedItem == null)
//            return;

//        string menuPath = GetMenuPath(selectedItem);
//        selectedToolLabel.Text = $"������ ����������: {menuPath}";
//    }


//    private string GetMenuPath(ToolStripMenuItem item)
//    {
//        string path = item.Text;
//        ToolStripItem parent = item.OwnerItem;
//        while (parent != null && parent is ToolStripMenuItem)
//        {
//            path = $"{parent.Text} > {path}";
//            parent = parent.OwnerItem;
//        }
//        return path;
//    }



//}

//public partial class MainForm : Form
//{
//    private DrawingHandler drawingHandler;

//    public MainForm()
//    {
//        InitializeComponent();

//        BufferedPanel panel = new BufferedPanel
//        {
//            Dock = DockStyle.Fill,
//            BackColor = Color.White
//        };

//        this.Controls.Add(panel);

//        // ������� ���������� ���������
//        drawingHandler = new DrawingHandler(panel);
//    }


//}