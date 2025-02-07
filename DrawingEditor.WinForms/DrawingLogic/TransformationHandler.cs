namespace DrawingEditor.WinForms;

public class TransformationHandler
{
    public float ZoomFactor { get; private set; } = 1f;
    public PointF Translation { get; private set; } = PointF.Empty;

    public void Zoom(Point mouseLocation, float delta)
    {
        float oldZoomFactor = ZoomFactor;
        ZoomFactor *= delta > 0 ? 1.1f : 1f / 1.1f;

        float scaleChange = ZoomFactor / oldZoomFactor;
        Translation = new PointF(
            mouseLocation.X - scaleChange * (mouseLocation.X - Translation.X),
            mouseLocation.Y - scaleChange * (mouseLocation.Y - Translation.Y)
        );
    }
}