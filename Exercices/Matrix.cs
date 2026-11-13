using System.Numerics;

namespace Exercices;

public class Matrix<T> where T : INumber<T>
{
#region Variables    
    public int NbLines;
    public int NbColumns;
    public T[,] ArrayMatrix;
#endregion

#region Constructors
public Matrix(int lines, int columns)
    {
        NbLines = lines;
        NbColumns = columns;
        ArrayMatrix = new T[NbLines, NbColumns];
    }
    public Matrix(T[,] matrix)
    {
        ArrayMatrix = (T[,])matrix.Clone();
        NbLines = matrix.GetLength(0);
        NbColumns = matrix.GetLength(1);
    }

    public Matrix(Matrix<T> matrix)
    {
        ArrayMatrix = (T[,])matrix.ArrayMatrix.Clone();
        NbLines = matrix.NbLines;
        NbColumns = matrix.NbColumns;
    }
#endregion

#region Methods
    public T this[int i, int i1]
    {
        get => ArrayMatrix[i, i1];
        set => ArrayMatrix[i, i1] = value;
    }
    public object? ToArray2D()    
    {
        return ArrayMatrix;
    }
    public static Matrix<T> Identity(int size)
    {
        Matrix<T> matrix = new Matrix<T>(size, size);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i == j) matrix[i, j] = T.One;
                else matrix[i, j] = T.Zero;
            }
        }
        return matrix;
    }
    public bool IsIdentity()
    {
        if (NbColumns != NbLines) return false;
        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                if (i == j)
                {
                    if (this[i, j] != T.One) return false;
                }
                else if (this[i, j] != T.Zero)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public Matrix<T> Multiply(T factor)
    {
        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                this[i, j] *= factor;
            }
        }
        return this;
    }
    public static Matrix<T> Multiply(Matrix<T> baseMatrix, T factor)
    {
        Matrix<T> matrix = new Matrix<T>(baseMatrix);
        return matrix.Multiply(factor);
    }

    public static Matrix<T> operator *(Matrix<T> matrix, T i)
    {
        return new Matrix<T>(Multiply(matrix, i));
    }
    
    public static Matrix<T> operator *(T i, Matrix<T> matrix)
    {
        return new Matrix<T>(Multiply(matrix, i));
    }
    
    public static Matrix<T> operator -(Matrix<T> baseMatrix)
    {
        Matrix<T> matrix = new Matrix<T>(baseMatrix);
        for (int i = 0; i < matrix.NbLines; i++)
        {
            for (int j = 0; j < matrix.NbColumns; j++)
            {
                matrix[i, j] *= -T.One;
            }
        }
        return matrix;
    }
    public Matrix<T> Add(Matrix<T> matrix)
    {
        if (matrix.NbLines != this.NbLines || matrix.NbColumns != this.NbColumns)
        {
            throw new MatrixSumException();
        }
        
        for (int i = 0; i < this.NbLines; i++)
        {
            for (int j = 0; j < this.NbColumns; j++)
            {
                this[i, j] += matrix[i, j];
            }
        }
        return this;
    }
    public static Matrix<T> Add(Matrix<T> baseM1, Matrix<T> baseM2)
    {
        Matrix<T> matrix = new Matrix<T>(baseM1);
        return matrix.Add(baseM2);
    }

    public static Matrix<T> operator +(Matrix<T> m1, Matrix<T> m2)
    {
        return Add(m1, m2);
    }

    public static Matrix<T> operator -(Matrix<T> m1, Matrix<T> m2)
    {
        return Add(m1, -m2);
    }
    public Matrix<T> Multiply(Matrix<T> mB)
    {
        if (this.NbColumns != mB.NbLines) throw new MatrixMultiplyException();
        
        Matrix<T> matrix = new Matrix<T>(this.NbLines, mB.NbColumns);

        for (int i = 0; i < matrix.NbLines; i++)
        {
            for (int j = 0; j < matrix.NbColumns; j++)
            {
                T total = T.Zero;
                for (int k = 0; k < mB.NbLines; k++)
                {
                    total += this[i, k] * mB[k, j];
                }
                matrix[i, j] = total;
            }
        }
        return matrix;
    }
    public static Matrix<T> Multiply(Matrix<T> baseM1, Matrix<T> baseM2)
    {
        return baseM1.Multiply(baseM2);
    }
    
    public static Matrix<T> operator *(Matrix<T> baseM1, Matrix<T> baseM2)
    {
        return Multiply(baseM1, baseM2);
    }
    public Matrix<T> Transpose()
    {
        Matrix<T> matrix = new Matrix<T>(NbColumns, NbLines);
        for (int i = 0; i < matrix.NbLines; i++)
        {
            for (int j = 0; j < matrix.NbColumns; j++)
            {
                matrix[i, j] = this[j, i];
            }
        }
        return matrix;
    }
    public static Matrix<T> Transpose(Matrix<T> matrix)
    {
        return matrix.Transpose();
    }
#endregion

    public static Matrix<T> GenerateAugmentedMatrix(Matrix<T> m1, Matrix<T> m2)
    {
        Matrix<T> matrix = new Matrix<T>(m1.NbLines, m1.NbColumns + m2.NbColumns);
        for (int i = 0; i < matrix.NbLines; i++)
        {
            for (int j = 0; j < matrix.NbColumns; j++)
            {
                if (j < m1.NbColumns) matrix[i, j] = m1[i, j];
                else matrix[i, j] = m2[i, j - m1.NbColumns];
            }
        }
        return matrix;
    }

    public (Matrix<T> m1, Matrix<T> m2) Split(int columnSplitIndex)
    {
        Matrix<T> m1 = new Matrix<T>(NbLines, columnSplitIndex + 1);
        Matrix<T> m2 = new Matrix<T>(NbLines, NbColumns - (columnSplitIndex + 1));
        for (int i = 0; i < m1.NbColumns; i++)
        {
            for (int j = 0; j < NbLines; j++)
            {
                m1[i, j] = this[i, j];
            }
        }

        for (int i = 0; i < m2.NbLines; i++)
        {
            for (int j = 0; j < m2.NbColumns; j++)
            {
                m2[i, j] = this[i, j + (columnSplitIndex + 1)];
            }
        }
        return (m1, m2);
    }

    public Matrix<T> InvertByRowReduction()
    {
        if (NbLines != NbColumns) throw new MatrixInvertException();

        try
        {
            (_, Matrix<T> inverse) = MatrixRowReductionAlgorithm.Apply(this, Identity(NbLines), true);
            return inverse;
        }
        catch (MatrixRowReductionException)
        {
            throw new MatrixInvertException();
        }
    }
    public static Matrix<T> InvertByRowReduction(Matrix<T> m)
    {
        return m.InvertByRowReduction();
    }

    public Matrix<T> SubMatrix(int row, int column)
    {
        Matrix<T> sub = new Matrix<T>(NbLines - 1, NbColumns - 1);

        for (int i = 0; i < NbLines; i++)
        {
            if (i == row) continue;
            int subI = i < row ? i : i - 1;

            for (int j = 0; j < NbColumns; j++)
            {
                if (j == column) continue;
                int subJ = j < column ? j : j - 1;

                sub[subI, subJ] = this[i, j];
            }
        }
        return sub;
    }

    public static Matrix<T> SubMatrix(Matrix<T> matrix, int row, int column)
    {
        return matrix.SubMatrix(row, column);
    }
    
    private T Determinant()
    {
        if (NbLines != NbColumns) throw new MatrixDeterminantException();

        if (NbLines == 1) return this[0, 0];
        if (NbLines == 2) return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];

        T determinant = T.Zero;
        for (int j = 0; j < NbColumns; j++)
        {
            if (this[0, j] == T.Zero) continue; 
            T cofactor = this[0, j] * SubMatrix(0, j).Determinant();
            determinant += (j % 2 == 0) ? cofactor : -cofactor; 
        }
        return determinant;
    }

    public static T Determinant(Matrix<T> matrix)
    {
        return matrix.Determinant();
    }
    public Matrix<T> Adjugate()
    {
        if (NbLines != NbColumns) throw new MatrixAdjugateException();

        Matrix<T> adjugate = new Matrix<T>(NbLines, NbColumns);

        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                T cofactor = SubMatrix(i, j).Determinant();
                if ((i + j) % 2 != 0) cofactor = -cofactor; 

                adjugate[j, i] = cofactor; 
            }
        }
        return adjugate;
    }

    public static Matrix<T> Adjugate(Matrix<T> matrix)
    {
        return matrix.Adjugate();
    }
    
    public Matrix<T> InvertByDeterminant()
    {
        if (NbLines != NbColumns) throw new MatrixInvertException();

        T determinant = Determinant();

        if (T.Abs(determinant) <= T.CreateTruncating(1e-5))
            throw new MatrixInvertException();

        return Adjugate() * (T.One / determinant);
    }

    public static Matrix<T> InvertByDeterminant(Matrix<T> matrix)
    {
        return matrix.InvertByDeterminant();
    }
    
    private static Matrix<T> FromVector4(Vector4 v)
    {
        return new Matrix<T>(new T[,]
        {
            { T.CreateTruncating(v.x) },
            { T.CreateTruncating(v.y) },
            { T.CreateTruncating(v.z) },
            { T.CreateTruncating(v.w) },
        });
    }
    public static Vector4 operator *(Matrix<T> m, Vector4 v)
    {
        Matrix<T> result = m * FromVector4(v);
        return new Vector4(
            float.CreateTruncating(result[0, 0]),
            float.CreateTruncating(result[1, 0]),
            float.CreateTruncating(result[2, 0]),
            float.CreateTruncating(result[3, 0]));
    }
    
    public static implicit operator Matrix<T>(Vector4 v)
    {
        return new Matrix<T>(new T[,]
        {
            { T.CreateTruncating(v.x) },
            { T.CreateTruncating(v.y) },
            { T.CreateTruncating(v.z) },
            { T.CreateTruncating(v.w) },
        });
    }
}


public static class MatrixRowReductionAlgorithm
{
    public static (Matrix<T> m1, Matrix<T> m2) Apply<T> (Matrix<T> m1, Matrix<T> m2, bool ThrowException = false) where T : INumber<T>
    {
        if (m1.NbLines != m2.NbLines) throw new MatrixRowReductionException();

        T epsilon = T.CreateTruncating(1e-5);
        Matrix<T> augmentedMatrix = Matrix<T>.GenerateAugmentedMatrix(m1, m2);
        int pivotRow = 0;

        for (int col = 0; col < m1.NbColumns && pivotRow < augmentedMatrix.NbLines; col++)
        {
            int best = pivotRow;
            for (int i = pivotRow + 1; i < augmentedMatrix.NbLines; i++)
            {
                if (T.Abs(augmentedMatrix[i, col]) > T.Abs(augmentedMatrix[best, col]))
                    best = i;
            }

            if (T.Abs(augmentedMatrix[best, col]) <= epsilon)
            {
                if (ThrowException) throw new MatrixRowReductionException();
                continue;
            }
            if (best != pivotRow)
            {
                for (int j = 0; j < augmentedMatrix.NbColumns; j++)
                {
                    (augmentedMatrix[pivotRow, j], augmentedMatrix[best, j]) = (augmentedMatrix[best, j], augmentedMatrix[pivotRow, j]);
                }
            }
            T pivot = augmentedMatrix[pivotRow, col];
            for (int j = 0; j < augmentedMatrix.NbColumns; j++)
            {
                augmentedMatrix[pivotRow, j] /= pivot;
            }
            for (int i = 0; i < augmentedMatrix.NbLines; i++)
            {
                if (i == pivotRow) continue;
                T factor = augmentedMatrix[i, col];
                if (factor == T.Zero) continue;
                for (int j = 0; j < augmentedMatrix.NbColumns; j++)
                    augmentedMatrix[i, j] -= factor * augmentedMatrix[pivotRow, j];
            }

            pivotRow++;
        }

        return augmentedMatrix.Split(m1.NbColumns - 1);
    }
}





#region Exceptions
public class MatrixSumException : Exception
{
}
public class MatrixMultiplyException : Exception
{
}
public class MatrixRowReductionException : Exception
{
}
public class MatrixInvertException : Exception
{
}
public class MatrixDeterminantException : Exception
{
}
public class MatrixAdjugateException : Exception
{
}
#endregion

public struct Vector4(float x, float y, float z, float w)
{
    public float x = x;
    public float y = y;
    public float z = z;
    public float w = w;
}
public struct Vector3(float x, float y, float z)
{
    public float x = x;
    public float y = y;
    public float z = z;
}
public class Transform
{
    public Vector3 LocalPosition { get; set; } = new Vector3(0f, 0f, 0f);
    public Vector3 LocalRotation { get; set; } = new Vector3(0f, 0f, 0f);
    
    public Vector3 LocalScale { get; set; } = new Vector3(1f, 1f, 1f);

    public Transform? Parent { get; private set; }

    private static float ToRadians(float degrees) => degrees * MathF.PI / 180f;

    public void SetParent(Transform? parent)
    {
        for (Transform? t = parent; t != null; t = t.Parent)
        {
            if (t == this) return;
        }
        Parent = parent;
    }

    public Matrix<float> LocalTranslationMatrix
    {
        get
        {
            Matrix<float> m = Matrix<float>.Identity(4);
            m[0, 3] = LocalPosition.x;
            m[1, 3] = LocalPosition.y;
            m[2, 3] = LocalPosition.z;
            return m;
        }
    }

    public Matrix<float> LocalRotationXMatrix
    {
        get
        {
            float a = ToRadians(LocalRotation.x);
            float cos = MathF.Cos(a);
            float sin = MathF.Sin(a);

            Matrix<float> m = Matrix<float>.Identity(4);
            m[1, 1] = cos;
            m[1, 2] = -sin;
            m[2, 1] = sin;
            m[2, 2] = cos;
            return m;
        }
    }

    public Matrix<float> LocalRotationYMatrix
    {
        get
        {
            float a = ToRadians(LocalRotation.y);
            float cos = MathF.Cos(a);
            float sin = MathF.Sin(a);

            Matrix<float> m = Matrix<float>.Identity(4);
            m[0, 0] = cos;
            m[0, 2] = sin;
            m[2, 0] = -sin;
            m[2, 2] = cos;
            return m;
        }
    }

    public Matrix<float> LocalRotationZMatrix
    {
        get
        {
            float a = ToRadians(LocalRotation.z);
            float cos = MathF.Cos(a);
            float sin = MathF.Sin(a);

            Matrix<float> m = Matrix<float>.Identity(4);
            m[0, 0] = cos;
            m[0, 1] = -sin;
            m[1, 0] = sin;
            m[1, 1] = cos;
            return m;
        }
    }

    public Matrix<float> LocalRotationMatrix
    {
        get => LocalRotationYMatrix * LocalRotationXMatrix * LocalRotationZMatrix;
    }

    public Matrix<float> LocalScaleMatrix
    {
        get
        {
            Matrix<float> m = Matrix<float>.Identity(4);
            m[0, 0] = LocalScale.x;
            m[1, 1] = LocalScale.y;
            m[2, 2] = LocalScale.z;
            return m;
        }
    }

    private Matrix<float> LocalMatrix
    {
        get => LocalTranslationMatrix * LocalRotationMatrix * LocalScaleMatrix;
    }

    public Matrix<float> LocalToWorldMatrix
    {
        get => Parent == null ? LocalMatrix : Parent.LocalToWorldMatrix * LocalMatrix;
    }

    public Matrix<float> WorldToLocalMatrix
    {
        get => LocalToWorldMatrix.InvertByDeterminant();
    }

    public Vector3 WorldPosition
    {
        get
        {
            Matrix<float> m = LocalToWorldMatrix;
            return new Vector3(m[0, 3], m[1, 3], m[2, 3]);
        }
        set
        {
            if (Parent == null)
            {
                LocalPosition = value;
            }
            else
            {
                Vector4 local = Parent.WorldToLocalMatrix * new Vector4(value.x, value.y, value.z, 1f);
                LocalPosition = new Vector3(local.x, local.y, local.z);
            }
        }
    }
    public Quaternion LocalRotationQuaternion
    {
        get => Quaternion.Euler(LocalRotation);
        set => LocalRotation = value.EulerAngles;
    }
}
public struct Quaternion(float x, float y, float z, float w)
{
    public float x = x;
    public float y = y;
    public float z = z;
    public float w = w;

    public static Quaternion Identity => new Quaternion(0f, 0f, 0f, 1f);
    
    public static Quaternion AngleAxis(float angle, Vector3 axis)
    {
        float half = angle * MathF.PI / 180f / 2f;
        float sin = MathF.Sin(half);
        float cos = MathF.Cos(half);

        float length = MathF.Sqrt(axis.x * axis.x + axis.y * axis.y + axis.z * axis.z);
        if (length < 1e-6f) return Identity;

        float nx = axis.x / length;
        float ny = axis.y / length;
        float nz = axis.z / length;

        return new Quaternion(nx * sin, ny * sin, nz * sin, cos);
    }
    
    public static Quaternion operator *(Quaternion a, Quaternion b)
    {
        return new Quaternion(
            a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
            a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
            a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
            a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z);
    }
    
    public static Vector3 operator *(Quaternion q, Vector3 v)
    {
        float tx = 2f * (q.y * v.z - q.z * v.y);
        float ty = 2f * (q.z * v.x - q.x * v.z);
        float tz = 2f * (q.x * v.y - q.y * v.x);

        return new Vector3(
            v.x + q.w * tx + (q.y * tz - q.z * ty),
            v.y + q.w * ty + (q.z * tx - q.x * tz),
            v.z + q.w * tz + (q.x * ty - q.y * tx));
    }
    
    public Matrix<float> Matrix
    {
        get
        {
            float xx = x * x, yy = y * y, zz = z * z;
            float xy = x * y, xz = x * z, yz = y * z;
            float wx = w * x, wy = w * y, wz = w * z;

            Matrix<float> m = Matrix<float>.Identity(4);
            m[0, 0] = 1f - 2f * (yy + zz);
            m[0, 1] = 2f * (xy - wz);
            m[0, 2] = 2f * (xz + wy);

            m[1, 0] = 2f * (xy + wz);
            m[1, 1] = 1f - 2f * (xx + zz);
            m[1, 2] = 2f * (yz - wx);

            m[2, 0] = 2f * (xz - wy);
            m[2, 1] = 2f * (yz + wx);
            m[2, 2] = 1f - 2f * (xx + yy);
            return m;
        }
    }
    
    public static Quaternion Euler(float x, float y, float z)
    {
        Quaternion qx = AngleAxis(x, new Vector3(1f, 0f, 0f));
        Quaternion qy = AngleAxis(y, new Vector3(0f, 1f, 0f));
        Quaternion qz = AngleAxis(z, new Vector3(0f, 0f, 1f));
        return qy * qx * qz;
    }

    public static Quaternion Euler(Vector3 euler) => Euler(euler.x, euler.y, euler.z);

    public Vector3 EulerAngles
    {
        get
        {
            Matrix<float> m = Matrix;
            const float rad2deg = 180f / MathF.PI;

            float sinX = Math.Clamp(-m[1, 2], -1f, 1f);
            float ax = MathF.Asin(sinX);
            float ay, az;

            if (MathF.Abs(sinX) < 0.9999f)
            {
                ay = MathF.Atan2(m[0, 2], m[2, 2]);
                az = MathF.Atan2(m[1, 0], m[1, 1]);
            }
            else
            {
                az = 0f;
                ay = sinX > 0f ? MathF.Atan2(m[0, 1], m[0, 0]) : MathF.Atan2(-m[0, 1], m[0, 0]);
            }

            return new Vector3(ax * rad2deg, ay * rad2deg, az * rad2deg);
        }
    }
    
}