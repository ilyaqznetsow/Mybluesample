using System;
using System.Collections.Generic;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace MyBlueSample.Views
{
    public class CustomAnimation : SKCanvasView
    {
        List<RainDrop> _raindrops;

        bool _isAnimating;

        public CustomAnimation()
        {
            _raindrops = new List<RainDrop>();
            EnableTouchEvents = true;
        }

        protected override void OnTouch(SKTouchEventArgs e)
        {
            base.OnTouch(e);
            if (e.ActionType != SKTouchAction.Pressed)
                return;
            var center = new SKPoint(e.Location.X, e.Location.Y);
            SKColor endColor = SKColors.LightSkyBlue;
            var drop = new RainDrop(center, endColor);
            _raindrops.Add(drop);
            drop.Stopwatch = new Stopwatch();
            Animate(drop);
            Debug.WriteLine($"drop at {drop.Center.X} {drop.Center.Y}");
        }

        int duration = 2;
        void Animate(RainDrop rainDrop)
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(16), () =>
            {
                if (rainDrop.Stopwatch.ElapsedTicks == 0)
                    rainDrop.Stopwatch.Start();

                double progress = rainDrop.Stopwatch.Elapsed.TotalSeconds / duration;
                rainDrop.AnimationCoeff = progress;

                if (progress >= 1)
                {
                    rainDrop.Stopwatch.Stop();
                    rainDrop.Stopwatch = null;
                    _raindrops.Remove(rainDrop);
                    InvalidateSurface();
                    return false;
                }

                InvalidateSurface();
                return true;
            });
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            if (_isAnimating) return;
            var surface = e.Surface;
            var surfaceWidth = e.Info.Width;
            var surfaceHeight = e.Info.Height;

            var canvas = surface.Canvas;
            canvas.Clear();
            _isAnimating = true;
            for (var i = 0; i < _raindrops.Count; i++)
            {
                var drop = _raindrops[i];
                var radius = (float)(surfaceHeight * drop.AnimationCoeff);

                var transparency = 1 - drop.AnimationCoeff * 2;
                transparency = transparency < 0 ? 0 : transparency;
                var startColor = Color.Transparent.ToSKColor();
                var endColor = drop.EndColor.WithAlpha((byte)(0xFF * transparency));

                if (transparency > 0)
                {
                    var shader = SKShader.CreateRadialGradient(drop.Center, radius,
                        new[] { startColor, endColor, startColor },
                        new float[] { 0.65f, 0.75f, 0.85f },
                            SKShaderTileMode.Clamp);
                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill
                    })
                    {
                        paint.Shader = shader;
                        canvas.DrawCircle(drop.Center.X, drop.Center.Y, radius, paint);
                    }
                    Debug.WriteLine($"draw radius {radius} coeff {drop.AnimationCoeff} transp {transparency}");
                }
            }
            canvas.Flush();
            _isAnimating = false;
        }
    }

    public class RainDrop
    {
        public RainDrop(SKPoint center, SKColor endColor)
        {
            Center = center;
            EndColor = endColor;
        }
        public double AnimationCoeff { get; set; }
        public SKColor EndColor { get; set; }
        public SKPoint Center { get; set; }
        public Stopwatch Stopwatch { get; set; }
    }
}
