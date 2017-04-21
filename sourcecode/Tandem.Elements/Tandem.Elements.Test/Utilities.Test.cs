using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class UtilitiesTest
    {
        int[,] _array = { { 1, 2, 3 }, 
                          { 4, 5, 6 } };

        int[][] _jaggedArray = new int[][] 
        {
            new int[] { 1, 2, 3 },
            new int[] { 4, 5, 6 },
        };

        Utilities _sut;

        public UtilitiesTest()
        {
            _sut = new Utilities();
        }


        [TestMethod]
        public void ToArrayPass()
        {
            int[,] expected = _sut.JaggedToArray(_jaggedArray);
            Assert.AreEqual(expected[0, 0], _array[0, 0]);
            Assert.AreEqual(expected[0, 1], _array[0, 1]);
            Assert.AreEqual(expected[1, 1], _array[1, 1]);
        }

        [TestMethod]
        public void ToJaggedArrayPass()
        {
            int[][] expected = _sut.ArrayToJagged(_array);
            Assert.AreEqual(expected[0][0], _jaggedArray[0][0]);
            Assert.AreEqual(expected[0][1], _jaggedArray[0][1]);
            Assert.AreEqual(expected[1][1], _jaggedArray[1][1]);
        }
    }
}
