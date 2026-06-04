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
        private bool _dirty;

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



        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.Register(
                nameof(Viewport),
                typeof(Viewport),
                typeof(ClipBase),
                new PropertyMetadata(null, OnViewportChanged));

        public Viewport? Viewport
        {
            get => (Viewport?)GetValue(ViewportProperty);
            set => SetValue(ViewportProperty, value);
        }

        private static void OnViewportChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var self = (ClipBase)d;

            if (e.OldValue is Viewport old)
                old.PropertyChanged -= self.OnViewportPropertyChanged;

            if (e.NewValue is Viewport @new)
                @new.PropertyChanged += self.OnViewportPropertyChanged;

            self.UpdateWidth();
        }

        private void UpdateWidth()
        {
            if (Viewport == null) return;
            Width = Viewport.ViewportWidth * Viewport.ZoomX;
            UpdateThumb();
            UpdateThumbs();
            //InvalidateVisual();
        }

        private void OnViewportPropertyChanged(
    object? sender,
    System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Only the properties that affect the axis layout need a rebuild.
            // CursorPosition changes are frequent and don't affect tick layout.
            switch (e.PropertyName)
            {
                //case nameof(Viewport.OffsetX):
                case nameof(Viewport.ZoomX):
                case nameof(Viewport.ViewportWidth):
                case nameof(Viewport.Scale):
                //case nameof(Viewport.WorldStart):
                //case nameof(Viewport.WorldEnd):
                    MarkDirty();
                    break;
            }
        }

        // =====================================================
        // Invalidation
        // =====================================================

        private static void OnInvalidate(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e) =>
            ((ClipBase)d).MarkDirty();

        private void MarkDirty()
        {
            _dirty = true;
            UpdateWidth();
        }


        public bool IsReadOnly
        {
            get => (bool) GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
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