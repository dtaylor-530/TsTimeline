using System.Windows;
using System.Windows.Controls;
using Renderers;

namespace TsTimeline
{


    public partial class ClipBase : TreeViewItem, ISelectable
    {
        protected Canvas _partCanvas;
        //private bool _dirty;


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

        // Using a DependencyProperty as the backing store for Update.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdaterProperty =
            DependencyProperty.Register(nameof(Updater), typeof(IUpdater), typeof(ClipBase), new PropertyMetadata());



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

            self.Update();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Update();
        }

        private void Update()
        {
            if (Viewport == null) return;
            if (Updater is not null)
                Updater.Update(this);
            else
            {
                updateThumb();
                updateThumbs();
                updateBand();
                updatePoint();
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
                //case nameof(Viewport.OffsetX):
                case nameof(Viewport.ZoomX):
                case nameof(Viewport.ViewportWidth):
                case nameof(Viewport.ViewportHeight):
                case nameof(Viewport.ScaleX):         
                case nameof(Viewport.OffsetX):         
                    //case nameof(Viewport.WorldStart):
                    //case nameof(Viewport.WorldEnd):
                    Update();
                    break;
            }
        }

        // =====================================================
        // Invalidation
        // =====================================================


        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
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
            this.updateThumb();
            this.updateThumbs();
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