//-----------------------------------------------------------------------
// <copyright file = "RandomNumberSourceTests.cs" company = "Me">
//     Copyright (c) Me!
// </copyright>
//-----------------------------------------------------------------------

namespace FreyjaTests
{
    using FreyjaLib;
    using NUnit.Framework;

    [TestFixture]
    public class RandomNumberSourceTests
    {
        /// <summary>
        /// Tests that the maxValue overload of GetNextInt returns an integer equal to or less than the passed-in value
        /// </summary>
        [Test]
        public void GetNextIntReturnsAnIntegerLessThanOrEqualToMaxValue()
        {
            var aNumber = RandomNumberSource.GetNextInt(5);
            Assert.IsInstanceOf<int>(aNumber);
            Assert.That(aNumber, Is.LessThanOrEqualTo(5));
        }

        /// <summary>
        /// Tests that the min/max overload of GetNextInt returns a value between minValue and maxValue
        /// </summary>
        [Test]
        public void GetNextIntReturnsAnIntegerBetweenMinValueAndMaxValue()
        {
            var aNumber = RandomNumberSource.GetNextInt(2, 5);
            Assert.IsInstanceOf<int>(aNumber);
            Assert.That(aNumber, Is.GreaterThanOrEqualTo(2));
            Assert.That(aNumber, Is.LessThanOrEqualTo(5));
        }
    }
}
