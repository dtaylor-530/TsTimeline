using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TsTimeline;

namespace SandBox
{
    /// <summary>
    /// Interaction logic for ConfigurationUserControl.xaml
    /// </summary>
    public partial class ConfigurationUserControl : UserControl
    {
        public ConfigurationUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                PART_COMBOBOX.ItemsSource = Enum.GetValues<ChartType>();
            };       
        }
    }
}
