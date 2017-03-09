//-----------------------------------------------------------------------
// <copyright file = "Extensions.cs" company = "Me">
//     Copyright (c) Me!
// </copyright>
//-----------------------------------------------------------------------

namespace FreyjaLib
{
    using System;

    public static class Extensions
    {
        /// <summary>
        /// Allows us to do an Action "count" times
        /// </summary>
        /// <param name="count">The number of times to loop</param>
        /// <param name="action">The Action to perform in the loop</param>
        public static void Times(this int count, Action action)
        {
            for (int counter = 0; counter < count; counter++)
            {
                action();
            }
        }
    }
}
