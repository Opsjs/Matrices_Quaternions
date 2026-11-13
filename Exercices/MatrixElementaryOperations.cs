namespace Exercices;

public static class MatrixElementaryOperations
{
    public static void SwapLines(Matrix<int> matrix, int line1, int line2)
    {
        for (int i = 0; i < matrix.NbLines; i++)
        {
            (matrix[line1, i], matrix[line2, i]) = (matrix[line2, i], matrix[line1, i]);
        }
    }
    public static void SwapColumns(Matrix<int> matrix, int line1, int line2)
    {
        for (int i = 0; i < matrix.NbColumns; i++)
        {
            (matrix[i, line1], matrix[i, line2]) = (matrix[i, line2], matrix[i, line1]);
        }    
    }
    public static void MultiplyLine(Matrix<int> matrix, int line, int factor)
    {
        if (factor == 0) throw new MatrixScalarZeroException();
        for (int i = 0; i < matrix.NbLines; i++)
        {
            matrix[line, i] *= factor;
        }
    }
    public static void MultiplyColumn(Matrix<int> matrix, int column, int factor)
    {
        if (factor == 0) throw new MatrixScalarZeroException();
        for (int i = 0; i < matrix.NbColumns; i++)
        {
            matrix[i, column] *= factor;
        }
    }

    public static void AddLineToAnother(Matrix<int> matrix, int line1, int line2, int factor)
    {
        for (int i = 0; i < matrix.NbLines; i++)
        {
            matrix[line2, i] += factor * matrix[line1, i];
        }
    }

    public static void AddColumnToAnother(Matrix<int> matrix, int column1, int column2, int factor)
    {
        for (int i = 0; i < matrix.NbLines; i++)
        {
            matrix[i, column2] += factor * matrix[i, column1];
        }
    }
}

public class MatrixScalarZeroException : Exception
{
}