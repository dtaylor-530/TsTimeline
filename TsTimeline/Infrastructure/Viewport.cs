using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Sandbox2.Models;

namespace TsTimeline
{
    public sealed class Viewport : Notification
    {
        private double _offsetX;
        private double _zoomX = 1.0;
        private double _viewportWidth;
        private double _worldStart;
        private double _worldEnd = 1000;
        private double _cursorPosition;
        private int _minPixelSpacing = 5;
        private int _scale = 10;

        public double OffsetX
        {
            get => _offsetX;
            set
            {
                var clamped = ClampOffset(value);
                if (_offsetX == clamped) return;
                _offsetX = clamped;
                OnPropertyChanged();
                OnPropertyChanged(nameof(VisibleStart));
                OnPropertyChanged(nameof(VisibleEnd));
            }
        }

        public double ZoomX
        {
            get => _zoomX;
            set
            {
                value = Math.Max(0.01, value);
                if (Math.Abs(_zoomX - value) < 0.00001) return;
                _zoomX = value;
                // Re-clamp offset now that visible width has changed
                _offsetX = ClampOffset(_offsetX);
                OnPropertyChanged();
                OnPropertyChanged(nameof(VisibleWorldWidth));
                OnPropertyChanged(nameof(VisibleStart));
                OnPropertyChanged(nameof(VisibleEnd));
            }
        }

        public double ViewportWidth
        {
            get => _viewportWidth;
            set
            {
                if (_viewportWidth == value) return;
                _viewportWidth = value;
                // Re-clamp: a wider viewport may push OffsetX out of range
                _offsetX = ClampOffset(_offsetX);
                OnPropertyChanged();
                OnPropertyChanged(nameof(VisibleWorldWidth));
                OnPropertyChanged(nameof(VisibleStart));
                OnPropertyChanged(nameof(VisibleEnd));
            }
        }

        public double WorldStart
        {
            get => _worldStart;
            set
            {
                if (_worldStart == value) return;
                _worldStart = value;
                _offsetX = ClampOffset(_offsetX);
                OnPropertyChanged();
                OnPropertyChanged(nameof(WorldLength));
                OnPropertyChanged(nameof(VisibleStart));
                OnPropertyChanged(nameof(VisibleEnd));
            }
        }

        public double WorldEnd
        {
            get => _worldEnd;
            set
            {
                if (_worldEnd == value) return;
                _worldEnd = value;
                _offsetX = ClampOffset(_offsetX);
                OnPropertyChanged();
                OnPropertyChanged(nameof(WorldLength));
                OnPropertyChanged(nameof(VisibleEnd));
            }
        }

        public double CursorPosition
        {
            get => _cursorPosition;
            set
            {
                if (_cursorPosition == value) return;
                _cursorPosition = value;
                OnPropertyChanged();
            }
        }

        public int MinPixelSpacing
        {
            get => _minPixelSpacing;
            set
            {
                if (_minPixelSpacing == value) return;
                _minPixelSpacing = value;
                OnPropertyChanged();
            }
        }

        public int Scale
        {
            get => _scale;
            set
            {
                if (_scale == value) return;
                _scale = value;
                OnPropertyChanged();
            }
        }

        public double WorldLength => WorldEnd - WorldStart;
        public double VisibleWorldWidth => ViewportWidth / ZoomX;
        public double VisibleStart => OffsetX;
        public double VisibleEnd => OffsetX + VisibleWorldWidth;

        // --- coordinate transforms ---
          
        
        //public double DomainToScreen(double value)
        //        => value * PixelsPerUnit - Offset;

        public double WorldToScreen(double world) =>
            (world - OffsetX) * ZoomX * Scale;

        public double ScreenToWorld(double screen) =>
            OffsetX + screen / ZoomX;

        // --- navigation ---

        public void Pan(double deltaWorld) =>
            OffsetX += deltaWorld;

        public void CenterOn(double worldPosition) =>
            OffsetX = worldPosition - VisibleWorldWidth * 0.5;

        /// <summary>Zoom by <paramref name="factor"/> keeping the world position
        /// under <paramref name="screenX"/> stationary.</summary>
        public void ZoomAt(double factor, double screenX)
        {
            var worldAnchor = ScreenToWorld(screenX);
            ZoomX *= factor;
            // Shift offset so the anchor point stays under the cursor
            OffsetX += worldAnchor - ScreenToWorld(screenX);
        }

        // --- helpers ---

        private double ClampOffset(double offset)
        {
            // When the visible range is wider than the world, pin to WorldStart
            double maxOffset = Math.Max(WorldStart, WorldEnd - VisibleWorldWidth);
            return Math.Clamp(offset, WorldStart, maxOffset);
        }
    }
}