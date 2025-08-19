using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using data.fiscal.io;
using util.fiscal.io;

namespace Monitor.PanelManager;

public class frmNpsSurvey : Form
{
	private NpsSurvey _NpsSurvey;

	private IContainer components;

	private CheckBox chkRememberAgain;

	private Button btnSend;

	private Button btn1;

	private Button btn5;

	private Button btn10;

	private Button btn7;

	private Button btn4;

	private Button btn2;

	private Button btn3;

	private Button btn9;

	private Button btn6;

	private Button btn8;

	private Button btn0;

	private Label label1;

	private Label label2;

	private Label lbDesc;

	private Label lbTitleBar;

	private Button btClose;

	public frmNpsSurvey()
	{
		InitializeComponent();
	}

	public frmNpsSurvey(NpsSurvey pNpsSurvey)
	{
		InitializeComponent();
		_NpsSurvey = pNpsSurvey;
		lbTitleBar.Text = pNpsSurvey.nsTitle;
		lbDesc.Text = pNpsSurvey.nsQuestion;
		if (clsFunction.IsEqual(_NpsSurvey.nsMandatory, "X", pIgnoreCase: true))
		{
			chkRememberAgain.Visible = false;
		}
	}

	private async void btClose_Click(object sender, EventArgs e)
	{
		if (chkRememberAgain.Checked)
		{
			funcNotAnswer();
		}
		Close();
	}

	private async void btnSend_Click(object sender, EventArgs e)
	{
		if (chkRememberAgain.Checked)
		{
			funcNotAnswer();
		}
		else
		{
			funcSaveDate();
		}
	}

	private void chkRememberAgain_CheckedChanged(object sender, EventArgs e)
	{
		if (chkRememberAgain.Checked)
		{
			btnSend.Enabled = true;
			btnSend.Text = "Fechar";
			btnSend.BackColor = Color.Gray;
			return;
		}
		if (funcValidNote())
		{
			btnSend.Enabled = true;
		}
		else
		{
			btnSend.Enabled = false;
		}
		btnSend.Text = "Enviar";
		btnSend.BackColor = Color.FromArgb(36, 199, 118);
	}

	private void btn0_Click(object sender, EventArgs e)
	{
		if (btn0.BackColor == Color.LightGray)
		{
			funcRemoveAllRates();
			return;
		}
		funcRemoveAllRates();
		btn0.BackColor = Color.LightGray;
		btnSend.Enabled = true;
	}

	private void btn1_Click(object sender, EventArgs e)
	{
		if (btn1.BackColor == Color.LightGray)
		{
			funcRemoveAllRates();
			return;
		}
		funcRemoveAllRates();
		btn1.BackColor = Color.LightGray;
		btnSend.Enabled = true;
	}

	private void btn2_Click(object sender, EventArgs e)
	{
		if (btn2.BackColor == Color.LightGray)
		{
			funcRemoveAllRates();
			return;
		}
		funcRemoveAllRates();
		btn2.BackColor = Color.LightGray;
		btnSend.Enabled = true;
	}

	private void btn3_Click(object sender, EventArgs e)
	{
		if (btn3.BackColor == Color.LightGray)
		{
			funcRemoveAllRates();
			return;
		}
		funcRemoveAllRates();
		btn3.BackColor = Color.LightGray;
		btnSend.Enabled = true;
	}

	private void btn4_Click(object sender, EventArgs e)
	{
		if (btn4.BackColor == Color.LightGray)
		{
			funcRemoveAllRates();
			return;
		}
		funcRemoveAllRates();
		btn4.BackColor = Color.LightGray;
		btnSend.Enabled = true;
	}

	private void btn5_Click(object sender, EventArgs e)
	{
		if (btn5.BackColor == Color.LightGray)
		{
			funcRemoveAllRates();
			return;
		}
		funcRemoveAllRates();
		btn5.BackColor = Color.LightGray;
		btnSend.Enabled = true;
	}

	private void btn6_Click(object sender, EventArgs e)
	{
		if (btn6.BackColor == Color.LightGray)
		{
			funcRemoveAllRates();
			return;
		}
		funcRemoveAllRates();
		btn6.BackColor = Color.LightGray;
		btnSend.Enabled = true;
	}

	private void btn7_Click(object sender, EventArgs e)
	{
		if (btn7.BackColor == Color.LightGray)
		{
			funcRemoveAllRates();
			return;
		}
		funcRemoveAllRates();
		btn7.BackColor = Color.LightGray;
		btnSend.Enabled = true;
	}

	private void btn8_Click(object sender, EventArgs e)
	{
		if (btn8.BackColor == Color.LightGray)
		{
			funcRemoveAllRates();
			return;
		}
		funcRemoveAllRates();
		btn8.BackColor = Color.LightGray;
		btnSend.Enabled = true;
	}

	private void btn9_Click(object sender, EventArgs e)
	{
		if (btn9.BackColor == Color.LightGray)
		{
			funcRemoveAllRates();
			return;
		}
		funcRemoveAllRates();
		btn9.BackColor = Color.LightGray;
		btnSend.Enabled = true;
	}

	private void btn10_Click(object sender, EventArgs e)
	{
		if (btn10.BackColor == Color.LightGray)
		{
			funcRemoveAllRates();
			return;
		}
		funcRemoveAllRates();
		btn10.BackColor = Color.LightGray;
		btnSend.Enabled = true;
	}

	private async void funcNotAnswer()
	{
		_NpsSurvey.nsNote = string.Empty;
		_NpsSurvey.nsDtAnswer = DateTime.Now.ToString("yyyy-MM-dd");
		_NpsSurvey.nsNotAnswer = "X";
		await new clsDataNpsSurvey().funcUpdateAsync(_NpsSurvey);
		Close();
	}

	private async void funcSaveDate()
	{
		_NpsSurvey.nsNote = funcGetNoteValue();
		using frmNpsSurveyComment varfrmNpsSurveyReview = new frmNpsSurveyComment(_NpsSurvey);
		varfrmNpsSurveyReview.ShowDialog();
		if (varfrmNpsSurveyReview.DialogResult.Equals(DialogResult.OK))
		{
			Dispose();
		}
	}

	private string funcGetNoteValue()
	{
		Button[] array = new Button[11]
		{
			btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
			btn10
		};
		foreach (Button varItem in array)
		{
			if (varItem.BackColor == Color.LightGray)
			{
				return varItem.Tag.ToString();
			}
		}
		return "0";
	}

	private void funcRemoveAllRates()
	{
		Button[] array = new Button[11]
		{
			btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
			btn10
		};
		for (int i = 0; i < array.Length; i++)
		{
			array[i].BackColor = Color.White;
		}
		if (!chkRememberAgain.Checked)
		{
			btnSend.Enabled = false;
		}
	}

	private bool funcValidNote()
	{
		if (new Button[11]
		{
			btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
			btn10
		}.All((Button b) => b.BackColor == Color.White))
		{
			return false;
		}
		return true;
	}

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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.PanelManager.frmNpsSurvey));
		this.btnSend = new System.Windows.Forms.Button();
		this.chkRememberAgain = new System.Windows.Forms.CheckBox();
		this.btn1 = new System.Windows.Forms.Button();
		this.btn5 = new System.Windows.Forms.Button();
		this.btn10 = new System.Windows.Forms.Button();
		this.btn7 = new System.Windows.Forms.Button();
		this.btn4 = new System.Windows.Forms.Button();
		this.btn2 = new System.Windows.Forms.Button();
		this.btn3 = new System.Windows.Forms.Button();
		this.btn9 = new System.Windows.Forms.Button();
		this.btn6 = new System.Windows.Forms.Button();
		this.btn8 = new System.Windows.Forms.Button();
		this.btn0 = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.lbDesc = new System.Windows.Forms.Label();
		this.lbTitleBar = new System.Windows.Forms.Label();
		this.btClose = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.btnSend.BackColor = System.Drawing.Color.FromArgb(36, 199, 118);
		this.btnSend.Enabled = false;
		this.btnSend.FlatAppearance.BorderSize = 0;
		this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnSend.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSend.ForeColor = System.Drawing.Color.White;
		this.btnSend.Location = new System.Drawing.Point(618, 321);
		this.btnSend.Name = "btnSend";
		this.btnSend.Size = new System.Drawing.Size(130, 42);
		this.btnSend.TabIndex = 0;
		this.btnSend.Text = "Enviar";
		this.btnSend.UseVisualStyleBackColor = false;
		this.btnSend.Click += new System.EventHandler(btnSend_Click);
		this.chkRememberAgain.AutoSize = true;
		this.chkRememberAgain.BackColor = System.Drawing.Color.Transparent;
		this.chkRememberAgain.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.chkRememberAgain.Location = new System.Drawing.Point(34, 333);
		this.chkRememberAgain.Name = "chkRememberAgain";
		this.chkRememberAgain.Size = new System.Drawing.Size(217, 20);
		this.chkRememberAgain.TabIndex = 2;
		this.chkRememberAgain.Text = "Não desejo responder à pesquisa";
		this.chkRememberAgain.UseVisualStyleBackColor = false;
		this.chkRememberAgain.CheckedChanged += new System.EventHandler(chkRememberAgain_CheckedChanged);
		this.btn1.FlatAppearance.BorderSize = 0;
		this.btn1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn1.Image = Monitor.Resources.input_1;
		this.btn1.Location = new System.Drawing.Point(90, 175);
		this.btn1.Margin = new System.Windows.Forms.Padding(0);
		this.btn1.Name = "btn1";
		this.btn1.Size = new System.Drawing.Size(66, 53);
		this.btn1.TabIndex = 3;
		this.btn1.Tag = "1";
		this.btn1.UseVisualStyleBackColor = true;
		this.btn1.Click += new System.EventHandler(btn1_Click);
		this.btn5.FlatAppearance.BorderSize = 0;
		this.btn5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn5.Image = (System.Drawing.Image)resources.GetObject("btn5.Image");
		this.btn5.Location = new System.Drawing.Point(354, 176);
		this.btn5.Margin = new System.Windows.Forms.Padding(0);
		this.btn5.Name = "btn5";
		this.btn5.Size = new System.Drawing.Size(66, 53);
		this.btn5.TabIndex = 4;
		this.btn5.Tag = "5";
		this.btn5.UseVisualStyleBackColor = true;
		this.btn5.Click += new System.EventHandler(btn5_Click);
		this.btn10.FlatAppearance.BorderSize = 0;
		this.btn10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn10.Image = (System.Drawing.Image)resources.GetObject("btn10.Image");
		this.btn10.Location = new System.Drawing.Point(684, 176);
		this.btn10.Margin = new System.Windows.Forms.Padding(0);
		this.btn10.Name = "btn10";
		this.btn10.Size = new System.Drawing.Size(66, 53);
		this.btn10.TabIndex = 5;
		this.btn10.Tag = "10";
		this.btn10.UseVisualStyleBackColor = true;
		this.btn10.Click += new System.EventHandler(btn10_Click);
		this.btn7.BackColor = System.Drawing.Color.White;
		this.btn7.FlatAppearance.BorderSize = 0;
		this.btn7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn7.Image = (System.Drawing.Image)resources.GetObject("btn7.Image");
		this.btn7.Location = new System.Drawing.Point(486, 176);
		this.btn7.Margin = new System.Windows.Forms.Padding(0);
		this.btn7.Name = "btn7";
		this.btn7.Size = new System.Drawing.Size(66, 53);
		this.btn7.TabIndex = 6;
		this.btn7.Tag = "7";
		this.btn7.UseVisualStyleBackColor = false;
		this.btn7.Click += new System.EventHandler(btn7_Click);
		this.btn4.FlatAppearance.BorderSize = 0;
		this.btn4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn4.Image = (System.Drawing.Image)resources.GetObject("btn4.Image");
		this.btn4.Location = new System.Drawing.Point(288, 176);
		this.btn4.Margin = new System.Windows.Forms.Padding(0);
		this.btn4.Name = "btn4";
		this.btn4.Size = new System.Drawing.Size(66, 53);
		this.btn4.TabIndex = 7;
		this.btn4.Tag = "4";
		this.btn4.UseVisualStyleBackColor = true;
		this.btn4.Click += new System.EventHandler(btn4_Click);
		this.btn2.FlatAppearance.BorderSize = 0;
		this.btn2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn2.Image = (System.Drawing.Image)resources.GetObject("btn2.Image");
		this.btn2.Location = new System.Drawing.Point(156, 176);
		this.btn2.Margin = new System.Windows.Forms.Padding(0);
		this.btn2.Name = "btn2";
		this.btn2.Size = new System.Drawing.Size(66, 53);
		this.btn2.TabIndex = 8;
		this.btn2.Tag = "2";
		this.btn2.UseVisualStyleBackColor = true;
		this.btn2.Click += new System.EventHandler(btn2_Click);
		this.btn3.FlatAppearance.BorderSize = 0;
		this.btn3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn3.Image = (System.Drawing.Image)resources.GetObject("btn3.Image");
		this.btn3.Location = new System.Drawing.Point(222, 176);
		this.btn3.Margin = new System.Windows.Forms.Padding(0);
		this.btn3.Name = "btn3";
		this.btn3.Size = new System.Drawing.Size(66, 53);
		this.btn3.TabIndex = 9;
		this.btn3.Tag = "3";
		this.btn3.UseVisualStyleBackColor = true;
		this.btn3.Click += new System.EventHandler(btn3_Click);
		this.btn9.FlatAppearance.BorderSize = 0;
		this.btn9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn9.Image = (System.Drawing.Image)resources.GetObject("btn9.Image");
		this.btn9.Location = new System.Drawing.Point(618, 176);
		this.btn9.Margin = new System.Windows.Forms.Padding(0);
		this.btn9.Name = "btn9";
		this.btn9.Size = new System.Drawing.Size(66, 53);
		this.btn9.TabIndex = 10;
		this.btn9.Tag = "9";
		this.btn9.UseVisualStyleBackColor = true;
		this.btn9.Click += new System.EventHandler(btn9_Click);
		this.btn6.FlatAppearance.BorderSize = 0;
		this.btn6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn6.Image = (System.Drawing.Image)resources.GetObject("btn6.Image");
		this.btn6.Location = new System.Drawing.Point(420, 175);
		this.btn6.Margin = new System.Windows.Forms.Padding(0);
		this.btn6.Name = "btn6";
		this.btn6.Size = new System.Drawing.Size(66, 53);
		this.btn6.TabIndex = 11;
		this.btn6.Tag = "6";
		this.btn6.UseVisualStyleBackColor = true;
		this.btn6.Click += new System.EventHandler(btn6_Click);
		this.btn8.FlatAppearance.BorderSize = 0;
		this.btn8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn8.Image = (System.Drawing.Image)resources.GetObject("btn8.Image");
		this.btn8.Location = new System.Drawing.Point(552, 176);
		this.btn8.Margin = new System.Windows.Forms.Padding(0);
		this.btn8.Name = "btn8";
		this.btn8.Size = new System.Drawing.Size(66, 53);
		this.btn8.TabIndex = 12;
		this.btn8.Tag = "8";
		this.btn8.UseVisualStyleBackColor = true;
		this.btn8.Click += new System.EventHandler(btn8_Click);
		this.btn0.FlatAppearance.BorderSize = 0;
		this.btn0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn0.Image = Monitor.Resources.input_0;
		this.btn0.Location = new System.Drawing.Point(24, 175);
		this.btn0.Margin = new System.Windows.Forms.Padding(0);
		this.btn0.Name = "btn0";
		this.btn0.Size = new System.Drawing.Size(66, 53);
		this.btn0.TabIndex = 13;
		this.btn0.Tag = "0";
		this.btn0.UseVisualStyleBackColor = true;
		this.btn0.Click += new System.EventHandler(btn0_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(31, 162);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(120, 13);
		this.label1.TabIndex = 14;
		this.label1.Text = "NÃO RECOMENDARIA";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(654, 162);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(94, 13);
		this.label2.TabIndex = 15;
		this.label2.Text = "RECOMENDARIA";
		this.lbDesc.AutoSize = true;
		this.lbDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbDesc.Location = new System.Drawing.Point(30, 123);
		this.lbDesc.Name = "lbDesc";
		this.lbDesc.Size = new System.Drawing.Size(452, 20);
		this.lbDesc.TabIndex = 16;
		this.lbDesc.Text = "Qual a chance de você recomendar o Fiscal.io Monitor?";
		this.lbTitleBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbTitleBar.BackColor = System.Drawing.Color.White;
		this.lbTitleBar.Font = new System.Drawing.Font("Verdana", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitleBar.Location = new System.Drawing.Point(48, 9);
		this.lbTitleBar.Name = "lbTitleBar";
		this.lbTitleBar.Size = new System.Drawing.Size(683, 40);
		this.lbTitleBar.TabIndex = 19;
		this.lbTitleBar.Text = "Pesquisa NPS";
		this.lbTitleBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btClose.FlatAppearance.BorderSize = 0;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.Image = Monitor.Resources.image_screen_close;
		this.btClose.Location = new System.Drawing.Point(737, 12);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(26, 24);
		this.btClose.TabIndex = 20;
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		base.ClientSize = new System.Drawing.Size(775, 420);
		base.ControlBox = false;
		base.Controls.Add(this.btClose);
		base.Controls.Add(this.lbTitleBar);
		base.Controls.Add(this.lbDesc);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.btn0);
		base.Controls.Add(this.btn8);
		base.Controls.Add(this.btn6);
		base.Controls.Add(this.btn9);
		base.Controls.Add(this.btn3);
		base.Controls.Add(this.btn2);
		base.Controls.Add(this.btn4);
		base.Controls.Add(this.btn7);
		base.Controls.Add(this.btn10);
		base.Controls.Add(this.btn5);
		base.Controls.Add(this.btn1);
		base.Controls.Add(this.chkRememberAgain);
		base.Controls.Add(this.btnSend);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmNpsSurvey";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
