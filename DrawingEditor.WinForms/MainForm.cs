using DrawingEditor.Core;
using System.Windows.Forms;

namespace DrawingEditor.WinForms
{
    public partial class MainForm : Form
    {
        //���������� ����������� ����������
        private readonly PanelController panelController;
        private BufferedPanel bufferedPanel;
        private ToolMenuStrip toolMenuStrip;
        private Label selectedToolLabel;
        //-----
        
        private GraphicsEditorFacade _graphicsEditorFacade;

        public MainForm()
        {
            InitializeComponent();

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


            //������
            bufferedPanel = new BufferedPanel 
            {   Dock = DockStyle.Fill,
                Top = selectedToolLabel.Height,
                Left = 0,
                Height = (int)(this.Height * 0.9),
                Width = this.Width,
                BorderStyle = BorderStyle.Fixed3D
            };
            Controls.Add(bufferedPanel);

            selectedToolLabel.BringToFront();

            panelController = new PanelController(bufferedPanel, 1000, 1000, 7);
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
