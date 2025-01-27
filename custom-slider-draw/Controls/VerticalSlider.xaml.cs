namespace custom_slider_draw.Controls;

public partial class VerticalSlider : Grid
{
    public VerticalSlider()
    {
        InitializeComponent();
        SizeChanged += (sender, e) =>
        {
            slider.WidthRequest = Height - Margin.VerticalThickness;
            slider.HeightRequest = Width - Margin.HorizontalThickness;
        };
        slider.Focused += (sender, e) =>
        {
            if (sender is Slider slider)
            {
                Dispatcher.Dispatch(() => slider.Unfocus());
            }
        };
    }
    public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(
                nameof(Value),
                typeof(double),
                typeof(VerticalSlider),
                propertyChanged: (bindable, oldValue, newValue) =>
                    ((VerticalSlider)bindable).slider.Value = (double)newValue);

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly BindableProperty MaximumProperty =
        BindableProperty.Create(
            nameof(Maximum),
            typeof(double),
            typeof(VerticalSlider),
            propertyChanged: (bindable, oldValue, newValue) =>
                ((VerticalSlider)bindable).slider.Maximum = (double)newValue);

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly BindableProperty MinimumProperty =
        BindableProperty.Create(
            nameof(Minimum),
            typeof(double),
            typeof(VerticalSlider),
            propertyChanged: (bindable, oldValue, newValue) =>
                ((VerticalSlider)bindable).slider.Minimum = (double)newValue);

    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly BindableProperty ThumbColorProperty =
        BindableProperty.Create(
            nameof(ThumbColor),
            typeof(Color),
            typeof(VerticalSlider),
            propertyChanged: (bindable, oldValue, newValue) =>
                ((VerticalSlider)bindable).slider.ThumbColor = (Color)newValue);

    public Color ThumbColor
    {
        get => (Color)GetValue(ThumbColorProperty);
        set => SetValue(ThumbColorProperty, value);
    }

    public static readonly BindableProperty MinimumTrackColorProperty =
        BindableProperty.Create(
            nameof(MinimumTrackColor),
            typeof(Color),
            typeof(VerticalSlider),
            propertyChanged: (bindable, oldValue, newValue) =>
                ((VerticalSlider)bindable).slider.MinimumTrackColor = (Color)newValue);

    public Color MinimumTrackColor
    {
        get => (Color)GetValue(MinimumTrackColorProperty);
        set => SetValue(MinimumTrackColorProperty, value);
    }

    public static readonly BindableProperty MaximumTrackColorProperty =
        BindableProperty.Create(
            nameof(MaximumTrackColor),
            typeof(Color),
            typeof(VerticalSlider),
            propertyChanged: (bindable, oldValue, newValue) =>
                ((VerticalSlider)bindable).slider.MaximumTrackColor = (Color)newValue);

    public Color MaximumTrackColor
    {
        get => (Color)GetValue(MaximumTrackColorProperty);
        set => SetValue(MaximumTrackColorProperty, value);
    }
}