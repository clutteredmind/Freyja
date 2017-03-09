//-----------------------------------------------------------------------
// <copyright file = "PlayerTests.cs" company = "Me">
//     Copyright (c) Me!
// </copyright>
//-----------------------------------------------------------------------

namespace FreyjaTests
{
    using NUnit.Framework;
    using FreyjaLib;

    /// <summary>
    /// Tests for the Player class
    /// </summary>
    [TestFixture]
    public class PlayerTests
    {
        /// <summary>
        /// A player to use for testing
        /// </summary>
        private Player player;

        /// <summary>
        /// Performs setup for the tests
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            player = new Player();
        }

        /// <summary>
        /// Tests that the player's level is an integer greater than or equal to 0
        /// </summary>
        [Test]
        public void LevelIsGettable()
        {
            Assert.IsInstanceOf<int>(player.Level);
            Assert.That(player.Level, Is.GreaterThanOrEqualTo(0));
        }

        /// <summary>
        /// Tests that MaximumHitPoints is an integer greater than or equal to 1
        /// </summary>
        [Test]
        public void MaximumHitPointsIsGettable()
        {
            Assert.IsInstanceOf<int>(player.MaximumHitPoints);
            Assert.That(player.MaximumHitPoints, Is.GreaterThanOrEqualTo(1));
        }

        /// <summary>
        /// Tests that CurrentHitPoints is an integer greater than or equal to 1
        /// </summary>
        [Test]
        public void CurrentHitPointsIsGettable()
        {
            Assert.IsInstanceOf<int>(player.CurrentHitPoints);
            Assert.That(player.CurrentHitPoints, Is.GreaterThanOrEqualTo(1));
        }

        /// <summary>
        /// Tests that Xp is an integer greater than or equal to 0
        /// </summary>
        [Test]
        public void XpIsGettable()
        {
            Assert.IsInstanceOf<int>(player.Xp);
            Assert.That(player.Xp, Is.GreaterThanOrEqualTo(0));
        }

        /// <summary>
        /// Tests that XpToNextLevel is an integer greater than or equal to 0
        /// </summary>
        [Test]
        public void XpToNextLevelIsGettable()
        {
            Assert.IsInstanceOf<int>(player.XpToNextLevel);
            Assert.That(player.XpToNextLevel, Is.GreaterThanOrEqualTo(0));
        }

        /// <summary>
        /// Tests that a player is created with certain initial values
        /// </summary>
        [Test]
        public void ConstructorShouldCreateAPlayer()
        {
            var newPlayer = new Player();
            Assert.AreEqual(0, newPlayer.Level);
            Assert.AreEqual(0, newPlayer.Xp);
            Assert.AreEqual(100, newPlayer.MaximumHitPoints);
            Assert.AreEqual(100, newPlayer.CurrentHitPoints);
            Assert.AreEqual(100, newPlayer.XpToNextLevel);
        }

        /// <summary>
        /// Tests that Attack() returns a non-zero integer
        /// </summary>
        [Test]
        public void AttackShouldReturnAnInteger()
        {
            var damage = player.Attack();
            Assert.IsInstanceOf<int>(damage);
            Assert.That(damage, Is.GreaterThanOrEqualTo(1));
        }

        /// <summary>
        /// Tests that TakeDamage() decreases the player's health by the correct amount
        /// </summary>
        [Test]
        public void TakeDamageDecreasesPlayerHealth()
        {
            var newPlayer = new Player();
            var damage = 25;
            Assert.AreEqual(100, newPlayer.CurrentHitPoints);
            newPlayer.TakeDamage(damage);
            Assert.AreEqual(75, newPlayer.CurrentHitPoints);
        }

        /// <summary>
        /// Tests that Heal() heals the player by the correct amount
        /// </summary>
        [Test]
        public void HealShouldHealThePlayer()
        {
            var newPlayer = new Player();
            var damage = 25;
            Assert.AreEqual(100, newPlayer.CurrentHitPoints);
            newPlayer.TakeDamage(damage);
            Assert.AreEqual(75, newPlayer.CurrentHitPoints);
            newPlayer.Heal(10);
            Assert.AreEqual(85, newPlayer.CurrentHitPoints);
        }

        /// <summary>
        /// Tests that the player can't be healed beyond MaximumHitPoints
        /// </summary>
        [Test]
        public void HealCannotHealThePlayerBeyondMaximumHitPoints()
        {
            var newPlayer = new Player();
            var damage = 25;
            Assert.AreEqual(100, newPlayer.CurrentHitPoints);
            newPlayer.TakeDamage(damage);
            Assert.AreEqual(75, newPlayer.CurrentHitPoints);
            // heal by an absurd amount
            newPlayer.Heal(100000);
            Assert.AreEqual(100, newPlayer.CurrentHitPoints);
        }

        /// <summary>
        /// Tests that AwardXp increases the player's XP
        /// </summary>
        [Test]
        public void AwardXpIncreasesPlayerXp()
        {
            var newPlayer = new Player();
            Assert.AreEqual(0, newPlayer.Xp);
            newPlayer.AwardXp(25);
            Assert.AreEqual(25, newPlayer.Xp);
        }

        /// <summary>
        /// Tests that the player can level up if awarded enough XP
        /// </summary>
        [Test]
        public void AwardXpWillIncreasePlayerLevel()
        {
            var newPlayer = new Player();
            Assert.AreEqual(0, newPlayer.Xp);
            Assert.AreEqual(0, newPlayer.Level);
            newPlayer.AwardXp(100);
            Assert.AreEqual(100, newPlayer.Xp);
            Assert.AreEqual(1, newPlayer.Level);
        }
    }
}
