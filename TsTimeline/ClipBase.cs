using System.Windows;
using System.Windows.Controls;
using Renderers;

namespace TsTimeline
{


    public partial class ClipBase : TreeViewItem, ISelectable
    {
        protected Canvas _partCanvas;

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ClipBase();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ClipBase;
        }

        static ClipBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClipBase),
                new FrameworkPropertyMetadata(typeof(ClipBase)));
        }

        public static SelectorService SelectorService => SelectorService.Default;

        public static readonly DependencyProperty IsReadOnlyProperty =
            DepProp.Register<ClipBase, bool>(nameof(IsReadOnly));



        public IUpdater Updater
        {
            get { return (IUpdater)GetValue(UpdaterProperty); }
            set { SetValue(UpdaterProperty, value); }
        }

        public static readonly DependencyProperty UpdaterProperty =
            DependencyProperty.Register(nameof(Updater), typeof(IUpdater), typeof(ClipBase), new PropertyMetadata());



        public static readonly DependencyProperty ViewportXProperty =
            DependencyProperty.Register(
                nameof(ViewportX),
                typeof(Viewport),
                typeof(ClipBase),
                new PropertyMetadata(null, OnViewportXChanged));

        public static readonly DependencyProperty ViewportYProperty =
            DependencyProperty.Register(
                nameof(ViewportY),
                typeof(Viewport),
                typeof(ClipBase),
                new PropertyMetadata(null, OnViewportYChanged));

        public Viewport? ViewportX
        {
            get => (Viewport?)GetValue(ViewportXProperty);
            set => SetValue(ViewportXProperty, value);
        }
        public Viewport? ViewportY
        {
            get => (Viewport?)GetValue(ViewportYProperty);
            set => SetValue(ViewportYProperty, value);
        }

        private static void OnViewportXChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var self = (ClipBase)d;

            if (e.OldValue is Viewport old)
                old.PropertyChanged -= self.OnViewportPropertyChanged;

            if (e.NewValue is Viewport @new)
                @new.PropertyChanged += self.OnViewportPropertyChanged;

            self.updateX();
        }
        private static void OnViewportYChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var self = (ClipBase)d;

            if (e.OldValue is Viewport old)
                old.PropertyChanged -= self.OnViewportPropertyChanged;

            if (e.NewValue is Viewport @new)
                @new.PropertyChanged += self.OnViewportPropertyChanged;

            self.updateY();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            updateX();
            updateY();
        }

        private void updateX()
        {
            if (ViewportX == null) return;   
            if (Updater is not null)
                Updater.UpdateX(this);
            else
            {
                updateThumb();
                updatePointX();
                updateThumbs();
                updateBand();
            
            }
        }
        private void updateY()
        {
            if (ViewportY == null) return;   
            if (Updater is not null)
                Updater.UpdateY(this);
            else
            {
                updatePointY();
            }
        }

        private void OnViewportPropertyChanged(
    object? sender,
    System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Only the properties that affect the axis layout need a rebuild.
            // CursorPosition changes are frequent and don't affect tick layout.
            switch (e.PropertyName)
            {
                case nameof(Viewport.Zoom):
                case nameof(Viewport.ViewportLength):       
                case nameof(Viewport.Offset):         
                    //case nameof(Viewport.Start):
                    //case nameof(Viewport.End):
                    updateX();
                    break;
            }
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        private static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClipBase t)
                t.OnSelectedChanged();
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