//-----------------------------------------------------------------------
// <copyright file = "RandomNumberSource.cs" company = "Me">
//     Copyright (c) Me!
// </copyright>
//-----------------------------------------------------------------------

namespace FreyjaLib
{
    using System;

    /// <summary>
    ///  A source of random numbers
    /// </summary>
    public static class RandomNumberSource
    {
        /// <summary>
        /// A source of random numbers
        /// </summary>
        private readonly static Random Random = new Random();

        /// <summary>
        /// Gets the next integer up to maxValue
        /// </summary>
        /// <param name="maxValue">The maximum value of the next integer</param>
        /// <returns></returns>
        public static int GetNextInt(int maxValue)
        {
            return Random.Next(maxValue);
        }

        /// <summary>
        /// Gets the next integer between minValue and maxValue
        /// </summary>
        /// <param name="minValue">The minimum value of the next integer</param>
        /// <param name="maxValue">The maximum value of the next integer</param>
        /// <returns></returns>
        public static int GetNextInt(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }
    }
}
