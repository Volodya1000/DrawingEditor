namespace DrawingEditor.WinForms;


// <<Singleton>>
internal class CurentDrawingSettings
{
    public static CurentDrawingSettings instance;

    private CurentDrawingSettings() { }
    public static CurentDrawingSettings GetInstance()
    {
        if(instance == null)
        {
            instance = new CurentDrawingSettings();
            instance.SelectedColor = Color.Black;
            instance.GridEnable=true;
        }
        return instance;
    }

    public bool GridEnable { get; set; }

    public Color SelectedColor { get; set; }



}
