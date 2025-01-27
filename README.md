Your image shows a misbehaving custom vertical scrollbar `AbsoluteLayout` and you asked:

>How can I fix the problem here?

The main "struggle" seems to be with taking `Slider` and turning it into a vertical slider that still behaves intuitively. So, you could experiment with making a basic vertical slider _without_ the view customization and start with just that one piece before making the custom vasrsion..
___

**Vertical Slider**
~~~xaml
<Grid 
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="custom_slider_draw.Controls.VerticalSlider"
    WidthRequest="30"
    HeightRequest="300">
    <Slider
        x:Name="slider"
        HeightRequest="30" 
        WidthRequest="300"
        Rotation="270"
        TranslationX="-1"/>
</Grid>
~~~

___

**Forward Bindable Properties and Changes of Size-Margin**

Also, deal with the transposed X-Y cause by the rotation.

~~~csharp

public partial class VerticalSlider : Grid
{
    public VerticalSlider()
    {
        InitializeComponent();
        PropertyChanged += (sender, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(Margin):
                    OnContainerSizeChanged(this, EventArgs.Empty);
                    break;
            }
        };
        SizeChanged += OnContainerSizeChanged;

        slider.Focused += (sender, e) =>
        {
            if (sender is Slider slider)
            {
                Dispatcher.Dispatch(() => slider.Unfocus());
            }
        };
    }

    private void OnContainerSizeChanged(object? sender, EventArgs e)
    {
        var width = Width == -1
            ? WidthRequest
            : Width;
        var height = Height == -1
            ? HeightRequest
            : Height;
        slider.WidthRequest = height - Margin.VerticalThickness;
        slider.HeightRequest = width - Margin.HorizontalThickness;
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
~~~

___

**Custom Vertical Slider**

Once the basic slider is behaving, you can more easily go about a customized version in a more standard way, using a `Grid` instead of an `Absolute` layout.

~~~xaml
<Grid 
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:controls="clr-namespace:custom_slider_draw.Controls"   
    x:Class="custom_slider_draw.Controls.CustomVerticalSlider" 
    VerticalOptions="Center"
    HorizontalOptions="Center"
    ColumnDefinitions="50"
    RowDefinitions="30,*,30"
    HeightRequest="300">

    <!-- Background Frame -->
    <Frame
        Grid.RowSpan="3"
        CornerRadius="10"
        BackgroundColor="Orange"
        Opacity="0.4"
        HasShadow="False" 
        Margin="0,10"></Frame>

    <!-- Bottom cap (drawn first) -->
    <Border
        x:Name="BottomCap"
        Grid.Row="2"
        BackgroundColor="Orange"
        Stroke="Transparent"
        StrokeThickness="0"
        Margin="1">
        <Border.StrokeShape>
            <RoundRectangle CornerRadius="0,0,10,10" />
        </Border.StrokeShape>
    </Border>

    <!-- Top cap -->
    <Border
        x:Name="TopCap"
        BackgroundColor="Orange"
        Stroke="Transparent"
        StrokeThickness="0"
        Margin="4,1,4,0">
        <Border.StrokeShape>
            <RoundRectangle CornerRadius="10,10,0,0" />
        </Border.StrokeShape>

        <!-- Handle -->
        <BoxView
            x:Name="HandleLine"
            HeightRequest="4"
            WidthRequest="30"
            VerticalOptions="Center"
            HorizontalOptions="Center"
            BackgroundColor="White" />
    </Border>

    <controls:VerticalSlider
        Grid.RowSpan="3"
        VerticalOptions="Fill"
        HorizontalOptions="Fill"
        ThumbColor="BlueViolet"
        MaximumTrackColor="Green"
        MinimumTrackColor="Blue">
        <controls:VerticalSlider.Margin>
            <OnPlatform x:TypeArguments="Thickness">
                <On Platform="Android" Value="0,15"/>
                <On Platform="WinUI" Value="0,20"/>
            </OnPlatform>
        </controls:VerticalSlider.Margin>
    </controls:VerticalSlider>
</Grid>
~~~

___

~~~csharp
public partial class CustomVerticalSlider : Grid
{
	public CustomVerticalSlider() => InitializeComponent();
}
~~~

___

**Try the CustomVerticalScrollbar on the MainPage**

Now use the `CustomVerticalSlider` on the main page to test (this might not be "exactly" how you want it to look, but I hope it's a good start). 

~~~xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="custom_slider_draw.MainPage"
             xmlns:controls="clr-namespace:custom_slider_draw.Controls">
    <Grid>
        <controls:CustomVerticalSlider />
    </Grid>
</ContentPage>
~~~

[![platform screenshots][1]][1]


  [1]: https://i.sstatic.net/AykKYG8J.png