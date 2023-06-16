using Microsoft.Maui.Devices.Sensors;
using System.Diagnostics;

namespace Multi_Platform_Compass;

public partial class CompassPage : ContentPage
{
    bool pointerActive = false;

	public CompassPage()
	{
        InitializeComponent();
        ToggleCompass();
    }

    private void OnPointerEnter(object sender, PointerEventArgs e)
    {
        pointerActive = true;   
        Point? relativeToPosition = RelativePositionFromCenter(needleContentView, e);
        needleImage.Rotation = CountRotation(relativeToPosition);
    }

    private void OnPointerMoved(object sender, PointerEventArgs e)
    {
        Point? relativeToPosition = RelativePositionFromCenter(needleContentView, e);
        needleImage.Rotation = CountRotation(relativeToPosition);
    }

    private void OnPointerExit(object sender, PointerEventArgs e)
    {
        pointerActive = false;
        needleImage.Rotation = 0;
    }

    private static Point? RelativePositionFromCenter(View layout, PointerEventArgs e)
    {
        Point? layoutPos = e.GetPosition(layout);
        Point? point = new Point?(new Point(layoutPos.Value.X - layout.Width/2, layoutPos.Value.Y - layout.Height/2));
        return point;
    }

    private static double CountRotation(Point? relativeToPosition)
    {
        double x = relativeToPosition.Value.X;
        double y = relativeToPosition.Value.Y;
        double degrees = 90 + (180 / Math.PI) * Math.Atan2(y,x);

        Debug.WriteLine(relativeToPosition.Value.ToString() + " " + degrees.ToString());
        return degrees;
    }

    private void ToggleCompass()
    {
        if (Compass.Default.IsSupported)
        {
            if (!Compass.Default.IsMonitoring)
            {
                // Turn on compass
                Compass.Default.ReadingChanged += CompassReadingChanged;
                Compass.Default.Start(SensorSpeed.UI, applyLowPassFilter: true);
            }
            else
            {
                // Turn off compass
                Compass.Default.Stop();
                Compass.Default.ReadingChanged -= CompassReadingChanged;
            }
        }
    }

    private void CompassReadingChanged(object sender, CompassChangedEventArgs e)
    {
        if (!pointerActive)
        {
            needleImage.Rotation = 0 - e.Reading.HeadingMagneticNorth;
        }

        // Update UI Label with compass state
        CompassLabel.TextColor = Colors.Green;
        CompassLabel.Text = $"Compass: {e.Reading}";
    }
}

