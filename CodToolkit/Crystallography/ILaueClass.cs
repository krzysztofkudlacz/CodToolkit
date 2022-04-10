using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using CodToolkit.Algebra;

namespace CodToolkit.Crystallography
{
    public interface ILaueClass : IReadOnlyList<Matrix3X3>
    {

    }

    public class LaueClass : ILaueClass
    {
        public void Foo()
        {
            var matrix1 = new Matrix3D();
            var matrix2 = new Matrix3D();
            var matrix = matrix1 * matrix2;

            
        }

        public IEnumerator<Matrix3X3> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; }

        public Matrix3D this[int index] => throw new NotImplementedException();
    }
}
