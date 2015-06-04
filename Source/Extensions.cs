//-----------------------------------------------------------------------
// <copyright file = "Extensions.cs" company = "Me">
//     Copyright (c) Me!
// </copyright>
//-----------------------------------------------------------------------

namespace Freyja.Source
{
   using System;

   public static class Extensions
   {
      #region Public Methods

      /// <summary>
      /// Allows us to do an Action "count" times
      /// </summary>
      /// <param name="count">The number of times to loop</param>
      /// <param name="action">The Action to perform in the loop</param>
      public static void Times (this int count, Action action)
      {
         for (int i = 0; i < count; i++)
         {
            action ();
         }
      }

      /// <summary>
      /// Allows us to increment "count" times and use the value of "count" in the Action
      /// </summary>
      /// <param name="count"></param>
      /// <param name="action"></param>
      public static void Times (this int count, Action<int> action)
      {
         for (int i = 0; i < count; i++)
         {
            action (i);
         }
      }

      #endregion Public Methods
   }
}