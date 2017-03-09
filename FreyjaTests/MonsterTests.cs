//-----------------------------------------------------------------------
// <copyright file = "MonsterTests.cs" company = "Me">
//     Copyright (c) Me!
// </copyright>
//-----------------------------------------------------------------------

namespace FreyjaTests
{
    using FreyjaLib;
    using NUnit.Framework;

    /// <summary>
    /// Tests for the Monster class
    /// </summary>
    [TestFixture]
    public class MonsterTests
    {
        /// <summary>
        /// The level of the player with which we will test
        /// </summary>
        private const int TestPlayerLevel = 1;
        /// <summary>
        /// A monster to use for testing
        /// </summary>
        private Monster monster;

        /// <summary>
        /// Creates a monster with which to test
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            monster = new Monster(TestPlayerLevel);
        }

        /// <summary>
        /// Tests that a new monster is alive
        /// </summary>
        [Test]
        public void NewMonsterShouldBeAlive()
        {
            Assert.IsFalse(monster.IsDead);
        }

        /// <summary>
        /// Tests that CurrentMonsterHitPoints is gettable and is an integer
        /// </summary>
        [Test]
        public void CurrentMonsterHitPointsIsGettable()
        {
            var hp = monster.CurrentMonsterHitPoints;
            Assert.IsInstanceOf<int>(hp);
        }

        /// <summary>
        /// Tests that Descriptor is a non-null string
        /// </summary>
        [Test]
        public void DescriptorIsStringAndNotNull()
        {
            Assert.IsFalse(string.IsNullOrEmpty(monster.Descriptor));
            Assert.IsInstanceOf<string>(monster.Descriptor);
        }

        /// <summary>
        /// Tests that FullName is a non-null string
        /// </summary>
        [Test]
        public void FullNameIsStringAndNotNull()
        {
            Assert.IsFalse(string.IsNullOrEmpty(monster.FullName));
            Assert.IsInstanceOf<string>(monster.FullName);
        }

        /// <summary>
        /// Tests that maximum hit points > 0 and is an integer
        /// </summary>
        [Test]
        public void MaximumMonsterHitPointsIsNotZeroAndIsInteger()
        {
            Assert.AreNotEqual(0, monster.MaximumMonsterHitPoints);
            Assert.IsInstanceOf<int>(monster.MaximumMonsterHitPoints);
        }

        /// <summary>
        /// Tests that Attack() returns an integer greater than 0
        /// </summary>
        [Test]
        public void AttackReturnsAnIntegerGreaterThanZero()
        {
            var damage = monster.Attack();
            Assert.IsInstanceOf<int>(damage);
            Assert.That(damage, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tests that MaximumMonsterHitPoints returns an integer
        /// </summary>
        [Test]
        public void MaximumMonsterHitPointsReturnsAnInteger()
        {
            Assert.IsInstanceOf<int>(monster.MaximumMonsterHitPoints);
        }

        /// <summary>
        /// Tests that MaximumMonsterHitPoints returns an integer
        /// </summary>
        [Test]
        public void MonsterLevelReturnsAnInteger()
        {
            Assert.IsInstanceOf<int>(monster.MonsterLevel);
        }

        /// <summary>
        /// Tests that MonsterType returns a string
        /// </summary>
        [Test]
        public void MonsterTypeReturnsAString()
        {
            Assert.IsInstanceOf<string>(monster.MonsterType);
        }

        /// <summary>
        /// Tests that new monsters have a level within two of the player's
        /// </summary>
        [Test]
        public void NewMonsterLevelShouldBeWithTwoLevelsOfPlayer()
        {
            var newMonster = new Monster(4);
            Assert.That(newMonster.MonsterLevel, Is.GreaterThanOrEqualTo(2));
            Assert.That(newMonster.MonsterLevel, Is.LessThanOrEqualTo(6));
        }

        /// <summary>
        /// Tests that a monster can take damage
        /// </summary>
        [Test]
        public void MonsterCanTakeDamage()
        {
            // choose a higher level for the player so we can be sure the monster gets sufficient hit points for the test
            var newMonster = new Monster(10);
            Assert.That(newMonster.CurrentMonsterHitPoints, Is.GreaterThanOrEqualTo(2));
            newMonster.TakeDamage(1);
            Assert.That(newMonster.CurrentMonsterHitPoints, Is.LessThan(newMonster.MaximumMonsterHitPoints));
        }
    }
}
