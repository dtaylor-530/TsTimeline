namespace Demos
{
    /// <summary>
    /// Interaction logic for ProgressUserControl.xaml
    /// </summary>
    public partial class ProgressUserControl : UserControl
    {
        public static readonly DependencyProperty ValueConverterProperty =
    DependencyProperty.Register(nameof(ValueConverter), typeof(IValueConverter), typeof(ProgressUserControl), new PropertyMetadata());


        public ProgressUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                if (this.DataContext is ViewModel { Key : Keys.Progress } progressViewModel)
                {
                    this.Value = ValueConverter.Convert(progressViewModel.X, typeof(string), default, default).ToString();
                    progressViewModel.PropertyChanged += (s, e) =>
                    {
                        this.Value = ValueConverter.Convert(progressViewModel.X, typeof(string), default, default).ToString();
                    };
                }
            };
        }

        public IValueConverter ValueConverter
        {
            get { return (IValueConverter)GetValue(ValueConverterProperty); }
            set { SetValue(ValueConverterProperty, value); }
        }



        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(string), typeof(ProgressUserControl), new PropertyMetadata());


    }
}
