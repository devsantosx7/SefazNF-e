using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Monitor;

public class frmCloseScreen : Form
{
	private IContainer components;

	private Label label3;

	private CheckBox ckNotShowExitConfirm;

	private Label lbNotasFiscais;

	private Button btClose;

	private PictureBox pictureBox1;

	private PictureBox pictureBox2;

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmCloseScreen));
		this.label3 = new System.Windows.Forms.Label();
		this.ckNotShowExitConfirm = new System.Windows.Forms.CheckBox();
		this.lbNotasFiscais = new System.Windows.Forms.Label();
		this.btClose = new System.Windows.Forms.Button();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		base.SuspendLayout();
		this.label3.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label3.Location = new System.Drawing.Point(14, 228);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(490, 2);
		this.label3.TabIndex = 114;
		this.ckNotShowExitConfirm.AutoSize = true;
		this.ckNotShowExitConfirm.Checked = true;
		this.ckNotShowExitConfirm.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ckNotShowExitConfirm.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ckNotShowExitConfirm.Location = new System.Drawing.Point(17, 247);
		this.ckNotShowExitConfirm.Name = "ckNotShowExitConfirm";
		this.ckNotShowExitConfirm.Size = new System.Drawing.Size(218, 17);
		this.ckNotShowExitConfirm.TabIndex = 113;
		this.ckNotShowExitConfirm.Text = "Não mostrar esta tela novamente";
		this.ckNotShowExitConfirm.UseVisualStyleBackColor = true;
		this.lbNotasFiscais.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbNotasFiscais.Location = new System.Drawing.Point(60, 19);
		this.lbNotasFiscais.Name = "lbNotasFiscais";
		this.lbNotasFiscais.Size = new System.Drawing.Size(444, 105);
		this.lbNotasFiscais.TabIndex = 112;
		this.lbNotasFiscais.Text = resources.GetString("lbNotasFiscais.Text");
		this.lbNotasFiscais.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.btClose.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.btClose.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.Location = new System.Drawing.Point(420, 239);
		this.btClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(84, 31);
		this.btClose.TabIndex = 111;
		this.btClose.Text = "&OK";
		this.btClose.UseVisualStyleBackColor = true;
		this.pictureBox1.Image = Monitor.Resources.image_tool_disabled;
		this.pictureBox1.Location = new System.Drawing.Point(14, 22);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(30, 30);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 115;
		this.pictureBox1.TabStop = false;
		this.pictureBox2.Image = Monitor.Resources.image_fiscal_io_taskbar;
		this.pictureBox2.Location = new System.Drawing.Point(149, 127);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(220, 72);
		this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox2.TabIndex = 116;
		this.pictureBox2.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(519, 280);
		base.Controls.Add(this.pictureBox2);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.ckNotShowExitConfirm);
		base.Controls.Add(this.lbNotasFiscais);
		base.Controls.Add(this.btClose);
		this.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmCloseScreen";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Notificação para ocultar o Fiscal.io Monitor";
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
