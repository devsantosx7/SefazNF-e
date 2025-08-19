using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using util.fiscal.io;

namespace Monitor.PanelManager;

public class frmNpsSurveyComment : Form
{
	private NpsSurvey _NpsSurvey;

	private IContainer components;

	private Button btnSend;

	private Label lbAsk;

	private TextBox txComment;

	private Button btClose;

	private Label lbTitleBar;

	public frmNpsSurveyComment()
	{
		InitializeComponent();
	}

	public frmNpsSurveyComment(NpsSurvey pNpsSurvey)
	{
		InitializeComponent();
		_NpsSurvey = pNpsSurvey;
		if (clsFunction.funcConvStrToInt(pNpsSurvey.nsNote) > 6)
		{
			lbAsk.Text += " (Opcional)";
		}
		lbTitleBar.Text = pNpsSurvey.nsTitle;
	}

	private async void btnSend_Click(object sender, EventArgs e)
	{
		if (await funcValidDate())
		{
			_NpsSurvey.nsNote = _NpsSurvey.nsNote;
			_NpsSurvey.nsDtAnswer = DateTime.Now.ToString("yyyy-MM-dd");
			_NpsSurvey.nsNotAnswer = string.Empty;
			_NpsSurvey.nsComment = clsFunction.funcClearDiacritics(txComment.Text);
			await new clsDataNpsSurvey().funcUpdateAsync(_NpsSurvey);
			base.DialogResult = DialogResult.OK;
			Close();
		}
	}

	private async Task<bool> funcValidDate()
	{
		if (txComment.Text.Length <= 0 && clsFunction.funcConvStrToInt(_NpsSurvey.nsNote) <= 6)
		{
			MessageBox.Show("Por favor, deixe um comentário antes de enviar sua avaliação.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return false;
		}
		return true;
	}

	private void btClose_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
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
		this.btnSend = new System.Windows.Forms.Button();
		this.lbAsk = new System.Windows.Forms.Label();
		this.txComment = new System.Windows.Forms.TextBox();
		this.btClose = new System.Windows.Forms.Button();
		this.lbTitleBar = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.btnSend.BackColor = System.Drawing.Color.FromArgb(36, 199, 118);
		this.btnSend.FlatAppearance.BorderSize = 0;
		this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnSend.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSend.ForeColor = System.Drawing.Color.White;
		this.btnSend.Location = new System.Drawing.Point(327, 324);
		this.btnSend.Name = "btnSend";
		this.btnSend.Size = new System.Drawing.Size(130, 42);
		this.btnSend.TabIndex = 0;
		this.btnSend.Text = "Enviar";
		this.btnSend.UseVisualStyleBackColor = false;
		this.btnSend.Click += new System.EventHandler(btnSend_Click);
		this.lbAsk.AutoSize = true;
		this.lbAsk.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbAsk.Location = new System.Drawing.Point(25, 118);
		this.lbAsk.Name = "lbAsk";
		this.lbAsk.Size = new System.Drawing.Size(379, 20);
		this.lbAsk.TabIndex = 16;
		this.lbAsk.Text = "Por quais motivos você decidiu dar essa nota?";
		this.txComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txComment.Location = new System.Drawing.Point(29, 152);
		this.txComment.MaxLength = 2000;
		this.txComment.Multiline = true;
		this.txComment.Name = "txComment";
		this.txComment.Size = new System.Drawing.Size(714, 95);
		this.txComment.TabIndex = 18;
		this.btClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btClose.FlatAppearance.BorderSize = 0;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.Image = Monitor.Resources.image_screen_close;
		this.btClose.Location = new System.Drawing.Point(737, 12);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(26, 24);
		this.btClose.TabIndex = 21;
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.lbTitleBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbTitleBar.BackColor = System.Drawing.Color.White;
		this.lbTitleBar.Font = new System.Drawing.Font("Verdana", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitleBar.Location = new System.Drawing.Point(48, 9);
		this.lbTitleBar.Name = "lbTitleBar";
		this.lbTitleBar.Size = new System.Drawing.Size(683, 40);
		this.lbTitleBar.TabIndex = 22;
		this.lbTitleBar.Text = "Pesquisa NPS";
		this.lbTitleBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		base.ClientSize = new System.Drawing.Size(775, 420);
		base.ControlBox = false;
		base.Controls.Add(this.lbTitleBar);
		base.Controls.Add(this.btClose);
		base.Controls.Add(this.txComment);
		base.Controls.Add(this.lbAsk);
		base.Controls.Add(this.btnSend);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmNpsSurveyComment";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
