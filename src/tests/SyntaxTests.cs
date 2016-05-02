using System;

namespace HappyCspp.Tests
{
    public class SyntaxTests : ITestCase
    {
        public cstring Name { get { return "C# syntax tests"; } }

        public cstring Description { get { return null; } }

        public int Priority { get { return 0; } }

        public bool Run()
        {
            #region Array creation
            int[] intArray1 = new int[5];
            Assert.Equals(5, intArray1.Length, "The length of intArray1 should be 5");

            int[] intArray2 = { 1, 2, 3 };
            Assert.Equals(3, intArray2.Length, "The length of intArray2 should be 3");
            Assert.Equals(2, intArray2[1], "The 2nd element of intArray2 should be 2");

            int[] intArray3 = new int[5];
            intArray3.Init(new int[]{ 1, 2, 3 });
            Assert.Equals(5, intArray3.Length, "The length of intArray3 should be 5");
            Assert.Equals(2, intArray3[1], "The 2nd element of intArray3 should be 2");
            Assert.Equals(0, intArray3[3], "The 4th element of intArray3 should be 0");

            int[,] intArray2D = new int[3, 4];
            Assert.Equals(12, intArray2D.Length, "The length of intArray2D should be 12");

            int[,] intArray2D_init = new int[,]
            {
                { 1, 2, 3, 4 },
                { 10, 20, 30, 40 },
                { 100, 200, 300, 400 },
            };
            Assert.Equals(12, intArray2D_init.Length, "The length of intArray2D_init should be 12");
            Assert.Equals(30, intArray2D_init[1, 2], "The element at [1,2] of intArray2D_init should be 30");

            int[,,] intArray3D = new int[3, 4, 2];
            Assert.Equals(24, intArray3D.Length, "The length of intArray3D should be 24");

            int[,,] intArray3D_init = new int[,,]
            {
                { { 1, 2 }, { 2, 3 }, { 3, 4 }, { 4, 5 } },
                { { 2, 3 }, { 3, 4 }, { 4, 5 }, { 5, 6 } },
                { { 3, 4 }, { 4, 5 }, { 5, 6 }, { 6, 7 } },
            };
            Assert.Equals(24, intArray3D_init.Length, "The length of intArray3D_init should be 24");
            Assert.Equals(5, intArray3D_init[1, 2, 1], "The element at [1,2,1] of intArray3D_init should be 5");

            int[][] jaggedArray1 = new int[3][];
            Assert.Equals(3, jaggedArray1.Length, "The length of jaggedArray1 should be 3");

            int[][] jaggedArray1_init = new int[][]
            {
                new int[]{ 1, 2, 3, 4 },
                new int[]{ 2, 3, 4, 5 },
                new int[]{ 3, 4, 5, 6 },
            };
            Assert.Equals(3, jaggedArray1_init.Length, "The length of jaggedArray1_init should be 3");
            Assert.Equals(4, jaggedArray1_init[1][2], "The element at [1][2] of jaggedArray1_init should be 4");

            int[][][][][][] jaggedArray2 = new int[3][][][][][];

            int[,][,] array2DinArray2D = new int[2, 2][,]
            {
                {
                    new int[,]{ { 1, 2 }, {21,31} },
                    new int[,]{ { 1, 2 }, {22,32} }
                },
                {
                    new int[,]{ { 211, 311 },{3,4} },
                    new int[,]{ { 2, 3 },{3,4} }
                }
            };
            Assert.Equals(4, array2DinArray2D.Length, "The length of array2DinArray2D should be 22");
            Assert.Equals(311, array2DinArray2D[1,0][0,1], "The element at [1,0][0,1] of array2DinArray2D should be 311");

            int[,][] jaggedArrayInArray2D = new int[2, 3][]
            {
                {
                    new int[]{ 1, 2 },
                    new int[]{ 2, 3, 4 },
                    new int[]{ 2, 3, 4 }
                },
                {
                    new int[]{ 3, 44 },
                    new int[]{ 4, 5, 6 },
                    new int[]{ 4, 5, 6 }
                }
            };
            Assert.Equals(6, jaggedArrayInArray2D.Length, "The length of jaggedArrayInArray2D should be 6");
            Assert.Equals(44, jaggedArrayInArray2D[1,0][1], "The element at [1,0][1] of jaggedArrayInArray2D should be 44");

            int[][,] array2DInJaggedArray = new int[2][,]
            {
                new int[2, 3]{ { 1, 2, 3 }, { 4, 5, 6 } },
                new int[2, 4]{ { 10, 20, 30, 40 }, { 40, 50, 60, 70 } }
            };
            Assert.Equals(2, array2DInJaggedArray.Length, "The length of array2DInJaggedArray should be 2");
            Assert.Equals(60, array2DInJaggedArray[1][1,2], "The element at [1][1,2] of array2DInJaggedArray should be 60");

            std.Vector<string[]> v = new std.Vector<string[]>();

            for (var it = v.Begin; it != v.End; it++)
            {
            }

            for (var it = v.RBegin; it != v.REnd; it++)
            {
            }

            #endregion
            return true;
        }
    }
}

