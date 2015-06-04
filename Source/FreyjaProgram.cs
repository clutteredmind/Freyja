//-----------------------------------------------------------------------
// <copyright file = "FreyjaProgram.cs" company = "Me">
//     Copyright (c) Me!
// </copyright>
//-----------------------------------------------------------------------

namespace Freyja
{
   using System;
   using System.Windows.Forms;

   static class FreyjaProgram
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main ()
      {
         Application.EnableVisualStyles ();
         Application.SetCompatibleTextRenderingDefault (false);
         Application.Run (new FreyjaForm ());
      }
   }
}
