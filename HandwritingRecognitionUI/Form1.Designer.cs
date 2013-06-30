namespace HandwritingRecognitionUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TrainingLabel = new System.Windows.Forms.Label();
            this.TestLabel = new System.Windows.Forms.Label();
            this.TrainingBox = new System.Windows.Forms.TextBox();
            this.TrainBtn = new System.Windows.Forms.Button();
            this.TestBtn = new System.Windows.Forms.Button();
            this.TestBox = new System.Windows.Forms.TextBox();
            this.ResultBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.WeightsBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TrainingLabel
            // 
            this.TrainingLabel.AutoSize = true;
            this.TrainingLabel.Location = new System.Drawing.Point(28, 23);
            this.TrainingLabel.Name = "TrainingLabel";
            this.TrainingLabel.Size = new System.Drawing.Size(64, 13);
            this.TrainingLabel.TabIndex = 0;
            this.TrainingLabel.Text = "Training Set";
            this.TrainingLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // TestLabel
            // 
            this.TestLabel.AutoSize = true;
            this.TestLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestLabel.Location = new System.Drawing.Point(31, 59);
            this.TestLabel.Name = "TestLabel";
            this.TestLabel.Size = new System.Drawing.Size(49, 15);
            this.TestLabel.TabIndex = 1;
            this.TestLabel.Text = "Test set";
            // 
            // TrainingBox
            // 
            this.TrainingBox.Location = new System.Drawing.Point(163, 23);
            this.TrainingBox.Name = "TrainingBox";
            this.TrainingBox.Size = new System.Drawing.Size(197, 20);
            this.TrainingBox.TabIndex = 2;
            // 
            // TrainBtn
            // 
            this.TrainBtn.Location = new System.Drawing.Point(34, 125);
            this.TrainBtn.Name = "TrainBtn";
            this.TrainBtn.Size = new System.Drawing.Size(75, 23);
            this.TrainBtn.TabIndex = 3;
            this.TrainBtn.Text = "Train";
            this.TrainBtn.UseVisualStyleBackColor = true;
            this.TrainBtn.Click += new System.EventHandler(this.TrainBtn_Click);
            // 
            // TestBtn
            // 
            this.TestBtn.Location = new System.Drawing.Point(213, 125);
            this.TestBtn.Name = "TestBtn";
            this.TestBtn.Size = new System.Drawing.Size(75, 23);
            this.TestBtn.TabIndex = 4;
            this.TestBtn.Text = "Test";
            this.TestBtn.UseVisualStyleBackColor = true;
            this.TestBtn.Click += new System.EventHandler(this.TestBtn_Click);
            // 
            // TestBox
            // 
            this.TestBox.Location = new System.Drawing.Point(163, 59);
            this.TestBox.Name = "TestBox";
            this.TestBox.Size = new System.Drawing.Size(197, 20);
            this.TestBox.TabIndex = 5;
            // 
            // ResultBox
            // 
            this.ResultBox.Location = new System.Drawing.Point(163, 184);
            this.ResultBox.Name = "ResultBox";
            this.ResultBox.Size = new System.Drawing.Size(197, 20);
            this.ResultBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Result";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Save weights to:";
            // 
            // WeightsBox
            // 
            this.WeightsBox.Location = new System.Drawing.Point(163, 86);
            this.WeightsBox.Name = "WeightsBox";
            this.WeightsBox.Size = new System.Drawing.Size(197, 20);
            this.WeightsBox.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 262);
            this.Controls.Add(this.WeightsBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ResultBox);
            this.Controls.Add(this.TestBox);
            this.Controls.Add(this.TestBtn);
            this.Controls.Add(this.TrainBtn);
            this.Controls.Add(this.TrainingBox);
            this.Controls.Add(this.TestLabel);
            this.Controls.Add(this.TrainingLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TrainingLabel;
        private System.Windows.Forms.Label TestLabel;
        private System.Windows.Forms.TextBox TrainingBox;
        private System.Windows.Forms.Button TrainBtn;
        private System.Windows.Forms.Button TestBtn;
        private System.Windows.Forms.TextBox TestBox;
        private System.Windows.Forms.TextBox ResultBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox WeightsBox;
    }
}

