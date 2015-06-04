//-----------------------------------------------------------------------
// <copyright file = "FreyjaForm.cs" company = "Me">
//     Copyright (c) Me!
// </copyright>
//-----------------------------------------------------------------------

namespace Freyja
{
   using Freyja.Source;
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
      #region Private Fields

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
      /// A place to store the last encounter verb, so they don't get used twice in a row
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
      private readonly Random randomNumberSource = new Random ();

      /// <summary>
      /// A wait handle used by the HP recovery thread to count seconds
      /// </summary>
      private readonly AutoResetEvent waitingForHpRecoveryTimer = new AutoResetEvent (false);

      /// <summary>
      /// A wait handle used by the potion recovery thread to count seconds
      /// </summary>
      private readonly AutoResetEvent waitingForPotionRefillTimer = new AutoResetEvent (false);

      /// <summary>
      /// The monster that the player is currently fighting
      /// </summary>
      private Monster currentMonster;
      /// <summary>
      /// The time between HP recoveries
      /// </summary>
      private TimeSpan hpRecoveryInterval = new TimeSpan (0/*days*/, 0/*hours*/, 0/*minutes*/, 30/*seconds*/);
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
      private TimeSpan potionRefillInterval = new TimeSpan (0/*days*/, 1/*hours*/, 0/*minutes*/, 0/*seconds*/);

      #endregion Private Fields

      #region Public Constructors

      /// <summary>
      /// Default constructor
      /// </summary>
      public FreyjaForm ()
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
         InitializeComponent ();
      }

      #endregion Public Constructors

      #region Private Methods

      /// <summary>
      /// Adds an item to the Journal listbox
      /// </summary>
      /// <param name="newEntry">The text of the new journal entry</param>
      private void AddEntryToJournal (string newEntry)
      {
         if (InvokeRequired)
         {
            BeginInvoke (new MethodInvoker (() => AddEntryToJournal (newEntry)));
         }
         else
         {
            // don't do anything if the form is disposed (which might happen if the application is closed just before this code executes)
            if (!IsDisposed)
            {
               listBoxAdventureJournal.Items.Add (newEntry);
               // if the journal has gotten too long, drop the oldest item
               if (listBoxAdventureJournal.Items.Count > 1000)
               {
                  listBoxAdventureJournal.Items.RemoveAt (0);
               }
               // scroll to bottom
               listBoxAdventureJournal.SelectedIndex = listBoxAdventureJournal.Items.Count - 1;
               // this clears the selected item in the box, so that the bottom line is not highlighted in blue after scroll to the bottom
               // I just think it looks better this way
               listBoxAdventureJournal.ClearSelected ();
            }
         }
      }

      /// <summary>
      /// Recovers hit points for the player over time
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="doWorkEventArgs"></param>
      private void HpRecoveryTimerThreadDoWork (object sender, DoWorkEventArgs doWorkEventArgs)
      {
         // a timer that ticks once per second
         using (var hpTimer = new Timer (1000))
         {
            hpTimer.Elapsed += HpTimerElapsed;
            // create and start a stopwatch
            var stopWatch = new Stopwatch ();
            stopWatch.Start ();
            // enable and start the timer
            hpTimer.Enabled = true;
            hpTimer.Start ();
            // fills up the progress bar as time passes between HP recoveries
            while ((stopWatch.ElapsedMilliseconds < hpRecoveryInterval.TotalMilliseconds) && !((BackgroundWorker)sender).CancellationPending)
            {
               // wait a mo to give some processor time to the rest of the application
               waitingForHpRecoveryTimer.WaitOne (10000);
               // update the GUI
               UpdateGui ((int)((stopWatch.ElapsedMilliseconds / hpRecoveryInterval.TotalMilliseconds) * progressBarHpRecoveryProgress.Maximum));
            }
            // stop timer
            hpTimer.Stop ();
         }
      }

      /// <summary>
      /// Increments the player's health and updates the GUI when the thread has finished its work
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="runWorkerCompletedEventArgs"></param>
      private void HpRecoveryTimerThreadRunWorkerCompleted (object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
      {
         // only increment the player's hit points if they are below his or her maximum
         if (playerCurrentHitPoints < playerMaximumHitPoints)
         {
            int increment = (playerMaximumHitPoints - playerCurrentHitPoints >= 5) ? 5 : playerMaximumHitPoints - playerCurrentHitPoints;
            playerCurrentHitPoints += increment;
         }
         // the progress bar will be full when this event fires, so we'll zero that out
         UpdateGui (0);
      }

      /// <summary>
      /// Signals the hp recovery thread to continue
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="elapsedEventArgs"></param>
      private void HpTimerElapsed (object sender, ElapsedEventArgs elapsedEventArgs)
      {
         waitingForHpRecoveryTimer.Set ();
      }

      /// <summary>
      /// Counts down until the user can use the potion again
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="doWorkEventArgs"></param>
      private void PotionRefillTimerThreadDoWork (object sender, DoWorkEventArgs doWorkEventArgs)
      {
         // a timer that ticks once per second
         using (var potionTimer = new Timer (1000))
         {
            potionTimer.Elapsed += PotionTimerElapsed;
            // create and start a stopwatch
            var stopWatch = new Stopwatch ();
            stopWatch.Start ();
            // enable and start the timer
            potionTimer.Enabled = true;
            potionTimer.Start ();
            // fills up the progress bar as time passes between HP recoveries
            while ((stopWatch.ElapsedMilliseconds < potionRefillInterval.TotalMilliseconds) && !((BackgroundWorker)sender).CancellationPending)
            {
               // wait a mo to give some processor time to the rest of the application
               waitingForPotionRefillTimer.WaitOne (10000);
               // update the GUI
               UpdateGui (-1, (int)((stopWatch.ElapsedMilliseconds / potionRefillInterval.TotalMilliseconds) * progressBarPotionRefillTimer.Maximum));
            }
            // stop timer
            potionTimer.Stop ();
         }
      }

      /// <summary>
      /// Updates the GUI when the potion timer runs out
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="runWorkerCompletedEventArgs"></param>
      private void PotionRefillTimerThreadRunWorkerCompleted (object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
      {
         AddEntryToJournal ("Your potion has refilled!");
         // clear the potion progress bar, while leaving the other one alone
         UpdateGui (-1, 0);
      }
      /// <summary>
      /// Signals the potion timer thread to continue
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="elapsedEventArgs"></param>
      private void PotionTimerElapsed (object sender, ElapsedEventArgs elapsedEventArgs)
      {
         waitingForPotionRefillTimer.Set ();
      }
      /// <summary>
      /// Updates the GUI to enable/disable buttons, set progress bar values, etc.
      /// </summary>
      private void UpdateGui (int hpRecoveryProgressBarValue = -1, int potionRefillProgressBarValue = -1)
      {
         if (InvokeRequired)
         {
            BeginInvoke (new MethodInvoker (() => UpdateGui (hpRecoveryProgressBarValue, potionRefillProgressBarValue)));
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
                  hpRecoveryTimerThread.RunWorkerAsync ();
               }
               else if (playerCurrentHitPoints == playerMaximumHitPoints && hpRecoveryTimerThread.IsBusy)
               {
                  // the player's hit points are completely recovered, but the recovery thread is still running
                  // honestly, this should never happen and this else clause might be over-engineering
                  hpRecoveryTimerThread.CancelAsync ();
               }
               // update the hit points display
               labelPlayerHitPointsDisplay.Text = string.Format ("{0}/{1}", playerCurrentHitPoints, playerMaximumHitPoints);
               // display level
               labelPlayerLevelDisplay.Text = playerLevel.ToString ();
               // update XP display
               labelPlayerTotalXpDisplay.Text = playerTotalXp.ToString ();
               labelPlayerXpToNextLevelDisplay.Text = playerXpToNextLevel.ToString ();
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

      #endregion Private Methods
   }
}