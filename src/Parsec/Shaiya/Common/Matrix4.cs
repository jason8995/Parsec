﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using Parsec.Extensions;
using Parsec.Readers;
using Parsec.Shaiya.Core;

namespace Parsec.Shaiya.Common
{
    [DataContract]
    public class Matrix4 : IBinary
    {
        private readonly float[,] _matrix = new float[4, 4];

        [DataMember]
        public float[,] Data => _matrix;

        private Matrix4x4 _numericMatrix;

        public Matrix4x4 NumericMatrix
        {
            get => _numericMatrix;
            set
            {
                _numericMatrix = value;

                var row1 = new float[]
                {
                    _numericMatrix.M11, _numericMatrix.M12, _numericMatrix.M13, _numericMatrix.M14
                };
                var row2 = new float[]
                {
                    _numericMatrix.M21, _numericMatrix.M22, _numericMatrix.M23, _numericMatrix.M24
                };
                var row3 = new float[]
                {
                    _numericMatrix.M31, _numericMatrix.M32, _numericMatrix.M33, _numericMatrix.M34
                };
                var row4 = new float[]
                {
                    _numericMatrix.M41, _numericMatrix.M42, _numericMatrix.M43, _numericMatrix.M44
                };

                SetRow(1, row1);
                SetRow(2, row2);
                SetRow(3, row3);
                SetRow(4, row4);
            }
        }

        public Matrix4(ShaiyaBinaryReader binaryReader)
        {
            MatrixExtensions.ReadMatrix4x4(binaryReader, out var numericMatrix);
            NumericMatrix = numericMatrix;
        }

        public Matrix4(Matrix4x4 numericMatrix)
        {
            NumericMatrix = numericMatrix;
        }

        public Matrix4(float[] row1, float[] row2, float[] row3, float[] row4)
        {
            if (row1.Length != 4 || row2.Length != 4 || row3.Length != 4 || row4.Length != 4)
            {
                throw new ArgumentException("Rows must have 4 elements each");
            }

            SetRow(1, row1);
            SetRow(2, row2);
            SetRow(3, row3);
            SetRow(4, row4);
        }

        public void SetRow(int rowNumber, float[] data)
        {
            rowNumber--;

            if (rowNumber < 0)
                throw new ArgumentException("Invalid row number");

            if (data.Length != 4)
                throw new ArgumentException("Rows must have 4 elements each");

            for (int i = 0; i < 4; i++)
            {
                _matrix[rowNumber, i] = data[i];
            }
        }

        public void SetColumn(int columnNumber, float[] data)
        {
            columnNumber--;

            if (columnNumber < 0)
                throw new ArgumentException("Invalid column number");

            if (data.Length != 4)
                throw new ArgumentException("Columns must have 4 elements each");

            for (int i = 0; i < 4; i++)
            {
                _matrix[i, columnNumber] = data[i];
            }
        }

        private float[] GetRow(int rowNumber)
        {
            if (rowNumber > 3)
                throw new ArgumentException("The matrix's valid rows are 0-3");

            return Enumerable.Range(0, _matrix.GetLength(0))
                             .Select(x => _matrix[rowNumber, x])
                             .ToArray();
        }

        private float[] GetColumn(int columnNumber)
        {
            if (columnNumber > 3)
                throw new ArgumentException("The matrix's valid columns are 0-3");

            return Enumerable.Range(0, _matrix.GetLength(0))
                             .Select(x => _matrix[x, columnNumber])
                             .ToArray();
        }

        public float[] Row1 => GetRow(0);
        public float[] Row2 => GetRow(1);
        public float[] Row3 => GetRow(2);
        public float[] Row4 => GetRow(3);

        public float[] Col1 => GetColumn(0);
        public float[] Col2 => GetColumn(1);
        public float[] Col3 => GetColumn(2);
        public float[] Col4 => GetColumn(3);

        public float M11 => _matrix[0, 0];
        public float M12 => _matrix[0, 1];
        public float M13 => _matrix[0, 2];
        public float M14 => _matrix[0, 3];
        public float M21 => _matrix[1, 0];
        public float M22 => _matrix[1, 1];
        public float M23 => _matrix[1, 2];
        public float M24 => _matrix[1, 3];
        public float M31 => _matrix[2, 0];
        public float M32 => _matrix[2, 1];
        public float M33 => _matrix[2, 2];
        public float M34 => _matrix[2, 3];
        public float M41 => _matrix[3, 0];
        public float M42 => _matrix[3, 1];
        public float M43 => _matrix[3, 2];
        public float M44 => _matrix[3, 3];

        public byte[] GetBytes()
        {
            // 4 bytes per matrix cell
            var buffer = new List<byte>();
            buffer.AddRange(BitConverter.GetBytes(M11));
            buffer.AddRange(BitConverter.GetBytes(M12));
            buffer.AddRange(BitConverter.GetBytes(M13));
            buffer.AddRange(BitConverter.GetBytes(M14));
            buffer.AddRange(BitConverter.GetBytes(M21));
            buffer.AddRange(BitConverter.GetBytes(M22));
            buffer.AddRange(BitConverter.GetBytes(M23));
            buffer.AddRange(BitConverter.GetBytes(M24));
            buffer.AddRange(BitConverter.GetBytes(M31));
            buffer.AddRange(BitConverter.GetBytes(M32));
            buffer.AddRange(BitConverter.GetBytes(M33));
            buffer.AddRange(BitConverter.GetBytes(M34));
            buffer.AddRange(BitConverter.GetBytes(M41));
            buffer.AddRange(BitConverter.GetBytes(M42));
            buffer.AddRange(BitConverter.GetBytes(M43));
            buffer.AddRange(BitConverter.GetBytes(M44));
            return buffer.ToArray();
        }
    }
}
