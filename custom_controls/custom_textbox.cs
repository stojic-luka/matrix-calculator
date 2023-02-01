namespace custom_controls {
    [System.ComponentModel.DefaultEvent("_TextChanged")]
    public partial class custom_textbox : System.Windows.Forms.UserControl {
        private System.Drawing.Color borderColor = System.Drawing.Color.MediumSlateBlue;
        private System.Drawing.Color borderFocusColor = System.Drawing.Color.HotPink;
        private int borderSize = 2;
        private bool underlinedStyle = false;
        private bool isFocused = false;

        private int borderRadius = 0;
        private System.Drawing.Color placeholderColor = System.Drawing.Color.DarkGray;
        private string placeholderText = "";
        private bool isPlaceholder = false;
        private bool isPasswordChar = false;

        public custom_textbox() {
            InitializeComponent();
            this.textBox1.TextChanged += textbox1_TextChanged;
            this.textBox1.KeyPress += textbox1_KeyPress;
        }
        public delegate void TextChangedEventHandler(object sender, System.EventArgs e);
        public event TextChangedEventHandler _TextChanged;
        private void textbox1_TextChanged(object sender, System.EventArgs e) {
            _TextChanged?.Invoke(this, e);
        }

        public delegate void KeyPressEventHandler(object sender, System.Windows.Forms.KeyPressEventArgs e);
        public event KeyPressEventHandler _KeyPress;
        private void textbox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
            _KeyPress?.Invoke(this, e);
        }

        public System.Drawing.Color BorderColor {
            get { return borderColor; }
            set {
                borderColor = value;
                this.Invalidate();
            }
        }

        public System.Drawing.Color BorderFocusColor {
            get { return borderFocusColor; }
            set { borderFocusColor = value; }
        }

        public int BorderSize {
            get { return borderSize; }
            set {
                if (value >= 1) {
                    borderSize = value;
                    this.Invalidate();
                }
            }
        }

        public bool UnderlinedStyle {
            get { return underlinedStyle; }
            set {
                underlinedStyle = value;
                this.Invalidate();
            }
        }

        public bool PasswordChar {
            get { return isPasswordChar; }
            set {
                isPasswordChar = value;
                if (!isPlaceholder)
                    textBox1.UseSystemPasswordChar = value;
            }
        }

        public bool Multiline {
            get { return textBox1.Multiline; }
            set { textBox1.Multiline = value; }
        }

        public override System.Drawing.Color BackColor {
            get { return base.BackColor; }
            set {
                base.BackColor = value;
                textBox1.BackColor = value;
            }
        }

        public override System.Drawing.Color ForeColor {
            get { return base.ForeColor; }
            set {
                base.ForeColor = value;
                textBox1.ForeColor = value;
            }
        }

        public override System.Drawing.Font Font {
            get { return base.Font; }
            set {
                base.Font = value;
                textBox1.Font = value;
                if (this.DesignMode)
                    UpdateControlHeight();
            }
        }

        public string Texts {
            get {
                if (isPlaceholder) return "";
                else return textBox1.Text;
            }
            set {
                textBox1.Text = value;
                SetPlaceholder();
            }
        }

        public int BorderRadius {
            get { return borderRadius; }
            set {
                if (value >= 0) {
                    borderRadius = value;
                    this.Invalidate();
                }
            }
        }

        public System.Drawing.Color PlaceholderColor {
            get { return placeholderColor; }
            set {
                placeholderColor = value;
                if (isPlaceholder)
                    textBox1.ForeColor = value;
            }
        }

        public string PlaceholderText {
            get { return placeholderText; }
            set {
                placeholderText = value;
                textBox1.Text = "";
                SetPlaceholder();
            }
        }

        public System.Windows.Forms.HorizontalAlignment TextAllign {
            get { return textBox1.TextAlign; }
            set {
                textBox1.TextAlign = value;
                SetPlaceholder();
            }
        }
        public int MaxLength {
            get { return textBox1.MaxLength; }
            set { textBox1.MaxLength = value; }
        }

        protected override void OnResize(System.EventArgs e) {
            base.OnResize(e);
            if (this.DesignMode)
                UpdateControlHeight();
        }
        protected override void OnLoad(System.EventArgs e) {
            base.OnLoad(e);
            UpdateControlHeight();
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
            base.OnPaint(e);
            System.Drawing.Graphics graph = e.Graphics;

            if (borderRadius > 1) {
                var rectBorderSmooth = this.ClientRectangle;
                var rectBorder = System.Drawing.Rectangle.Inflate(rectBorderSmooth, -borderSize, -borderSize);
                int smoothSize = borderSize > 0 ? borderSize : 1;

                using (System.Drawing.Drawing2D.GraphicsPath pathBorderSmooth = GetFigurePath(rectBorderSmooth, borderRadius))
                using (System.Drawing.Drawing2D.GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (System.Drawing.Pen penBorderSmooth = new System.Drawing.Pen(this.Parent.BackColor, smoothSize))
                using (System.Drawing.Pen penBorder = new System.Drawing.Pen(borderColor, borderSize)) {
                    this.Region = new System.Drawing.Region(pathBorderSmooth);
                    if (borderRadius > 15) SetTextBoxRoundedRegion();
                    graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                    if (isFocused) penBorder.Color = borderFocusColor;

                    if (underlinedStyle) {
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    } else {
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        graph.DrawPath(penBorder, pathBorder);
                    }
                }
            } else {
                using (System.Drawing.Pen penBorder = new System.Drawing.Pen(borderColor, borderSize)) {
                    this.Region = new System.Drawing.Region(this.ClientRectangle);
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                    if (isFocused) penBorder.Color = borderFocusColor;

                    if (underlinedStyle)
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    else
                        graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                }
            }
        }

        private void SetPlaceholder() {
            if (string.IsNullOrWhiteSpace(textBox1.Text) && placeholderText != "") {
                isPlaceholder = true;
                textBox1.Text = placeholderText;
                textBox1.ForeColor = placeholderColor;
                if (isPasswordChar)
                    textBox1.UseSystemPasswordChar = false;
            }
        }
        private void RemovePlaceholder() {
            if (isPlaceholder && placeholderText != "") {
                isPlaceholder = false;
                textBox1.Text = "";
                textBox1.ForeColor = this.ForeColor;
                if (isPasswordChar)
                    textBox1.UseSystemPasswordChar = true;
            }
        }
        private System.Drawing.Drawing2D.GraphicsPath GetFigurePath(System.Drawing.Rectangle rect, int radius) {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, (float)180, (float)90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
        private void SetTextBoxRoundedRegion() {
            System.Drawing.Drawing2D.GraphicsPath pathTxt;
            if (Multiline) {
                pathTxt = GetFigurePath(textBox1.ClientRectangle, borderRadius - borderSize);
                textBox1.Region = new System.Drawing.Region(pathTxt);
            } else {
                pathTxt = GetFigurePath(textBox1.ClientRectangle, borderSize * 2);
                textBox1.Region = new System.Drawing.Region(pathTxt);
            }
            pathTxt.Dispose();
        }
        private void UpdateControlHeight() {
            if (textBox1.Multiline == false) {
                int txtHeight = System.Windows.Forms.TextRenderer.MeasureText("Text", this.Font).Height + 1;
                textBox1.Multiline = true;
                textBox1.MinimumSize = new System.Drawing.Size(0, txtHeight - 5);
                textBox1.Multiline = false;

                this.Height = textBox1.Height + this.Padding.Top + this.Padding.Bottom;
            }
        }

        private void textBox1_Click(object sender, System.EventArgs e) {
            this.OnClick(e);
        }
        private void textBox1_MouseEnter(object sender, System.EventArgs e) {
            this.OnMouseEnter(e);
        }
        private void textBox1_MouseLeave(object sender, System.EventArgs e) {
            this.OnMouseLeave(e);
        }
        private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
            this.OnKeyPress(e);
        }

        private void textBox1_Enter(object sender, System.EventArgs e) {
            isFocused = true;
            this.Invalidate();
            RemovePlaceholder();
        }
        private void textBox1_Leave(object sender, System.EventArgs e) {
            isFocused = false;
            this.Invalidate();
            SetPlaceholder();
        }
    }
}