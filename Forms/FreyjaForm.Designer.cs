namespace Freyja
{
   partial class FreyjaForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose (bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose ();
         }
         base.Dispose (disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent ()
      {
         this.progressBarPotionRefillTimer = new System.Windows.Forms.ProgressBar();
         this.buttonDrinkPotion = new System.Windows.Forms.Button();
         this.labelMonsterLog = new System.Windows.Forms.Label();
         this.richTextBoxMonsterLog = new System.Windows.Forms.RichTextBox();
         this.labelPlayerXpToNextLevelDisplay = new System.Windows.Forms.Label();
         this.labelPlayerTotalXpDisplay = new System.Windows.Forms.Label();
         this.labelPlayerHitPointsDisplay = new System.Windows.Forms.Label();
         this.labelPlayerLevelDisplay = new System.Windows.Forms.Label();
         this.labelPlayerLevel = new System.Windows.Forms.Label();
         this.labelNotification = new System.Windows.Forms.Label();
         this.progressBarHpRecoveryProgress = new System.Windows.Forms.ProgressBar();
         this.labelPlayerXpToNextLevel = new System.Windows.Forms.Label();
         this.labelPlayerTotalXp = new System.Windows.Forms.Label();
         this.labelHpTimer = new System.Windows.Forms.Label();
         this.labelPlayerHitPoints = new System.Windows.Forms.Label();
         this.labelAdventureJournal = new System.Windows.Forms.Label();
         this.buttonFlee = new System.Windows.Forms.Button();
         this.buttonAttack = new System.Windows.Forms.Button();
         this.buttonLookForTrouble = new System.Windows.Forms.Button();
         this.listBoxAdventureJournal = new System.Windows.Forms.ListBox();
         this.SuspendLayout();
         // 
         // progressBarPotionRefillTimer
         // 
         this.progressBarPotionRefillTimer.Location = new System.Drawing.Point(408, 26);
         this.progressBarPotionRefillTimer.Maximum = 1000;
         this.progressBarPotionRefillTimer.Name = "progressBarPotionRefillTimer";
         this.progressBarPotionRefillTimer.Size = new System.Drawing.Size(83, 20);
         this.progressBarPotionRefillTimer.TabIndex = 43;
         this.progressBarPotionRefillTimer.Visible = false;
         // 
         // buttonDrinkPotion
         // 
         this.buttonDrinkPotion.Location = new System.Drawing.Point(408, 26);
         this.buttonDrinkPotion.Name = "buttonDrinkPotion";
         this.buttonDrinkPotion.Size = new System.Drawing.Size(83, 20);
         this.buttonDrinkPotion.TabIndex = 42;
         this.buttonDrinkPotion.Text = "Drink Potion";
         this.buttonDrinkPotion.UseVisualStyleBackColor = true;
         // 
         // labelMonsterLog
         // 
         this.labelMonsterLog.AutoSize = true;
         this.labelMonsterLog.Location = new System.Drawing.Point(494, 10);
         this.labelMonsterLog.Name = "labelMonsterLog";
         this.labelMonsterLog.Size = new System.Drawing.Size(66, 13);
         this.labelMonsterLog.TabIndex = 41;
         this.labelMonsterLog.Text = "Monster Log";
         // 
         // richTextBoxMonsterLog
         // 
         this.richTextBoxMonsterLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.richTextBoxMonsterLog.Location = new System.Drawing.Point(497, 26);
         this.richTextBoxMonsterLog.Name = "richTextBoxMonsterLog";
         this.richTextBoxMonsterLog.ReadOnly = true;
         this.richTextBoxMonsterLog.Size = new System.Drawing.Size(259, 88);
         this.richTextBoxMonsterLog.TabIndex = 40;
         this.richTextBoxMonsterLog.Text = "";
         this.richTextBoxMonsterLog.WordWrap = false;
         // 
         // labelPlayerXpToNextLevelDisplay
         // 
         this.labelPlayerXpToNextLevelDisplay.BackColor = System.Drawing.SystemColors.Window;
         this.labelPlayerXpToNextLevelDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.labelPlayerXpToNextLevelDisplay.Location = new System.Drawing.Point(268, 65);
         this.labelPlayerXpToNextLevelDisplay.Name = "labelPlayerXpToNextLevelDisplay";
         this.labelPlayerXpToNextLevelDisplay.Size = new System.Drawing.Size(223, 20);
         this.labelPlayerXpToNextLevelDisplay.TabIndex = 39;
         this.labelPlayerXpToNextLevelDisplay.Text = "labelPlayerXpToNextLevelDisplay";
         this.labelPlayerXpToNextLevelDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // labelPlayerTotalXpDisplay
         // 
         this.labelPlayerTotalXpDisplay.BackColor = System.Drawing.SystemColors.Window;
         this.labelPlayerTotalXpDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.labelPlayerTotalXpDisplay.Location = new System.Drawing.Point(21, 65);
         this.labelPlayerTotalXpDisplay.Name = "labelPlayerTotalXpDisplay";
         this.labelPlayerTotalXpDisplay.Size = new System.Drawing.Size(223, 20);
         this.labelPlayerTotalXpDisplay.TabIndex = 38;
         this.labelPlayerTotalXpDisplay.Text = "labelPlayerTotalXpDisplay";
         this.labelPlayerTotalXpDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // labelPlayerHitPointsDisplay
         // 
         this.labelPlayerHitPointsDisplay.BackColor = System.Drawing.SystemColors.Window;
         this.labelPlayerHitPointsDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.labelPlayerHitPointsDisplay.Location = new System.Drawing.Point(143, 26);
         this.labelPlayerHitPointsDisplay.Name = "labelPlayerHitPointsDisplay";
         this.labelPlayerHitPointsDisplay.Size = new System.Drawing.Size(101, 20);
         this.labelPlayerHitPointsDisplay.TabIndex = 37;
         this.labelPlayerHitPointsDisplay.Text = "labelPlayerHitPointsDisplay";
         this.labelPlayerHitPointsDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // labelPlayerLevelDisplay
         // 
         this.labelPlayerLevelDisplay.BackColor = System.Drawing.SystemColors.Window;
         this.labelPlayerLevelDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.labelPlayerLevelDisplay.Location = new System.Drawing.Point(21, 26);
         this.labelPlayerLevelDisplay.Name = "labelPlayerLevelDisplay";
         this.labelPlayerLevelDisplay.Size = new System.Drawing.Size(101, 20);
         this.labelPlayerLevelDisplay.TabIndex = 36;
         this.labelPlayerLevelDisplay.Text = "labelPlayerLevelDisplay";
         this.labelPlayerLevelDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // labelPlayerLevel
         // 
         this.labelPlayerLevel.AutoSize = true;
         this.labelPlayerLevel.Location = new System.Drawing.Point(14, 10);
         this.labelPlayerLevel.Name = "labelPlayerLevel";
         this.labelPlayerLevel.Size = new System.Drawing.Size(33, 13);
         this.labelPlayerLevel.TabIndex = 35;
         this.labelPlayerLevel.Text = "Level";
         // 
         // labelNotification
         // 
         this.labelNotification.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.labelNotification.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.labelNotification.ForeColor = System.Drawing.Color.Red;
         this.labelNotification.Location = new System.Drawing.Point(14, 117);
         this.labelNotification.Name = "labelNotification";
         this.labelNotification.Size = new System.Drawing.Size(742, 23);
         this.labelNotification.TabIndex = 34;
         this.labelNotification.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // progressBarHpRecoveryProgress
         // 
         this.progressBarHpRecoveryProgress.Location = new System.Drawing.Point(268, 26);
         this.progressBarHpRecoveryProgress.Maximum = 1000;
         this.progressBarHpRecoveryProgress.Name = "progressBarHpRecoveryProgress";
         this.progressBarHpRecoveryProgress.Size = new System.Drawing.Size(134, 20);
         this.progressBarHpRecoveryProgress.TabIndex = 33;
         // 
         // labelPlayerXpToNextLevel
         // 
         this.labelPlayerXpToNextLevel.AutoSize = true;
         this.labelPlayerXpToNextLevel.Location = new System.Drawing.Point(261, 49);
         this.labelPlayerXpToNextLevel.Name = "labelPlayerXpToNextLevel";
         this.labelPlayerXpToNextLevel.Size = new System.Drawing.Size(91, 13);
         this.labelPlayerXpToNextLevel.TabIndex = 32;
         this.labelPlayerXpToNextLevel.Text = "XP To Next Level";
         // 
         // labelPlayerTotalXp
         // 
         this.labelPlayerTotalXp.AutoSize = true;
         this.labelPlayerTotalXp.Location = new System.Drawing.Point(14, 49);
         this.labelPlayerTotalXp.Name = "labelPlayerTotalXp";
         this.labelPlayerTotalXp.Size = new System.Drawing.Size(48, 13);
         this.labelPlayerTotalXp.TabIndex = 31;
         this.labelPlayerTotalXp.Text = "Total XP";
         // 
         // labelHpTimer
         // 
         this.labelHpTimer.AutoSize = true;
         this.labelHpTimer.Location = new System.Drawing.Point(261, 10);
         this.labelHpTimer.Name = "labelHpTimer";
         this.labelHpTimer.Size = new System.Drawing.Size(146, 13);
         this.labelHpTimer.TabIndex = 30;
         this.labelHpTimer.Text = "Time Until Next HP Recovery";
         // 
         // labelPlayerHitPoints
         // 
         this.labelPlayerHitPoints.AutoSize = true;
         this.labelPlayerHitPoints.Location = new System.Drawing.Point(136, 10);
         this.labelPlayerHitPoints.Name = "labelPlayerHitPoints";
         this.labelPlayerHitPoints.Size = new System.Drawing.Size(22, 13);
         this.labelPlayerHitPoints.TabIndex = 29;
         this.labelPlayerHitPoints.Text = "HP";
         // 
         // labelAdventureJournal
         // 
         this.labelAdventureJournal.AutoSize = true;
         this.labelAdventureJournal.Location = new System.Drawing.Point(11, 140);
         this.labelAdventureJournal.Name = "labelAdventureJournal";
         this.labelAdventureJournal.Size = new System.Drawing.Size(41, 13);
         this.labelAdventureJournal.TabIndex = 28;
         this.labelAdventureJournal.Text = "Journal";
         // 
         // buttonFlee
         // 
         this.buttonFlee.Enabled = false;
         this.buttonFlee.Location = new System.Drawing.Point(416, 91);
         this.buttonFlee.Name = "buttonFlee";
         this.buttonFlee.Size = new System.Drawing.Size(75, 20);
         this.buttonFlee.TabIndex = 27;
         this.buttonFlee.Text = "&Flee";
         this.buttonFlee.UseVisualStyleBackColor = true;
         this.buttonFlee.Click += new System.EventHandler(this.ButtonFleeClick);
         // 
         // buttonAttack
         // 
         this.buttonAttack.Enabled = false;
         this.buttonAttack.Location = new System.Drawing.Point(335, 91);
         this.buttonAttack.Name = "buttonAttack";
         this.buttonAttack.Size = new System.Drawing.Size(75, 20);
         this.buttonAttack.TabIndex = 26;
         this.buttonAttack.Text = "&Attack";
         this.buttonAttack.UseVisualStyleBackColor = true;
         this.buttonAttack.Click += new System.EventHandler(this.ButtonAttackClick);
         // 
         // buttonLookForTrouble
         // 
         this.buttonLookForTrouble.Location = new System.Drawing.Point(14, 91);
         this.buttonLookForTrouble.Name = "buttonLookForTrouble";
         this.buttonLookForTrouble.Size = new System.Drawing.Size(315, 20);
         this.buttonLookForTrouble.TabIndex = 25;
         this.buttonLookForTrouble.Text = "&Look For Trouble";
         this.buttonLookForTrouble.UseVisualStyleBackColor = true;
         this.buttonLookForTrouble.Click += new System.EventHandler(this.ButtonLookForTroubleClick);
         // 
         // listBoxAdventureJournal
         // 
         this.listBoxAdventureJournal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.listBoxAdventureJournal.FormattingEnabled = true;
         this.listBoxAdventureJournal.HorizontalScrollbar = true;
         this.listBoxAdventureJournal.IntegralHeight = false;
         this.listBoxAdventureJournal.Location = new System.Drawing.Point(14, 156);
         this.listBoxAdventureJournal.Name = "listBoxAdventureJournal";
         this.listBoxAdventureJournal.Size = new System.Drawing.Size(742, 176);
         this.listBoxAdventureJournal.TabIndex = 24;
         // 
         // FreyjaForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(766, 343);
         this.Controls.Add(this.progressBarPotionRefillTimer);
         this.Controls.Add(this.buttonDrinkPotion);
         this.Controls.Add(this.labelMonsterLog);
         this.Controls.Add(this.richTextBoxMonsterLog);
         this.Controls.Add(this.labelPlayerXpToNextLevelDisplay);
         this.Controls.Add(this.labelPlayerTotalXpDisplay);
         this.Controls.Add(this.labelPlayerHitPointsDisplay);
         this.Controls.Add(this.labelPlayerLevelDisplay);
         this.Controls.Add(this.labelPlayerLevel);
         this.Controls.Add(this.labelNotification);
         this.Controls.Add(this.progressBarHpRecoveryProgress);
         this.Controls.Add(this.labelPlayerXpToNextLevel);
         this.Controls.Add(this.labelPlayerTotalXp);
         this.Controls.Add(this.labelHpTimer);
         this.Controls.Add(this.labelPlayerHitPoints);
         this.Controls.Add(this.labelAdventureJournal);
         this.Controls.Add(this.buttonFlee);
         this.Controls.Add(this.buttonAttack);
         this.Controls.Add(this.buttonLookForTrouble);
         this.Controls.Add(this.listBoxAdventureJournal);
         this.KeyPreview = true;
         this.MinimumSize = new System.Drawing.Size(782, 382);
         this.Name = "FreyjaForm";
         this.Text = "Freyja";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FreyjaFormFormClosing);
         this.Load += new System.EventHandler(this.FreyjaFormLoad);
         this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FreyjaFormKeyDown);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ProgressBar progressBarPotionRefillTimer;
      private System.Windows.Forms.Button buttonDrinkPotion;
      private System.Windows.Forms.Label labelMonsterLog;
      private System.Windows.Forms.RichTextBox richTextBoxMonsterLog;
      private System.Windows.Forms.Label labelPlayerXpToNextLevelDisplay;
      private System.Windows.Forms.Label labelPlayerTotalXpDisplay;
      private System.Windows.Forms.Label labelPlayerHitPointsDisplay;
      private System.Windows.Forms.Label labelPlayerLevelDisplay;
      private System.Windows.Forms.Label labelPlayerLevel;
      private System.Windows.Forms.Label labelNotification;
      private System.Windows.Forms.ProgressBar progressBarHpRecoveryProgress;
      private System.Windows.Forms.Label labelPlayerXpToNextLevel;
      private System.Windows.Forms.Label labelPlayerTotalXp;
      private System.Windows.Forms.Label labelHpTimer;
      private System.Windows.Forms.Label labelPlayerHitPoints;
      private System.Windows.Forms.Label labelAdventureJournal;
      private System.Windows.Forms.Button buttonFlee;
      private System.Windows.Forms.Button buttonAttack;
      private System.Windows.Forms.Button buttonLookForTrouble;
      private System.Windows.Forms.ListBox listBoxAdventureJournal;
   }
}

