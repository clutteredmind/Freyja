//-----------------------------------------------------------------------
// <copyright file = "FreyjaForm.cs" company = "Me">
//     Copyright (c) Me!
// </copyright>
//-----------------------------------------------------------------------

namespace Freyja
{
    using FreyjaLib;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;
    using Timer = System.Timers.Timer;

    /// <summary>
    /// The main form for Freyja
    /// </summary>
    public partial class FreyjaForm : Form
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
        /// A place to store the last encounter verb, so it doesn't get used twice in a row
        /// </summary>
        private static string lastEncounterVerb = "";

        /// <summary>
        /// A thread to use for timing HP recovery
        /// </summary>
        private readonly BackgroundWorker hpRecoveryTimerThread;

        /// <summary>
        /// A list of verbs to use when the player goes looking for trouble
        /// </summary>
        private readonly List<string> lookingForTroubleVerbs = new List<string>
                                                                {
                                                                   "angered",
                                                                   "blundered into",
                                                                   "cheesed off",
                                                                   "encountered",
                                                                   "insulted",
                                                                   "lost a bet with",
                                                                   "mocked",
                                                                   "perturbed",
                                                                   "run afoul of",
                                                                   "run smack into",
                                                                   "startled",
                                                                   "stumbled upon",
                                                                   "swindled",
                                                                   "tripped over",
                                                                   "woken"
                                                                };

        /// <summary>
        /// A thread to use for the drink potion timer
        /// </summary>
        private readonly BackgroundWorker potionRefillTimerThread;

        /// <summary>
        /// A random number source
        /// </summary>
        private readonly Random randomNumberSource = new Random();

        /// <summary>
        /// A wait handle used by the HP recovery thread to count seconds
        /// </summary>
        private readonly AutoResetEvent waitingForHpRecoveryTimer = new AutoResetEvent(false);

        /// <summary>
        /// A wait handle used by the potion recovery thread to count seconds
        /// </summary>
        private readonly AutoResetEvent waitingForPotionRefillTimer = new AutoResetEvent(false);

        /// <summary>
        /// The monster that the player is currently fighting
        /// </summary>
        private Monster currentMonster;

        /// <summary>
        /// The time between HP recoveries
        /// </summary>
        private TimeSpan hpRecoveryInterval = new TimeSpan(0/*days*/, 0/*hours*/, 0/*minutes*/, 30/*seconds*/);

        /// <summary>
        /// The player's current hit points
        /// </summary>
        private int playerCurrentHitPoints;

        /// <summary>
        /// The player's current level
        /// </summary>
        private int playerLevel;

        /// <summary>
        /// The player's maximum hit points
        /// </summary>
        private int playerMaximumHitPoints;

        /// <summary>
        /// The player's total XP
        /// </summary>
        private int playerTotalXp;

        /// <summary>
        /// The amount of XP the player needs to get to the next level
        /// </summary>
        private int playerXpToNextLevel;

        /// <summary>
        /// The time between potion uses
        /// </summary>
        private TimeSpan potionRefillInterval = new TimeSpan(0/*days*/, 1/*hours*/, 0/*minutes*/, 0/*seconds*/);

        /// <summary>
        /// Default constructor
        /// </summary>
        public FreyjaForm()
        {
            // initialize player values
            playerLevel = 0;
            playerMaximumHitPoints = 100;
            playerCurrentHitPoints = playerMaximumHitPoints;
            playerXpToNextLevel = BaseXpRequirement;
            playerTotalXp = 0;
            // set up HP recovery timer thread
            hpRecoveryTimerThread = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            hpRecoveryTimerThread.DoWork += HpRecoveryTimerThreadDoWork;
            hpRecoveryTimerThread.RunWorkerCompleted += HpRecoveryTimerThreadRunWorkerCompleted;
            // set up potion timer thread
            potionRefillTimerThread = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            potionRefillTimerThread.DoWork += PotionRefillTimerThreadDoWork;
            potionRefillTimerThread.RunWorkerCompleted += PotionRefillTimerThreadRunWorkerCompleted;
            // set up the form and its controls
            InitializeComponent();
        }

        /// <summary>
        /// Adds an item to the Journal listbox
        /// </summary>
        /// <param name="newEntry">The text of the new journal entry</param>
        private void AddEntryToJournal(string newEntry)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => AddEntryToJournal(newEntry)));
            }
            else
            {
                // don't do anything if the form is disposed (which might happen if the application is closed just before this code executes)
                if (!IsDisposed)
                {
                    listBoxAdventureJournal.Items.Add(newEntry);
                    // if the journal has gotten too long, drop the oldest item
                    if (listBoxAdventureJournal.Items.Count > 1000)
                    {
                        listBoxAdventureJournal.Items.RemoveAt(0);
                    }
                    // scroll to bottom
                    listBoxAdventureJournal.SelectedIndex = listBoxAdventureJournal.Items.Count - 1;
                    // this clears the selected item in the box, so that the bottom line is not highlighted in blue after scroll to the bottom
                    // I just think it looks better this way
                    listBoxAdventureJournal.ClearSelected();
                }
            }
        }

        /// <summary>
        /// Recovers hit points for the player over time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="doWorkEventArgs"></param>
        private void HpRecoveryTimerThreadDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            // a timer that ticks once per second
            using (var hpTimer = new Timer(1000))
            {
                hpTimer.Elapsed += HpTimerElapsed;
                // create and start a stopwatch
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                // enable and start the timer
                hpTimer.Enabled = true;
                hpTimer.Start();
                // fills up the progress bar as time passes between HP recoveries
                while ((stopWatch.ElapsedMilliseconds < hpRecoveryInterval.TotalMilliseconds) && !((BackgroundWorker)sender).CancellationPending)
                {
                    // wait a mo to give some processor time to the rest of the application
                    waitingForHpRecoveryTimer.WaitOne(10000);
                    // update the GUI
                    UpdateGui((int)((stopWatch.ElapsedMilliseconds / hpRecoveryInterval.TotalMilliseconds) * progressBarHpRecoveryProgress.Maximum));
                }
                // stop timer
                hpTimer.Stop();
            }
        }

        /// <summary>
        /// Increments the player's health and updates the GUI when the thread has finished its work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="runWorkerCompletedEventArgs"></param>
        private void HpRecoveryTimerThreadRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            // only increment the player's hit points if they are below his or her maximum
            if (playerCurrentHitPoints < playerMaximumHitPoints)
            {
                int increment = (playerMaximumHitPoints - playerCurrentHitPoints >= 5) ? 5 : playerMaximumHitPoints - playerCurrentHitPoints;
                playerCurrentHitPoints += increment;
            }
            // the progress bar will be full when this event fires, so we'll zero that out
            UpdateGui(0);
        }

        /// <summary>
        /// Signals the hp recovery thread to continue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void HpTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            waitingForHpRecoveryTimer.Set();
        }

        /// <summary>
        /// Counts down until the user can use the potion again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="doWorkEventArgs"></param>
        private void PotionRefillTimerThreadDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            // a timer that ticks once per second
            using (var potionTimer = new Timer(1000))
            {
                potionTimer.Elapsed += PotionTimerElapsed;
                // create and start a stopwatch
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                // enable and start the timer
                potionTimer.Enabled = true;
                potionTimer.Start();
                // fills up the progress bar as time passes between HP recoveries
                while ((stopWatch.ElapsedMilliseconds < potionRefillInterval.TotalMilliseconds) && !((BackgroundWorker)sender).CancellationPending)
                {
                    // wait a mo to give some processor time to the rest of the application
                    waitingForPotionRefillTimer.WaitOne(10000);
                    // update the GUI
                    UpdateGui(-1, (int)((stopWatch.ElapsedMilliseconds / potionRefillInterval.TotalMilliseconds) * progressBarPotionRefillTimer.Maximum));
                }
                // stop timer
                potionTimer.Stop();
            }
        }

        /// <summary>
        /// Updates the GUI when the potion timer runs out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="runWorkerCompletedEventArgs"></param>
        private void PotionRefillTimerThreadRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            AddEntryToJournal("Your potion has refilled!");
            // clear the potion progress bar, while leaving the other one alone
            UpdateGui(-1, 0);
        }

        /// <summary>
        /// Signals the potion timer thread to continue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void PotionTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            waitingForPotionRefillTimer.Set();
        }

        /// <summary>
        /// Updates the GUI to enable/disable buttons, set progress bar values, etc.
        /// </summary>
        private void UpdateGui(int hpRecoveryProgressBarValue = -1, int potionRefillProgressBarValue = -1)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => UpdateGui(hpRecoveryProgressBarValue, potionRefillProgressBarValue)));
            }
            else
            {
                // don't do anything if the form is disposed (which might happen if the application is closed just before this code executes)
                if (!IsDisposed)
                {
                    // only update the progress bar if it is not at the default value
                    // this keeps it from resetting to zero and then immediately returning to the correct value when the timer thread fires off an update
                    if (hpRecoveryProgressBarValue != -1)
                    {
                        // bounds-check the new value
                        if (hpRecoveryProgressBarValue > progressBarHpRecoveryProgress.Maximum)
                        {
                            hpRecoveryProgressBarValue = progressBarHpRecoveryProgress.Maximum;
                        }
                        else if (hpRecoveryProgressBarValue < progressBarHpRecoveryProgress.Minimum)
                        {
                            hpRecoveryProgressBarValue = progressBarHpRecoveryProgress.Minimum;
                        }
                        // set hp recovery progress bar's value
                        progressBarHpRecoveryProgress.Value = hpRecoveryProgressBarValue;
                    }
                    if (potionRefillProgressBarValue != -1)
                    {
                        // bounds-check the new value
                        if (potionRefillProgressBarValue > progressBarPotionRefillTimer.Maximum)
                        {
                            potionRefillProgressBarValue = progressBarPotionRefillTimer.Maximum;
                        }
                        else if (potionRefillProgressBarValue < progressBarPotionRefillTimer.Minimum)
                        {
                            potionRefillProgressBarValue = progressBarPotionRefillTimer.Minimum;
                        }
                        // set potion refill progress bar's value
                        progressBarPotionRefillTimer.Value = potionRefillProgressBarValue;
                    }
                    // decide whether or not we should be running the HP timer
                    if (playerCurrentHitPoints < playerMaximumHitPoints && !hpRecoveryTimerThread.IsBusy)
                    {
                        // the player's hit points are below maximum and the recovery thread is not running, so start it
                        hpRecoveryTimerThread.RunWorkerAsync();
                    }
                    else if (playerCurrentHitPoints == playerMaximumHitPoints && hpRecoveryTimerThread.IsBusy)
                    {
                        // the player's hit points are completely recovered, but the recovery thread is still running
                        // honestly, this should never happen and this else clause might be over-engineering
                        hpRecoveryTimerThread.CancelAsync();
                    }
                    // update the hit points display
                    labelPlayerHitPointsDisplay.Text = $"{playerCurrentHitPoints}/{playerMaximumHitPoints}";
                    // display level
                    labelPlayerLevelDisplay.Text = playerLevel.ToString();
                    // update XP display
                    labelPlayerTotalXpDisplay.Text = playerTotalXp.ToString();
                    labelPlayerXpToNextLevelDisplay.Text = playerXpToNextLevel.ToString();
                    // the attack and flee buttons should be enabled if there is an encounter happening
                    // and the look for trouble button should be disabled if there is a current monster
                    if (currentMonster != null)
                    {
                        buttonAttack.Enabled = true;
                        buttonFlee.Enabled = true;
                        // can't look for more trouble while currently dealing with a monster
                        buttonLookForTrouble.Enabled = false;
                    }
                    else
                    {
                        buttonAttack.Enabled = false;
                        buttonFlee.Enabled = false;
                        // the look for trouble button should only be enabled if the player has enough health to go looking for trouble
                        if (playerCurrentHitPoints > 1)
                        {
                            buttonLookForTrouble.Enabled = true;
                        }
                    }
                    // the drink potion button should be disabled if the player is at full health
                    buttonDrinkPotion.Enabled = playerCurrentHitPoints != playerMaximumHitPoints;
                    // decide which of the potion button and potion progress bar should be visible
                    if (potionRefillTimerThread.IsBusy)
                    {
                        progressBarPotionRefillTimer.Visible = true;
                        buttonDrinkPotion.Visible = false;
                    }
                    else
                    {
                        progressBarPotionRefillTimer.Visible = false;
                        buttonDrinkPotion.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates and returns the XP required for the player's level
        /// </summary>
        /// <param name="playerLevel">The level of the player for which the XP requirement must be calculated</param>
        /// <returns>The XP required for the player's new level</returns>
        private static double GetXpForLevel(int playerLevel)
        {
            // calculate the XP required.
            // This is a geometric progression
            return BaseXpRequirement * (1 - Math.Pow(GeometricSequenceScaleFactor, playerLevel + 1)) / (1 - GeometricSequenceScaleFactor);
        }

        /// <summary>
        /// Adds a line to the monsters defeated log
        /// </summary>
        /// <param name="newEntry">The text of the new monster defeated entry. A newline will be appended to this string.</param>
        private void AddMonsterToLog(string newEntry)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => AddMonsterToLog(newEntry)));
            }
            else
            {
                // don't do anything if the form is disposed (which might happen if the application is closed just before this code executes)
                if (!IsDisposed)
                {
                    richTextBoxMonsterLog.Text += $"{newEntry}\n";
                    // normally, one would use "richTextBoxMonstersDefeated.SelectionStart = richTextBoxMonstersDefeated.Text.Length" here
                    // before ScrollToCaret. But since our lines are sometimes longer than the text box is wide, that makes things scroll
                    // horizontally as well as vertically and I think that looks pretty cheesy. So instead, we're going to move the caret
                    // to the beginning of the last line added and scroll to that
                    richTextBoxMonsterLog.SelectionStart = richTextBoxMonsterLog.Text.Length - newEntry.Length - 1;
                    richTextBoxMonsterLog.ScrollToCaret();
                }
            }
        }

        /// <summary>
        /// Attacks the current Monster
        /// </summary>
        private void AttackMonster()
        {
            if (currentMonster != null)
            {
                // the player always does ((1-4) * level) damage, where level is always at least 1
                var playerAttackResult = randomNumberSource.Next(1, 4) * (playerLevel == 0 ? 1 : playerLevel);
                // the player always attacks first
                currentMonster.TakeDamage(playerAttackResult);
                AddEntryToJournal(string.Format("Your attack did {0} point{1} of damage to the {2}!", playerAttackResult, playerAttackResult > 1 ? "s" : "", currentMonster.FullName));
                // if that didn't defeat the monster, it gets to counterattack
                if (!currentMonster.IsDead)
                {
                    AddEntryToJournal(string.Format("The {0} has {1} hit point{2} left.", currentMonster.FullName, currentMonster.CurrentMonsterHitPoints,
                                                    currentMonster.CurrentMonsterHitPoints > 1 ? "s" : ""));
                    var monsterAttackDamage = currentMonster.Attack();
                    // the player is not allowed to die
                    if (monsterAttackDamage >= playerCurrentHitPoints)
                    {
                        monsterAttackDamage = playerCurrentHitPoints - 1;
                    }
                    playerCurrentHitPoints -= monsterAttackDamage;
                    // check to see if we need to flee from the monster
                    // sometimes, the above math results in the monster doing 0 points of damage. that looks stupid when displayed, so we're going to cheat
                    // and make it always do at least one point of damage, even if it technically did no damage
                    if (monsterAttackDamage <= 0)
                    {
                        monsterAttackDamage = 1;
                    }
                    if (playerCurrentHitPoints == 1)
                    {
                        AddEntryToJournal(string.Format("The {0} has gravely wounded you for {1} point{2} of damage, but you have managed to escape to fight another day.",
                                                        currentMonster.FullName, monsterAttackDamage, monsterAttackDamage > 1 ? "s" : ""));
                        // log the player's cowardly flight from danger
                        AddMonsterToLog($"Fled from a level {currentMonster.MonsterLevel} {currentMonster.FullName}");
                        // get rid of the monster
                        currentMonster = null;
                        // clear encounter text
                        SetEncounterText("");
                    }
                    else
                    {
                        AddEntryToJournal(string.Format("Ouch! The {0} attacked for {1} point{2} of damage!", currentMonster.FullName, monsterAttackDamage, monsterAttackDamage > 1 ? "s" : ""));
                    }
                }
                else
                {
                    // the player defeated the critter
                    AddEntryToJournal($"You've defeated the {currentMonster.FullName}!");
                    // award XP. The player gets 10 times the monster's level plus one XP for each hit point the monster had
                    var xpAward = (10 * currentMonster.MonsterLevel) + currentMonster.MaximumMonsterHitPoints;
                    AddEntryToJournal($"You earned {xpAward} XP for defeating the {currentMonster.FullName}");
                    playerTotalXp += xpAward;
                    // log the victory
                    var victoryString = $"You defeated a level {currentMonster.MonsterLevel} {currentMonster.FullName} with {currentMonster.MaximumMonsterHitPoints} HP ({xpAward} XP awarded)";
                    AddMonsterToLog(victoryString);
                    SetEncounterText(victoryString);
                    // decrement XP required for next level
                    playerXpToNextLevel -= xpAward;
                    if (playerXpToNextLevel <= 0)
                    {
                        // the player has leveled up
                        playerLevel++;
                        // add additional hit points
                        var additionalHitPoints = randomNumberSource.Next(1, 10) * 2;
                        AddEntryToJournal(string.Format("Congratulations! You are now level {0} and now have {1} additional hit point{2}.", playerLevel, additionalHitPoints,
                                                        additionalHitPoints > 1 ? "s" : ""));
                        playerMaximumHitPoints += additionalHitPoints;
                        // make the new HP immediately usable
                        playerCurrentHitPoints += additionalHitPoints;
                        // update XP requirement
                        playerXpToNextLevel += (int)GetXpForLevel(playerLevel);
                    }
                    // get rid of the dead monster
                    currentMonster = null;
                }
                UpdateGui();
            }
        }

        /// <summary>
        /// Attacks the current monster, if any
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ButtonAttackClick(object sender, EventArgs eventArgs)
        {
            AttackMonster();
        }

        /// <summary>
        /// Allows the user to flee the current critter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ButtonFleeClick(object sender, EventArgs eventArgs)
        {
            FleeFromMonster();
        }

        /// <summary>
        /// Lets the player go looking for trouble, that is, look for a monster to fight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ButtonLookForTroubleClick(object sender, EventArgs eventArgs)
        {
            LookForTrouble();
        }

        /// <summary>
        /// Allows the player to drink a poition and refill his/her health
        /// </summary>
        private void DrinkPotion()
        {
            playerCurrentHitPoints = playerMaximumHitPoints;
            AddEntryToJournal("Your health is fully recovered!");
            // start the potion recovery timer
            potionRefillTimerThread.RunWorkerAsync();
            // update the GUI, this will cause the HP recovery thread to be canceled
            UpdateGui();
        }

        /// <summary>
        /// Allows the player to flee from the current monster
        /// </summary>
        private void FleeFromMonster()
        {
            if (currentMonster != null)
            {
                // add a journal entry
                AddEntryToJournal($"You bravely ran away. Good thing, too. That {currentMonster.MonsterType} couldn't have been more {currentMonster.Descriptor.ToLower()}.");
                // log the monster from which the player fled
                AddMonsterToLog($"Fled from a level {currentMonster.MonsterLevel} {currentMonster.FullName}.");
                // get rid of the monster
                currentMonster = null;
                // clear encounter text
                SetEncounterText("");
                // update the GUI
                UpdateGui();
            }
        }

        /// <summary>
        /// Handler for the FormClosing event. Attempts to stop the HP recovery and potion refill threads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="formClosingEventArgs"></param>
        private void FreyjaFormFormClosing(object sender, FormClosingEventArgs formClosingEventArgs)
        {
            // check to see if the HP recovery thread is running
            if (hpRecoveryTimerThread.IsBusy)
            {
                // cancel the hp recovery timer thread
                hpRecoveryTimerThread.CancelAsync();
                // signal the corresponding reset event, in case the thread's waiting
                waitingForHpRecoveryTimer.Set();
            }
            // check to see if potion refill thread is running
            if (potionRefillTimerThread.IsBusy)
            {
                // cancel potion refill thread
                potionRefillTimerThread.CancelAsync();
                // signal the corresponding reset event, in case the thread's waiting
                waitingForPotionRefillTimer.Set();
            }
        }

        /// <summary>
        /// Intercepts certain key presses to allow the player to play without having to use the mouse to click buttons on the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="keyEventArgs"></param>
        private void FreyjaFormKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.KeyCode)
            {
                case Keys.A:
                    // attack
                    AttackMonster();
                    break;

                case Keys.D:
                    // drink potion, if the player's below his or her maximum health
                    if (playerCurrentHitPoints < playerMaximumHitPoints)
                    {
                        DrinkPotion();
                    }
                    break;

                case Keys.F:
                    // flee
                    FleeFromMonster();
                    break;

                case Keys.L:
                    // look for trouble
                    LookForTrouble();
                    break;

                case Keys.P:
                    // play
                    if (playerCurrentHitPoints == 1 && !potionRefillTimerThread.IsBusy)
                    {
                        // if the player's out of health and there's a potion to drink, drink it
                        DrinkPotion();
                    }
                    if (currentMonster == null && playerCurrentHitPoints > 1)
                    {
                        // if the player's not currently fighting a monster, look for a new one
                        LookForTrouble();
                    }
                    else
                    {
                        // attack the current monster
                        AttackMonster();
                    }
                    break;

                default:
                    // including a default case is just good practice
                    break;
            }
            // let the key press fall through to whatever control is selected, just in case it is needed
            keyEventArgs.Handled = false;
        }

        /// <summary>
        /// The form's Load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void FreyjaFormLoad(object sender, EventArgs eventArgs)
        {
            // update the GUI
            UpdateGui();
        }

        /// <summary>
        /// Let the player go looking for a monster to fight
        /// </summary>
        private void LookForTrouble()
        {
            if (currentMonster == null)
            {
                // get a new monster
                currentMonster = new Monster(playerLevel);
                var encounterVerb = lookingForTroubleVerbs[randomNumberSource.Next(0, lookingForTroubleVerbs.Count - 1)];
                // make sure we don't get the same encounter verb twice in a row
                while (encounterVerb == lastEncounterVerb)
                {
                    encounterVerb = lookingForTroubleVerbs[randomNumberSource.Next(0, lookingForTroubleVerbs.Count - 1)];
                }
                lastEncounterVerb = encounterVerb;
                var encounterString = string.Format("You've {0} a level {1} {2} with {3} hit point{4}!",
                                                    encounterVerb, currentMonster.MonsterLevel,
                                                    currentMonster.FullName, currentMonster.MaximumMonsterHitPoints, currentMonster.MaximumMonsterHitPoints != 1 ? "s" : "");
                SetEncounterText(encounterString);
                AddEntryToJournal(encounterString);
                UpdateGui();
            }
        }

        /// <summary>
        /// Sets the text of the encounter notification label
        /// </summary>
        /// <param name="newEncounterString">A string describing the encounter that gets displayed on the main form</param>
        private void SetEncounterText(string newEncounterString)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => SetEncounterText(newEncounterString)));
            }
            else
            {
                // don't do anything if the form is disposed (which might happen if the application is closed just before this code executes)
                if (!IsDisposed)
                {
                    // set the text
                    labelNotification.Text = newEncounterString;
                }
            }
        }
    }
}