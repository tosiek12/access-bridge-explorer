namespace AccessBridgeExplorer {
  partial class TooltipWindow {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.Label = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // Label
      // 
      this.Label.AutoSize = true;
      this.Label.Location = new System.Drawing.Point(0, 0);
      this.Label.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.Label.Name = "Label";
      this.Label.Size = new System.Drawing.Size(30, 17);
      this.Label.TabIndex = 0;
      this.Label.Text = "text";
      // 
      // TooltipWindow
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
      this.ClientSize = new System.Drawing.Size(105, 53);
      this.ControlBox = false;
      this.Controls.Add(this.Label);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.Name = "TooltipWindow";
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.Label Label;
  }
}