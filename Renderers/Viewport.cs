using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Sandbox;

namespace Renderers
{
    public sealed class Viewport : Notification
    {
        private double _offset;
        private double _zoom = 1.0;
        private double _viewportLength = 1000;
        private double _worldStart;
        private double _worldEnd = 1000;
        private double _cursorPosition;
        private int _minPixelSpacing = 5;
        private int _scale = 10;

        public double Offset
        {
            get => _offset;
            set
            {
                //var clamped = clampOffset(value);
                //if (_offsetX == clamped) return;
                if (_offset == value) return;               
                _offset = value;
                OnPropertyChanged();
            }
        }

        public double Zoom
        {
            get => _zoom;
            set
            {
                value = Math.Max(0.01, value);
                if (Math.Abs(_zoom - value) < 0.00001) return;
                _zoom = value;
                // Re-clamp offset now that visible width has changed
                _offset = clampOffset(_offset);
                OnPropertyChanged();
            }
        }

        public double ViewportLength
        {
            get => _viewportLength;
            set
            {
                if (_viewportLength == value) return;
                _viewportLength = value;
                // Re-clamp: a wider viewport may push OffsetX out of range
                _offset = clampOffset(_offset);
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


        public double WorldStart
        {
            get => _worldStart;
            set
            {
                if (_worldStart == value) return;
                _worldStart = value;
                _offset = clampOffset(_offset);
                OnPropertyChanged();
            }
        }

        public double WorldEnd
        {
            get => _worldEnd;
            set
            {
                if (_worldEnd == value) return;
                _worldEnd = value;
                _offset = clampOffset(_offset);
                OnPropertyChanged();
            }
        }

        //public double CursorPosition
        //{
        //    get => _cursorPosition;
        //    set
        //    {
        //        if (_cursorPosition == value) return;
        //        _cursorPosition = value;
        //        OnPropertyChanged();
        //    }
        //}

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


        //public double WorldLength => WorldEnd - WorldStart;
        public double VisibleWorldWidth => ViewportLength / Zoom;
        public double VisibleStart => Offset;
        public double VisibleEnd => Offset + VisibleWorldWidth;

        // --- coordinate transforms ---
          
        
        //public double DomainToScreen(double value)
        //        => value * PixelsPerUnit - Offset;

        public double WorldToScreen(double world) =>
            (world - Offset) * Zoom * Scale;

        //public double WorldToYScreen(double world) =>
        //    (world - Offset) * Zoom * Scale;

        public double ScreenToWorld(double screen) =>
            Offset + screen / Zoom;

        // --- navigation ---

        public void Pan(double deltaWorld) =>
            Offset += deltaWorld;

        public void CenterOn(double worldPosition) =>
            Offset = worldPosition - VisibleWorldWidth * 0.5;

        /// <summary>Zoom by <paramref name="factor"/> keeping the world position
        /// under <paramref name="screenX"/> stationary.</summary>
        public void ZoomAt(double factor, double screenX)
        {
            var worldAnchor = ScreenToWorld(screenX);
            Zoom *= factor;
            // Shift offset so the anchor point stays under the cursor
            Offset += worldAnchor - ScreenToWorld(screenX);
        }

        // --- helpers ---

        private double clampOffset(double offset)
        {
            // When the visible range is wider than the world, pin to WorldStart
            double maxOffset = Math.Max(WorldStart, WorldEnd - VisibleWorldWidth);
            return Math.Clamp(offset, WorldStart, maxOffset);
        }
    }
}