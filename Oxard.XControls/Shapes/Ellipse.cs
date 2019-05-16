﻿using Oxard.XControls.Graphics;

namespace Oxard.XControls.Shapes
{
    public class Ellipse : Shape
    {
        private Geometry actualGeometry;

        public override Geometry Geometry => this.actualGeometry;

        protected override void OnSizeAllocated(double width, double height)
        {
            this.CalculateGeometry();
        }

        private void CalculateGeometry()
        {
            this.actualGeometry = GeometryHelper.GetEllipse(this.Width, this.Height, this.StrokeThickness);
            this.InvalidateGeometry();
        }
    }
}
