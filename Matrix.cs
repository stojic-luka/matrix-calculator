using System;

namespace MatrixMath {
    internal class Matrix {
        #region MainVariables

        protected float[,] array = new float[0, 0];
        protected int rows = 0, cols = 0;

        public bool squareMatrix => rows == cols;

        public int sizeX => rows;
        public int sizeY => cols;
        public int size => squareMatrix ? rows : 0;

        public float[,] getArray() => array;

#nullable enable
        public float[]? getRow(int row) {
            if (row >= rows) return null;
            float[] result = new float[cols];
            for (int i = 0; i < cols; i++)
                result[i] = array[row, i];
            return result;
        }
        public float[]? getColumn(int column) {
            if (column >= cols) return null;
            float[] result = new float[rows];
            for (int i = 0; i < rows; i++)
                result[i] = array[i, column];
            return result;
        }
        public float[]? getDiagonal() {
            if (!squareMatrix)
                return null;
            float[] result = new float[rows];
            for (int i = 0; i < rows; i++)
                result[i] = At(i, i);
            return result;
        }
        public float[]? getReverseDiagonal() {
            if (!squareMatrix)
                return null;
            float[] result = new float[rows];
            for (int i = 0; i < rows; i++)
                result[i] = At(i, (rows - 1) - i);
            return result;
        }
#nullable disable

        public float At(int row, int column) {
            if (row >= rows || column >= cols)
                throw new IndexOutOfRangeException();
            return array[row, column];
        }
        public void At(int row, int column, float value) {
            if (row >= rows || column >= cols)
                throw new IndexOutOfRangeException();
            array[row, column] = value;
        }

        #endregion

        #region Constructors

        public Matrix() => InitializeMatrix(1, 1);
        public Matrix(int size) => InitializeMatrix(size, size);
        public Matrix(int rows, int cols) => InitializeMatrix(rows, cols);
        public Matrix(float[,] array) => InitializeMatrix(array);
        public Matrix(Matrix matrix) => InitializeMatrix(matrix);

        private void InitializeMatrix(int rows, int cols) {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentOutOfRangeException();
            this.rows = rows;
            this.cols = cols;
            this.array = new float[rows, cols];
        }
        private void InitializeMatrix(float[,] array) {
            if (array == null)
                throw new ArgumentNullException();
            this.rows = array.GetLength(0);
            this.cols = array.GetLength(1);

            this.array = new float[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    this.array[i, j] = array[i, j];
        }
        private void InitializeMatrix(Matrix matrix) => InitializeMatrix(matrix.array);

        #region Defaults

        public static Matrix Mat1x1 => new Matrix(1);
        public static Matrix Mat2x2 => new Matrix(2);
        public static Matrix Mat3x3 => new Matrix(3);
        public static Matrix Mat4x4 => new Matrix(4);
        public static Matrix Mat5x5 => new Matrix(5);
        public static Matrix Mat10x10 => new Matrix(10);

        #region Identity
        public static Matrix IdentityMatrix(int size) {
            if (size <= 0)
                throw new ArgumentOutOfRangeException();
            Matrix result = new Matrix(size, size);
            for (int i = 0; i < size; i++)
                result.At(i, i, 1);
            return result;
        }
        public static Matrix Identity2x2 => IdentityMatrix(2);
        public static Matrix Identity3x3 => IdentityMatrix(3);
        public static Matrix Identity4x4 => IdentityMatrix(4);
        public static Matrix Identity5x5 => IdentityMatrix(5);
        public static Matrix Identity10x10 => IdentityMatrix(10);
        #endregion
        #endregion

        #endregion

        #region Operators

        public static bool operator ==(Matrix a, Matrix b) => a.array == b.array;
        public static bool operator !=(Matrix a, Matrix b) => a.array != b.array;

        public static bool operator &(Matrix a, Matrix b) => a.rows == b.rows && a.cols == b.cols;
        public static Matrix operator |(Matrix a, Matrix b) => a.array.Length > b.array.Length ? a : b;

        public static Matrix operator *(Matrix a, float scalar) {
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++)
                    a.At(i, j, a.At(i, j) * scalar);
            return a;
        }
        public static bool CanMultiply(Matrix a, Matrix b) => a.cols == b.rows;
        public static void GetMultiplySize(Matrix a, Matrix b, ref int rows, ref int columns) {
            if (!CanMultiply(a, b)) {
                rows = 0; columns = 0;
                return; 
            }
            rows = a.sizeX;
            columns = b.sizeY;
        }
        public static Matrix operator *(Matrix a, Matrix b) {
            if (!CanMultiply(a, b))
                return a;
            Matrix result = new Matrix(a.rows, b.cols);
            float value = 0;
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < b.cols; j++) {
                    value = 0;
                    for (int k = 0; k < b.rows; k++) {
                        value += a.At(i, k) * b.At(k, j);
                    }
                    result.At(i, j, value);
                }
            return result;
        }

        public static Matrix operator +(Matrix a, Matrix b) {
            if (!(a & b))
                throw new Exception("Trying to sum matrices with diferent sizes!");

            Matrix result = new Matrix(a.sizeX, a.sizeY);
            for (int i = 0; i < a.sizeX; i++)
                for (int j = 0; j < a.sizeY; j++)
                    result.At(i, j, a.At(i, j) + b.At(i, j));
            return result;
        }
        public static Matrix operator -(Matrix a, Matrix b) {
            if (!(a & b))
                throw new Exception("Trying to sum matrices with diferent sizes!");

            Matrix result = new Matrix(a.sizeX, a.sizeY);
            for (int i = 0; i < a.sizeX; i++)
                for (int j = 0; j < a.sizeY; j++)
                    result.At(i, j, a.At(i, j) - b.At(i, j));
            return result;
        }

        #endregion

        #region PublicFunctions
        public void print() => PrintMatrix(this);

        #endregion

        #region Statics

        public static Matrix Transponse(Matrix matrix) {
            Matrix result = new Matrix(matrix.cols, matrix.rows);
            for (int i = 0; i < matrix.rows; i++)
                for (int j = 0; j < matrix.cols; j++)
                    result.At(j, i, matrix.At(i, j));
            return result;
        }

        [Obsolete("Pow2 canno't be used anymore,use Pow instead.")]
        public static Matrix Pow2(Matrix matrix, int power = 2) => matrix * (float)Math.Pow(3, power - 1);

        public static Matrix Pow(Matrix matrix, int power) {
            if (power <= 0)
                throw new ArgumentOutOfRangeException();
            else if (power == 1)
                return matrix;

            return matrix * Pow(matrix, power - 1);
        }

        #region Minor
        public static Matrix Minor(Matrix matrix, int row, int col) {
            if (row >= matrix.rows || col >= matrix.cols)
                throw new IndexOutOfRangeException();
            void nextIndex(ref int x, ref int y) {
                y++;
                if (y >= matrix.sizeY - 1) {
                    y = 0;
                    x++;
                }
            }
            Matrix result = new Matrix(matrix.sizeX - 1, matrix.sizeY - 1);
            for (int i = 0, m = 0; i < matrix.rows; i++) {
                for (int j = 0, k = 0; j < matrix.cols; j++) {
                    if (i == row || j == col) continue;

                    result.At(m, k, matrix.At(i, j));
                    nextIndex(ref m, ref k);
                }
            }
            return result;
        }
        public static Matrix Minor(Matrix matrix, int index) => Minor(matrix, index, index);
        #endregion

        #region Determinant

        public static float Determinant2x2(Matrix matrix) {
            if (matrix.size != 2)
                throw new AccessViolationException();
            return (matrix.At(0, 0) * matrix.At(1, 1)) -
             (matrix.At(0, 1) * matrix.At(1, 0));
        }
        [Obsolete("Determinant3x3 is deprecated, please use Determinant instead.")]
        public static float Determinant3x3(Matrix matrix) {
            if (matrix.size != 3)
                throw new AccessViolationException();
            return (matrix.At(0, 0) * Determinant2x2(Minor(matrix, 0, 0))) -
                (matrix.At(0, 1) * Determinant2x2(Minor(matrix, 0, 1))) +
                (matrix.At(0, 2) * Determinant2x2(Minor(matrix, 0, 2)));
        }
        public static float Determinant(Matrix matrix) {
            if (!matrix.squareMatrix)
                throw new AccessViolationException();
            else if (matrix.size == 2)
                return Determinant2x2(matrix);

            float value = 0;
            for (int i = 0, coef = 1; i < matrix.rows; i++, coef *= -1)
                value += coef *
                    matrix.At(i, 0) *
                    Determinant(Minor(matrix, i, 0));
            return value;
        }

        #endregion

        #region Adjoint
        //A(i,j) = (-1)^(i+j)*|M(j,i)|
        public static Matrix Adjugate2x2(Matrix matrix) {
            if (matrix.size != 2)
                throw new AccessViolationException();
            Matrix result = new Matrix(2, 2);
            result.At(0, 0, matrix.At(1, 1));
            result.At(1, 1, matrix.At(0, 0));

            result.At(0, 1, -matrix.At(0, 1));
            result.At(1, 0, -matrix.At(1, 0));
            return result;
        }
        [Obsolete("Adjugate3x3 is deprecated, please use Adjugate instead.")]
        public static Matrix Adjugate3x3(Matrix matrix) {
            if (matrix.size != 3)
                throw new AccessViolationException();
            Matrix result = new Matrix(3, 3);
            float value = 0;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Matrix minor = Minor(matrix, i, j);
                    value = (int)Math.Pow(-1, (i + 1) + (j + 1)) * Determinant2x2(minor);
                    result.At(j, i, value);
                }
            }
            return result;
        }
        public static Matrix Adjugate(Matrix matrix) {
            if (!matrix.squareMatrix)
                throw new AccessViolationException();
            Matrix result = new Matrix(matrix.rows, matrix.cols);
            float value = 0;
            for (int i = 0; i < matrix.rows; i++) {
                for (int j = 0; j < matrix.cols; j++) {
                    value = (int)Math.Pow(-1, (i + 1) + (j + 1)) * Determinant(Minor(matrix, i, j));
                    result.At(j, i, value);
                }
            }
            return result;
        }
        #endregion

        #region Inverse

        [Obsolete("Inverse2x2 is deprecated, please use Inverse instead.")]
        public static Matrix Inverse2x2(Matrix matrix) {
            if (matrix.size != 2)
                throw new AccessViolationException();
            float determinant = Determinant2x2(matrix);
            if (determinant == 0)
                return matrix;
            Matrix adjointMatrix = Adjugate2x2(matrix);
            return adjointMatrix * (1 / determinant);
        }
        [Obsolete("CheckInverse2x2 is deprecated, please use CheckInverse instead.")]
        public bool CheckInverse2x2(Matrix matrix, Matrix inverse) {
            if (matrix & inverse)
                return false;
            return (matrix * inverse) == Identity2x2;
        }
        [Obsolete("Inverse3x3 is deprecated, please use Inverse instead.")]
        public static Matrix Inverse3x3(Matrix matrix) {
            if (matrix.size != 3)
                throw new AccessViolationException();
            float determinant = Determinant3x3(matrix);
            if (determinant == 0)
                return matrix;
            Matrix adjointMatrix = Adjugate3x3(matrix);
            return adjointMatrix * (1 / determinant);
        }
        [Obsolete("CheckInverse3x3 is deprecated, please use CheckInverse instead.")]
        public bool CheckInverse3x3(Matrix matrix, Matrix inverse) {
            if (matrix & inverse)
                return false;
            return (matrix * inverse) == Identity3x3;
        }
        public static Matrix Inverse(Matrix matrix) {
            if (!matrix.squareMatrix)
                throw new AccessViolationException();
            float determinant = Determinant(matrix);
            if (determinant == 0)
                return matrix;
            if (matrix.size == 2)
                return Inverse2x2(matrix);
            Matrix adjointMatrix = Adjugate(matrix);
            return adjointMatrix * (1 / determinant);
        }
        public bool CheckInverse(Matrix matrix, Matrix inverse) {
            if (matrix & inverse)
                return false;
            return (matrix * inverse) == IdentityMatrix(matrix.rows);
        }

        #endregion

        #region Printing
        public static void PrintColumn(float[] column) {
            foreach (var item in column)
                Console.WriteLine(item.ToString());
        }
        public static void PrintRow(float[] row) {
            foreach (var item in row)
                Console.Write(item.ToString() + "\t");
            Console.Write("\n");
        }
        public static void PrintMatrix(Matrix matrix) {
            for (int i = 0; i < matrix.rows; i++)
                PrintRow(matrix.getRow(i));
        }

        #nullable enable
            public override bool Equals(object? obj) {
            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (ReferenceEquals(obj, null)) {
                return false;
            }

            return base.Equals(obj);
        }
        #nullable disable
        public override int GetHashCode() => base.GetHashCode();
        #endregion

        #endregion
    }
}
