using MatrixMath;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Matrix_calculator {
    public partial class Form1 : Form {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        public Form1() {
            InitializeComponent();
            matrix_dimentions_textbox_TextChanged(new object { }, new EventArgs());
        }
        private void Form1_Load(object sender, EventArgs e) => Form1_Resize(sender, e);
        private void Form1_Resize(object sender, EventArgs e) {
            tabs.Size = new Size(this.Width - 7, this.Height - 65);
        }

        private void only_allow_numbers_in_textbox(object sender, KeyPressEventArgs e) { if (!char.IsDigit(e.KeyChar) && (e.KeyChar != (char)8)) e.Handled = true; }
        private void only_allow_numbers_and_dash_in_textbox(object sender, KeyPressEventArgs e) { if (!char.IsDigit(e.KeyChar) && (e.KeyChar != (char)8) && (e.KeyChar != (char)45) && (e.KeyChar != (char)46)) e.Handled = true; }
        private void EditingControlShowing_datagridview(object sender, DataGridViewEditingControlShowingEventArgs e) { e.Control.KeyPress += new KeyPressEventHandler(only_allow_numbers_and_dash_in_textbox); }
        private bool if_all_cells_filled(DataGridView matrix_grid) {
            for (int i = 0; i < matrix_grid.Rows.Count; i++)
                for (int j = 0; j < matrix_grid.Columns.Count; j++) {
                    if (matrix_grid.Rows[i].Cells[j].Value != null) {
                        if (matrix_grid.Rows[i].Cells[j].Value.ToString() == "")
                            return false;
                    } else
                        return false;
                }
            return true;
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e) => matrix_dimentions_textbox_TextChanged(new object() { }, new EventArgs());
        private void matrix_size_trackbar_Scroll(object sender, EventArgs e) => matrix_dimentions_textbox_TextChanged(new object() { }, new EventArgs());
        private void trackbar_value_textbox_TextChanged(object sender, EventArgs e) {
            var tb = sender as custom_controls.custom_textbox;
            if (tb.Texts != "" && tb.Texts != "1")
                switch (tb.Name) {
                    case "stepenovanje_trackbar_value": stepenovanje_matrix_size_trackbar.Value = int.Parse(tb.Texts); break;
                    case "minor_trackbar_value": minor_matrix_size_trackbar.Value = int.Parse(tb.Texts); break;
                    case "determinanta_trackbar_value": determinanta_matrix_size_trackbar.Value = int.Parse(tb.Texts); break;
                    case "inverzna_trackbar_value": inverzna_matrix_size_trackbar.Value = int.Parse(tb.Texts); break;
                }
                matrix_dimentions_textbox_TextChanged(new object() { }, new EventArgs());
        }
        private void matrix_dimentions_textbox_TextChanged(object sender, EventArgs e) {
            Size dim1 = new Size(), dim2 = new Size(), dim3 = new Size();
            int MATRIX_OFFSET_NEXT, MATRIX_OFFSET_UNDER, mx_width1, mx_height1, mx_width2, mx_height2;

            switch (tabs.SelectedIndex) {
                case 0:
                    if (sabiranje_matrix_width_textbox.Texts != "" && sabiranje_matrix_height_textbox.Texts != "") {
                        mx_width1 = int.Parse(sabiranje_matrix_width_textbox.Texts) == 0 ? 1 : int.Parse(sabiranje_matrix_width_textbox.Texts);
                        mx_height1 = int.Parse(sabiranje_matrix_height_textbox.Texts) == 0 ? 1 : int.Parse(sabiranje_matrix_height_textbox.Texts);
                        dim1 = new Size(mx_width1, mx_height1);
                        MATRIX_OFFSET_NEXT = 50 + dim1.Width * CELL_WIDTH;
                        MATRIX_OFFSET_UNDER = 40 + dim1.Height * CELL_HEIGHT;
                        operator_sabiranje_label.Location = new Point((MATRIX_OFFSET_NEXT - 57) / 2, MATRIX_OFFSET_UNDER + 25);
                        sabiranje_equals_label.Location = new Point(MATRIX_OFFSET_NEXT - 30, MATRIX_OFFSET_UNDER + 23);
                        draw_matrix(matrix_gridview_subtract1, dim1);
                        draw_matrix(matrix_gridview_subtract2, dim1, 0, MATRIX_OFFSET_UNDER);
                        draw_matrix(matrix_gridview_subtract_final, dim1, MATRIX_OFFSET_NEXT, MATRIX_OFFSET_UNDER / 2);
                        sabiranje_underflow_panel.Size = new Size(
                            Math.Max(70 + matrix_gridview_subtract1.Width + matrix_gridview_subtract_final.Width, 323),
                            100 + matrix_gridview_subtract1.Height + matrix_gridview_subtract2.Height
                        );
                    }
                    break;
                case 1:
                    if (mnozenje_matrix_width_cinilac1_textbox.Texts != "" && mnozenje_matrix_height_cinilac1_textbox.Texts != "" &&
                        mnozenje_matrix_width_cinilac2_textbox.Texts != "" && mnozenje_matrix_height_cinilac2_textbox.Texts != "") {
                        mx_width1 = int.Parse(mnozenje_matrix_width_cinilac1_textbox.Texts) == 0 ? 1 : int.Parse(mnozenje_matrix_width_cinilac1_textbox.Texts);
                        mx_height1 = int.Parse(mnozenje_matrix_height_cinilac1_textbox.Texts) == 0 ? 1 : int.Parse(mnozenje_matrix_height_cinilac1_textbox.Texts);
                        mx_width2 = int.Parse(mnozenje_matrix_width_cinilac2_textbox.Texts) == 0 ? 1 : int.Parse(mnozenje_matrix_width_cinilac2_textbox.Texts);
                        mx_height2 = int.Parse(mnozenje_matrix_height_cinilac2_textbox.Texts) == 0 ? 1 : int.Parse(mnozenje_matrix_height_cinilac2_textbox.Texts);
                        dim1 = new Size(mx_width1, mx_height1);
                        dim2 = new Size(mx_width2, mx_height2);
                        if (mnozenje_brojem_radio_button.Checked) dim3 = new Size(mx_width1, mx_height1);
                        else dim3 = new Size(mx_width2, mx_height1);
                        MATRIX_OFFSET_NEXT = 50 + Math.Max(dim1.Width, dim2.Width) * CELL_WIDTH;
                        MATRIX_OFFSET_UNDER = 40 + dim1.Height * CELL_HEIGHT;
                        operator_mnozenje_label.Location = new Point((MATRIX_OFFSET_NEXT - 56) / 2, MATRIX_OFFSET_UNDER + 23);
                        mnozenje_equals_label.Location = new Point(MATRIX_OFFSET_NEXT - 30, MATRIX_OFFSET_UNDER + 23);
                        matrix_draw_mnozenje2_textbox.Location = new Point(operator_mnozenje_label.Location.X - 21, operator_mnozenje_label.Location.Y + 37);
                        draw_matrix(matrix_gridview_multiply1, dim1);
                        draw_matrix(matrix_gridview_multiply2, dim2, 0, MATRIX_OFFSET_UNDER);
                        draw_matrix(matrix_gridview_multiply_final, dim3, MATRIX_OFFSET_NEXT, mnozenje_equals_label.Location.Y - dim3.Height * CELL_HEIGHT / 2 - 43);
                        mnozenje_underflow_panel.Size = new Size(
                            Math.Max(90 + matrix_gridview_multiply_final.Width + Math.Max(matrix_gridview_multiply1.Width, matrix_gridview_multiply2.Width), 495),
                            mnozenje_brojem_radio_button.Checked ? 151 + matrix_gridview_multiply1.Height + matrix_gridview_multiply_final.Height / 2 :
                            172 + matrix_gridview_multiply1.Height + Math.Max(matrix_gridview_multiply2.Height, matrix_gridview_multiply_final.Height / 2 - 22)
                        );
                        if (mx_height1 == mx_width2) matrix_gridview_multiply_final.BackgroundColor = SystemColors.Control;
                        else matrix_gridview_multiply_final.BackgroundColor = Color.Red;
                    }
                    break;
                case 2:
                    dim1 = new Size(stepenovanje_matrix_size_trackbar.Value, stepenovanje_matrix_size_trackbar.Value);
                    stepenovanje_trackbar_value.Texts = stepenovanje_matrix_size_trackbar.Value.ToString();
                    MATRIX_OFFSET_NEXT = 50 + dim1.Width * CELL_WIDTH;
                    MATRIX_OFFSET_UNDER = 40 + dim1.Height * CELL_HEIGHT;
                    stepenovanje_equals_label.Location = new Point(MATRIX_OFFSET_NEXT, MATRIX_OFFSET_UNDER / 2 + 24);
                    matrix_draw_stepen_textbox.Location = new Point(MATRIX_OFFSET_NEXT - 39, 51);
                    draw_matrix(matrix_gridview_power, dim1);
                    draw_matrix(matrix_gridview_power_final, dim1, MATRIX_OFFSET_NEXT + 24);
                    stepenovanje_underflow_panel.Size = new Size(Math.Max(123 + matrix_gridview_power.Width * 2, 337), 127 + matrix_gridview_power_final.Height);
                    break;
                case 3:
                    dim1 = new Size(minor_matrix_size_trackbar.Value, minor_matrix_size_trackbar.Value);
                    minor_trackbar_value.Texts = minor_matrix_size_trackbar.Value.ToString();
                    MATRIX_OFFSET_NEXT = 50 + dim1.Width * CELL_WIDTH;
                    MATRIX_OFFSET_UNDER = 40 + dim1.Height * CELL_HEIGHT;
                    minor_equals_label.Location = new Point(MATRIX_OFFSET_NEXT - 34, MATRIX_OFFSET_UNDER / 2 + 24);
                    draw_matrix(matrix_gridview_minor, dim1);
                    draw_matrix(matrix_gridview_minor_final, new Size(dim1.Width - 1, dim1.Height - 1), MATRIX_OFFSET_NEXT - 10, 13);
                    minor_underflow_panel.Size = new Size(Math.Max(30 + matrix_gridview_minor.Width * 2, 245), 127 + matrix_gridview_minor.Height);
                    break;
                case 4:
                    dim1 = new Size(determinanta_matrix_size_trackbar.Value, determinanta_matrix_size_trackbar.Value);
                    determinanta_trackbar_value.Texts = determinanta_matrix_size_trackbar.Value.ToString();
                    MATRIX_OFFSET_NEXT = 50 + dim1.Width * CELL_WIDTH;
                    MATRIX_OFFSET_UNDER = 40 + dim1.Height * CELL_HEIGHT;
                    determinanta_equals_label.Location = new Point(MATRIX_OFFSET_NEXT - 34, MATRIX_OFFSET_UNDER / 2 + 24);
                    detA_textbox.Location = new Point(MATRIX_OFFSET_NEXT - 4, MATRIX_OFFSET_UNDER / 2 + 28);
                    draw_matrix(matrix_gridview_determinant, dim1);
                    determinanta_underflow_panel.Size = new Size(Math.Max(125 + matrix_gridview_determinant.Width, 335), 127 + matrix_gridview_determinant.Height);
                    break;
                case 5:
                    dim1 = new Size(inverzna_matrix_size_trackbar.Value, inverzna_matrix_size_trackbar.Value);
                    inverzna_trackbar_value.Texts = inverzna_matrix_size_trackbar.Value.ToString();
                    MATRIX_OFFSET_NEXT = 50 + dim1.Width * CELL_WIDTH;
                    MATRIX_OFFSET_UNDER = 40 + dim1.Height * CELL_HEIGHT;
                    inverzna_equals_label.Location = new Point(MATRIX_OFFSET_NEXT - 34, MATRIX_OFFSET_UNDER / 2 + 24);
                    draw_matrix(matrix_gridview_inverse, dim1);
                    draw_matrix(matrix_gridview_inverse_final, dim1, MATRIX_OFFSET_NEXT - 10);
                    inverzna_underflow_panel.Size = new Size(Math.Max(80 + matrix_gridview_inverse.Width * 2, 335), 127 + matrix_gridview_inverse.Height);
                    break;
            }
        }

        #region Calc Buttons
        private void calculate_zbir_button_Click(object sender, EventArgs e) {
            if (if_all_cells_filled(matrix_gridview_subtract1) && if_all_cells_filled(matrix_gridview_subtract2)) {
                Matrix matrix1 = get_matrix_values(matrix_gridview_subtract1);
                Matrix matrix2 = get_matrix_values(matrix_gridview_subtract2);
                if (sabiranje_radio_button.Checked)
                    set_matrix_values(matrix_gridview_subtract_final, matrix1 + matrix2);
                else
                    set_matrix_values(matrix_gridview_subtract_final, matrix1 - matrix2);
            } else MessageBox.Show("Nisu sva polja ispunjena!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void calculate_proizvod_button_Click(object sender, EventArgs e) {
            if (if_all_cells_filled(matrix_gridview_multiply1) && (mnozenje_matricom_radio_button.Checked ? if_all_cells_filled(matrix_gridview_multiply2) : matrix_draw_mnozenje2_textbox.Texts != "")) {
                Matrix matrix1 = get_matrix_values(matrix_gridview_multiply1);
                if (mnozenje_matricom_radio_button.Checked) {
                    Matrix matrix2 = get_matrix_values(matrix_gridview_multiply2);
                    set_matrix_values(matrix_gridview_multiply_final, matrix1 * matrix2);
                } else {
                    Matrix resenje = matrix1 * float.Parse(matrix_draw_mnozenje2_textbox.Texts);
                    set_matrix_values(matrix_gridview_multiply_final, resenje);
                }
            } else MessageBox.Show("Nisu sva polja ispunjena!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void calculate_stepen_button_Click(object sender, EventArgs e) {
            if (if_all_cells_filled(matrix_gridview_power) && matrix_draw_stepen_textbox.Texts != "") {
                Matrix matrix1 = get_matrix_values(matrix_gridview_power);
                set_matrix_values(matrix_gridview_power_final, Matrix.Pow(matrix1, (int)Math.Round(float.Parse(matrix_draw_stepen_textbox.Texts))));
            } else MessageBox.Show("Nisu sva polja ispunjena!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void calculate_minor_TextBox_Click(object sender, DataGridViewCellEventArgs e) {
            if (if_all_cells_filled(matrix_gridview_minor)) {
                Matrix matrix1 = get_matrix_values(matrix_gridview_minor);
                set_matrix_values(matrix_gridview_minor_final, Matrix.Minor(matrix1, e.RowIndex, e.ColumnIndex));
            }
        }
        private void calculate_determinanta_button_Click(object sender, EventArgs e) {
            if (if_all_cells_filled(matrix_gridview_determinant)) {
                Matrix matrix1 = get_matrix_values(matrix_gridview_determinant);
                detA_textbox.Texts = string.Format("{0:N2}", Matrix.Determinant(matrix1));
            } else MessageBox.Show("Nisu sva polja ispunjena!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void calculate_inverzna_button_Click(object sender, EventArgs e) {
            if (if_all_cells_filled(matrix_gridview_inverse)) {
                Matrix matrix1 = get_matrix_values(matrix_gridview_inverse);
                set_matrix_values(matrix_gridview_inverse_final, Matrix.Inverse(matrix1));
            } else MessageBox.Show("Nisu sva polja ispunjena!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        private const int CELL_HEIGHT = 22, CELL_WIDTH = 55;
        private void draw_matrix(DataGridView matrix_grid, Size dimentions, int OFFSET_NEXT = 0, int OFFSET_UNDER = 0) {
            matrix_grid.Rows.Clear();
            matrix_grid.Columns.Clear();
            matrix_grid.Refresh();
            matrix_grid.Size = new Size(dimentions.Width * CELL_WIDTH + 3, dimentions.Height * CELL_HEIGHT + 3);
            matrix_grid.Location = new Point(6 + OFFSET_NEXT, 57 + OFFSET_UNDER);
            for (int i = 0; i < dimentions.Width; i++) {
                DataGridViewTextBoxCell textBoxCell = new DataGridViewTextBoxCell();
                textBoxCell.MaxInputLength = 6;
                textBoxCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                textBoxCell.Style.Font = new Font("Calibri", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
                textBoxCell.Style.Format = "N2";
                textBoxCell.Style.Padding = new Padding(1);
                textBoxCell.Style.WrapMode = DataGridViewTriState.False;
                textBoxCell.Style.SelectionForeColor = Color.Black;
                textBoxCell.Style.ForeColor = Color.Black;
                if (matrix_grid.Name == "matrix_gridview_multiply_final" && mnozenje_matrix_height_cinilac1_textbox.Texts != mnozenje_matrix_width_cinilac2_textbox.Texts) {
                    textBoxCell.Style.SelectionBackColor = Color.Red;
                    textBoxCell.Style.BackColor = Color.Red;
                } else {
                    textBoxCell.Style.SelectionBackColor = SystemColors.Control;
                    textBoxCell.Style.BackColor = SystemColors.Control;
                }
                DataGridViewColumn newCol = new DataGridViewColumn(textBoxCell);
                newCol.Width = CELL_WIDTH;
                matrix_grid.Columns.Add(newCol);
            }
            for (int i = 0; i < dimentions.Height; i++) {
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.Height = CELL_HEIGHT;
                matrix_grid.Rows.Add(newRow);
            }
        }

        private Matrix get_matrix_values(DataGridView matrix_grid) {
            Matrix matrix = new Matrix(matrix_grid.Rows.Count, matrix_grid.Columns.Count);
            for (int i = 0; i < matrix_grid.Rows.Count; i++)
                for (int j = 0; j < matrix_grid.Columns.Count; j++)
                    matrix.At(i, j, Convert.ToSingle(matrix_grid.Rows[i].Cells[j].Value.ToString().Replace(',', '.')));
            return matrix;
        }
        private static void set_matrix_values(DataGridView matrix_grid, Matrix matrix) {
            for (int i = 0; i < matrix_grid.Rows.Count; i++)
                for (int j = 0; j < matrix_grid.Columns.Count; j++)
                    matrix_grid.Rows[i].Cells[j].Value = matrix.At(i, j).ToString();
        }

        private void change_pages_buttons(object sender, EventArgs e) {
            switch ((sender as Button).Name) {
                case "sabiranje_button": tabs.SelectedIndex = 0; break;
                case "mnozenje_button": tabs.SelectedIndex = 1; break;
                case "stepenovanje_button": tabs.SelectedIndex = 2; break;
                case "minor_button": tabs.SelectedIndex = 3; break;
                case "determinanta_button": tabs.SelectedIndex = 4; detA_textbox.Texts = ""; break;
                case "inverzna_button": tabs.SelectedIndex = 5; break;
            }
        }

        private void mnozenje_radio_button_CheckedChanged(object sender, EventArgs e) {
            if (((RadioButton)sender).Checked == true)
                if (((RadioButton)sender).Name == "mnozenje_matricom_radio_button") {
                    matrix_gridview_multiply2.Visible = true;
                    matrix_draw_mnozenje2_textbox.Visible = false;
                    mnozenje_matrix_width_cinilac2_textbox.Enabled = true;
                    mnozenje_matrix_height_cinilac2_textbox.Enabled = true;
                } else {
                    matrix_gridview_multiply2.Visible = false;
                    matrix_draw_mnozenje2_textbox.Visible = true;
                    mnozenje_matrix_width_cinilac2_textbox.Enabled = false;
                    mnozenje_matrix_height_cinilac2_textbox.Enabled = false;
                }
        }

        private void sabiranje_radio_button_CheckedChanged(object sender, EventArgs e) {
            if (((RadioButton)sender).Checked == true) operator_sabiranje_label.Text = ((RadioButton)sender).Tag.ToString();
        }
    }
}