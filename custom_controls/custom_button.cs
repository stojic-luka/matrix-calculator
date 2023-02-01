namespace custom_controls {
    class custom_button : System.Windows.Forms.Button {
        private int borderSize = 0;
        private int borderRadius = 20;
        private System.Drawing.Color borderColor = System.Drawing.Color.PaleVioletRed;

        public custom_button() {
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new System.Drawing.Size(150, 40);
            this.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.ForeColor = System.Drawing.Color.White;
            this.Resize += new System.EventHandler(Button_Resize);
        }

        private void Button_Resize(object sender, System.EventArgs e) {
            if (borderRadius > this.Height)
                borderRadius = this.Height;
        }

        public int BorderSize {
            get { return borderSize; }
            set {
                borderSize = value;
                this.Invalidate();
            }
        }

        public int BorderRadius {
            get { return borderRadius; }
            set {
                borderRadius = value;
                this.Invalidate();
            }
        }

        public System.Drawing.Color BorderColor {
            get { return borderColor; }
            set {
                borderColor = value;
                this.Invalidate();
            }
        }

        public System.Drawing.Color BackgroundColor {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }

        public System.Drawing.Color TextColor {
            get { return this.ForeColor; }
            set { this.ForeColor = value; }
        }

        private System.Drawing.Drawing2D.GraphicsPath GetFigurePath(System.Drawing.Rectangle rect, float radius) {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            float curveSize = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs pevent) {
            base.OnPaint(pevent);
            System.Drawing.Rectangle rectSurface = this.ClientRectangle;
            System.Drawing.Rectangle rectBorder = System.Drawing.Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            int smoothSize = 2;
            if (borderSize > 0)
                smoothSize = borderSize;
            if (borderRadius > 2) {
                using (System.Drawing.Drawing2D.GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (System.Drawing.Drawing2D.GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (System.Drawing.Pen penSurface = new System.Drawing.Pen(this.Parent.BackColor, smoothSize))
                using (System.Drawing.Pen penBorder = new System.Drawing.Pen(borderColor, borderSize)) {
                    pevent.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    this.Region = new System.Drawing.Region(pathSurface);
                    pevent.Graphics.DrawPath(penSurface, pathSurface);
                    if (borderSize >= 1)
                        pevent.Graphics.DrawPath(penBorder, pathBorder);
                }
            } else {
                pevent.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                this.Region = new System.Drawing.Region(rectSurface);
                if (borderSize >= 1) {
                    using (System.Drawing.Pen penBorder = new System.Drawing.Pen(borderColor, borderSize)) {
                        penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }
        }

        protected override void OnHandleCreated(System.EventArgs e) {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += new System.EventHandler(Container_BackColorChanged);
        }

        private void Container_BackColorChanged(object sender, System.EventArgs e) {
            this.Invalidate();
        }
    }
}
