//-----------------------------------------------------------------------
// <copyright file = "Player.cs" company = "Me">
//     Copyright (c) Me!
// </copyright>
//-----------------------------------------------------------------------

namespace FreyjaLib
{
    using System;

    /// <summary>
    /// Represents the player in the game
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The base XP requirement. This is the amount that is required to go from level 0 to level 1, and is the value that
        /// will be used for calculating the XP requirement for the player's next level
        /// </summary>
        private const int BaseXpRequirement = 100;

        /// <summary>
        /// The scale factor used in calculating each level's XP requirement
        /// </summary>
        private const double GeometricSequenceScaleFactor = 1.1;

        /// <summary>
        /// The player's level
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// The player's maximum health
        /// </summary>
        public int MaximumHitPoints { get; private set; }
        
        /// <summary>
        /// The player's current health
        /// </summary>
        public int CurrentHitPoints { get; private set; }
        
        /// <summary>
        /// The player's current XP
        /// </summary>
        public int Xp { get; private set; }

        /// <summary>
        /// The amount of XP required to get to the next level
        /// </summary>
        public int XpToNextLevel { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Player()
        {
            /// The player starts at level 0
            Level = 0;
            // set maximum and current hitpoints
            MaximumHitPoints = CurrentHitPoints = 100;
            // the player starts with 0 XP
            Xp = 0;
            // set the XP required for the next level
            XpToNextLevel = BaseXpRequirement;
        }

        /// <summary>
        /// Calculates and returns the XP required for the player's level
        /// </summary>
        /// <returns>The XP required for the player's new level</returns>
        private double GetXpForLevel()
        {
            // calculate the XP required.
            // This is a geometric progression
            return BaseXpRequirement * (1 - Math.Pow(GeometricSequenceScaleFactor, Level + 1)) / (1 - GeometricSequenceScaleFactor);
        }

        /// <summary>
        /// Allows the player to do damage
        /// </summary>
        /// <returns>The amount of damage done by the player</returns>
        public int Attack()
        {
            return RandomNumberSource.GetNextInt(1, 4) * (Level == 0 ? 1 : Level);
        }

        /// <summary>
        /// Does damage to the player
        /// </summary>
        /// <returns>The amount of damage that the player has taken</returns>
        public void TakeDamage(int damage)
        {
            CurrentHitPoints -= damage;
            // the player can't die
            if(CurrentHitPoints <= 0)
            {
                CurrentHitPoints = 1;
            }
        }

        /// <summary>
        /// Allows the player to be healed
        /// </summary>
        /// <param name="healAmount">The anount of healing done to the player</param>
        public void Heal(int healAmount)
        {
            CurrentHitPoints += healAmount;
            if(CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }
        }

        /// <summary>
        /// Awards XP to the player
        /// </summary>
        /// <param name="xpAward">The amount of XP that the player has earned</param>
        public void AwardXp(int xpAward)
        {
            Xp += xpAward;
            XpToNextLevel -= xpAward;
            if(XpToNextLevel <= 0)
            {
                // the player has leveled up
                Level++;
                // the player earns 1-10 * 2 additional hit points
                var additionalHitPoints = RandomNumberSource.GetNextInt(1, 10) * 2;
                MaximumHitPoints += additionalHitPoints;
                // make the new health immediately available
                CurrentHitPoints += additionalHitPoints;
                XpToNextLevel = (int)GetXpForLevel();
            }
        }
    }
}
