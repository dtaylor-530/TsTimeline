using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
    
    public partial class ClipBase : TreeViewItem , ISelectable
    {
        protected Canvas _partCanvas;

        static ClipBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClipBase),
                new FrameworkPropertyMetadata(typeof(ClipBase)));
        }

        // 実装速度優先でstaticで扱う。将来的にはTimelineControlから注入する形にする
        // こうしないとUIで2か所以上でTimeLineControlが使いづらくなる。
        public static SelectorService SelectorService => SelectorService.Default;
                
        public static readonly DependencyProperty IsReadOnlyProperty =
            DepProp.Register<ClipBase, bool>(nameof(IsReadOnly));

        public static readonly DependencyProperty ScaleProperty =
            DepProp.Register<ClipBase, double>(nameof(Scale), 1, ValueChanged);
                
        public bool IsReadOnly
        {
            get => (bool) GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public double Scale
        {
            get => (double) GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }
        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClipBase t)
                t.OnValueChanged();
        }
        
        private static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClipBase t)
                t.OnSelectedChanged();
        }

        protected virtual void OnValueChanged()
        {
            this.UpdateThumb();
            this.UpdateThumbs();
        }

        protected virtual void OnSelectedChanged()
        {
            SelectorService.UpdateSelectedItems(this);
        }

        protected void OnMouseDownSelectedChanged()
        {
            SelectorService.MouseDownSelectionChanged(this);
        }
    }
}