namespace CustomTaskRunOrder
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbxActions = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbxFlow = new System.Windows.Forms.GroupBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.rtbxResult = new System.Windows.Forms.RichTextBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lbxActions
            //
            this.lbxActions.FormattingEnabled = true;
            this.lbxActions.ItemHeight = 15;
            this.lbxActions.Location = new System.Drawing.Point(12, 27);
            this.lbxActions.Name = "lbxActions";
            this.lbxActions.Size = new System.Drawing.Size(141, 379);
            this.lbxActions.TabIndex = 0;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "動作列表";
            //
            // gbxFlow
            //
            this.gbxFlow.Location = new System.Drawing.Point(174, 27);
            this.gbxFlow.Name = "gbxFlow";
            this.gbxFlow.Size = new System.Drawing.Size(575, 379);
            this.gbxFlow.TabIndex = 2;
            this.gbxFlow.TabStop = false;
            this.gbxFlow.Text = "Flow";
            //
            // btnRun
            //
            this.btnRun.Location = new System.Drawing.Point(767, 27);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(66, 23);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.buttonRun_Click);
            //
            // rtbxResult
            //
            this.rtbxResult.Location = new System.Drawing.Point(767, 56);
            this.rtbxResult.Name = "rtbxResult";
            this.rtbxResult.Size = new System.Drawing.Size(147, 350);
            this.rtbxResult.TabIndex = 4;
            this.rtbxResult.Text = "";
            //
            // btnReset
            //
            this.btnReset.Location = new System.Drawing.Point(848, 27);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(66, 23);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 418);
            this.Controls.Add(this.rtbxResult);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.gbxFlow);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbxActions);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox lbxActions;
        private Label label1;
        private GroupBox gbxFlow;
        private Button btnRun;
        private RichTextBox rtbxResult;
        private Button btnReset;
    }
}
