namespace custom_controls {
    class custom_radio_button : System.Windows.Forms.RadioButton {
        private System.Drawing.Color checkedColor = System.Drawing.Color.MediumSlateBlue;
        private System.Drawing.Color unCheckedColor = System.Drawing.Color.Gray;

        public custom_radio_button() {
            this.MinimumSize = new System.Drawing.Size(0, 21);
            this.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.AutoSize = false;
        }

        public System.Drawing.Color CheckedColor {
            get { return checkedColor; }
            set {
                checkedColor = value;
                this.Invalidate();
            }
        }

        public System.Drawing.Color UnCheckedColor {
            get { return unCheckedColor; }
            set {
                unCheckedColor = value;
                this.Invalidate();
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs pevent) {
            System.Drawing.Graphics graphics = pevent.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            float rbBorderSize = 18F;
            float rbCheckSize = 12F;
            System.Drawing.RectangleF rectRbBorder = new System.Drawing.RectangleF() {
                X = 0.5F,
                Y = (this.Height - rbBorderSize) / 2,
                Width = rbBorderSize,
                Height = rbBorderSize
            };
            System.Drawing.RectangleF rectRbCheck = new System.Drawing.RectangleF() {
                X = rectRbBorder.X + ((rectRbBorder.Width - rbCheckSize) / 2),
                Y = (this.Height - rbCheckSize) / 2,
                Width = rbCheckSize,
                Height = rbCheckSize
            };
            using (System.Drawing.Pen penBorder = new System.Drawing.Pen(checkedColor, 1.6F))
            using (System.Drawing.SolidBrush brushRbCheck = new System.Drawing.SolidBrush(checkedColor))
            using (System.Drawing.SolidBrush brushText = new System.Drawing.SolidBrush(this.ForeColor)) {
                graphics.Clear(this.BackColor);
                if (this.Checked) {
                    graphics.DrawEllipse(penBorder, rectRbBorder);
                    graphics.FillEllipse(brushRbCheck, rectRbCheck);
                } else {
                    penBorder.Color = unCheckedColor;
                    graphics.DrawEllipse(penBorder, rectRbBorder);
                }
                //Draw text
                graphics.DrawString(this.Text, this.Font, brushText,
                    rbBorderSize + 8, (this.Height - System.Windows.Forms.TextRenderer.MeasureText(this.Text, this.Font).Height) / 2);
            }
        }


    }
}
