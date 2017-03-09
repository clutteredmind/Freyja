//-----------------------------------------------------------------------
// <copyright file = "Monster.cs" company = "Me">
//     Copyright (c) Me!
// </copyright>
//-----------------------------------------------------------------------

namespace FreyjaLib
{
    using System;
    using System.Collections.Generic;

    public class Monster
    {
        #region Private Fields

        /// <summary>
        /// A source of random numbers
        /// </summary>
        private readonly static Random RandomNumberSource = new Random();

        /// <summary>
        /// I don't want the same descriptor to come up twice in a row, because I think that looks cheesy
        /// </summary>
        private static string lastDescriptor = "";

        /// <summary>
        /// As with the descriptor, I don't want the same monster coming up twice in a row
        /// </summary>
        private static string lastMonsterType = "";

        /// <summary>
        /// A list of silly monster descriptors
        /// </summary>
        private readonly List<string> descriptors = new List<string>
                                                     {
                                                        "Angsty", "Apathetic", "Arrogant", "Belligerent", "Bland", "Bloviating",
                                                        "Bookish", "Bossy", "Canadian", "Cantankerous", "Chapped", "Churlish",
                                                        "Clumsy", "Conservative", "Constipated", "Crazed", "Cruel", "Cunning",
                                                        "Cynical", "Disaffected", "Disagreeable", "Drunk", "Ethereal", "Evil",
                                                        "Excitable", "Fastidious", "Flatulent", "Foolish", "Forgetful",
                                                        "Foul-mouthed", "Frostbitten", "Greedy", "Grotesque", "Grumpy",
                                                        "Half-dead", "Hirsute", "Hoary", "Hypercritical", "Impatient",
                                                        "Impolite", "Incurious", "Inexplicable", "Itchy", "Jealous", "Lazy",
                                                        "Left-handed", "Libertarian", "Lost", "Malingering", "Mansplaining",
                                                        "Mean", "Messy", "Misanthropic", "Misunderstood", "Monstrous", "Moronic",
                                                        "Murderous", "Nauseous", "Nearsighted", "Nervous", "Obnoxious", "Odd",
                                                        "Off-putting", "Otherworldly", "Overbearing", "Patronizing", "Peculiar",
                                                        "Piebald", "Playful", "Proselytizing", "Ravening", "Raving", "Ridiculous",
                                                        "Ruthless", "Selfish", "Shady", "Shifty", "Sickly", "Sleeping", "Sleepy",
                                                        "Slimy", "Sloppy", "Slovenly", "Sneezing", "Snobby", "Spazzy", "Stubborn",
                                                        "Stupid", "Sunburned", "Supercilious", "Tactless", "Thoughtless",
                                                        "Truculent", "Two-faced", "Ugly", "Ungainly", "Unreliable", "Untidy",
                                                        "Untrustworthy", "Vain", "Vulgar", "Wary", "Weasly", "Xenophobic"
                                                     };

        /// <summary>
        /// A list of monster types
        /// A lot of this list was cribbed from the 1974 D&D list of monsters, which I got from Wikipedia. Some are just things I thought would be funny.
        /// </summary>
        private readonly List<string> monsterTypes = new List<string>
                                                      {
                                                         "Air Elemental", "Badger", "Bandit", "Basilisk", "Bat", "Bear", "Black Pudding",
                                                         "Blink Dog", "Brontosaurus", "Carnivorous Bunny", "Catoblepas", "Centaur",
                                                         "Charlatan", "Chimera", "Clown", "Demon", "Dervish", "Devil", "Displacer Beast",
                                                         "Djinn", "Djinn", "Donald Trump", "Dopplegänger", "Dragon", "Dryad",
                                                         "Earth Elemental", "Fire Elemental", "Ghast", "Ghoul", "Giant", "Gnoll",
                                                         "Goblin", "Golem", "Goth", "Green Slime", "Grey Ooze", "Griffin", "Hobgoblin",
                                                         "Homunculus", "Hydra", "Incubus", "Jester", "Kobold", "Kraken", "Lich",
                                                         "Lobster", "Manticore", "Mind Flayer", "Minotaur", "Mummy", "Nazi",
                                                         "Nickleback Fan", "Ogre", "Orc", "Owlbear", "Pick-up Artist", "Pixie", "Poet",
                                                         "Robot", "Rust Monster", "Sasquatch", "Serpent", "Skeleton", "Slaver",
                                                         "Spectre", "Spider", "Succubus", "Time Elemental", "Treant", "Troll", "Twit",
                                                         "Vampire", "Water Elemental", "Wight", "Wolf", "Wraith", "Wyvern", "Yeti",
                                                         "Zealot",  "Zombie"
                                                      };

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Creates a new monster
        /// </summary>
        /// <param name="playerLevel">The monster will be within two levels of the player when it is created</param>
        public Monster(int playerLevel)
        {
            // set descriptor
            Descriptor = descriptors[RandomNumberSource.Next(descriptors.Count)];
            while (Descriptor == lastDescriptor)
            {
                Descriptor = descriptors[RandomNumberSource.Next(descriptors.Count)];
            }
            // remember descriptor
            lastDescriptor = Descriptor;
            // set type
            MonsterType = monsterTypes[RandomNumberSource.Next(monsterTypes.Count)];
            while (MonsterType == lastMonsterType)
            {
                MonsterType = monsterTypes[RandomNumberSource.Next(monsterTypes.Count)];
            }
            // remember monster type
            lastMonsterType = MonsterType;
            // set level
            MonsterLevel = RandomNumberSource.Next(playerLevel >= 2 ? playerLevel - 2 : 0, playerLevel + 2);
            // it's not dead yet!
            IsDead = false;
            // calculate hit points
            MaximumMonsterHitPoints = 0;
            // using a fancy Ruby-like extension method here to calculate monster hit points
            MonsterLevel.Times(() => MaximumMonsterHitPoints += RandomNumberSource.Next(1, 10));
            // current health is the same as maximum, to start
            CurrentMonsterHitPoints = MaximumMonsterHitPoints;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// The monster's hit points. Set at creation.
        /// </summary>
        public int CurrentMonsterHitPoints { get; private set; }

        /// <summary>
        /// A descriptor for the monster. Set at creation.
        /// </summary>
        public string Descriptor { get; private set; }

        /// <summary>
        /// The monster's full name
        /// </summary>
        public string FullName
        {
            get
            {
                return Descriptor + " " + MonsterType;
            }
        }

        /// <summary>
        /// Monsters don't die of old age, so this flag indicates whether or not the monster has reached its untimely end
        /// </summary>
        public bool IsDead { get; private set; }

        /// <summary>
        /// The monster's hit points. Set at creation.
        /// </summary>
        public int MaximumMonsterHitPoints { get; private set; }

        /// <summary>
        /// The monster's level. Will be within 2 levels of the player's level
        /// </summary>
        public int MonsterLevel { get; private set; }

        /// <summary>
        /// The type of this monster. Set at creation.
        /// </summary>
        public string MonsterType { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Allows the monster to do damage to the player
        /// </summary>
        /// <returns>The amount of damage that the monster did</returns>
        public int Attack()
        {
            // the monster always does ((1-4) * (level/2)) damage (where level is always >= 1)
            return (RandomNumberSource.Next(1, 4) * ((MonsterLevel / 2 < 1 ? 1 : MonsterLevel / 2)));
        }

        /// <summary>
        /// Allows the player to do damage to the monster
        /// </summary>
        /// <param name="damageAmount">The amount of damage to do to the monster</param>
        public void TakeDamage(int damageAmount)
        {
            CurrentMonsterHitPoints -= damageAmount;
            if (CurrentMonsterHitPoints <= 0)
            {
                // make sure the final hit points end up at zero instead of a negative number
                CurrentMonsterHitPoints = 0;
                IsDead = true;
            }
        }

        #endregion Public Methods
    }
}
