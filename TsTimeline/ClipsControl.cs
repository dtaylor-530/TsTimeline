using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TsTimeline
{

  
    public class ClipsControl : TreeViewItem , ISelectable
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ClipBase();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ClipBase;
        }


        public SelectorService SelectorService => SelectorService.Default;
        
        public static readonly DependencyProperty LastMouseDownXProperty =
            DepProp.Register<ClipsControl, double>(nameof(LastMouseDownX));

        public double LastMouseDownX
        {
            get => (double) GetValue(LastMouseDownXProperty);
            set => SetValue(LastMouseDownXProperty, value);
        }

        private static void SelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ClipsControl c)
                c.OnSelectedChanged();
        }

        public ClipsControl()
        {
            PreviewMouseDown += (s, e) =>
            {
                //LastMouseDownX = e.GetPosition(this).X * (1.0 / TsTimeline);
            };
        }
        
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            OnMouseDownSelectedChanged();
            base.OnMouseDown(e);
            e.Handled = true;
        }

        private void OnSelectedChanged()
        {
            SelectorService.UpdateSelectedItems(this);
        }

        private void OnMouseDownSelectedChanged()
        {
            SelectorService.MouseDownSelectionChanged(this);
        }
    }
}