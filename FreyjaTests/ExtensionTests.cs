//-----------------------------------------------------------------------
// <copyright file = "FreyjaTests.cs" company = "Me">
//     Copyright (c) Me!
// </copyright>
//-----------------------------------------------------------------------

namespace FreyjaTests
{
    using FreyjaLib;
    using NUnit.Framework;

    /// <summary>
    /// Tests for the extension methods in Extensions.cs
    /// </summary>
    [TestFixture]
    public class ExtensionTests
    {
        [Test]
        public void TimesCanAddNTimes()
        {
            var aNumber = 5;
            aNumber.Times(() => aNumber++);
            Assert.That(aNumber, Is.EqualTo(10));
        }
    }
}
