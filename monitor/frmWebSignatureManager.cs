using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace Monitor;

public class frmWebSignatureManager : Form
{
	private WebView2 webView;

	private readonly string MANAGER_URL = "https://app.fiscal.io";

	private IContainer components;

	private Label lbLoading;

	public frmWebSignatureManager()
	{
		InitializeComponent();
		InitializeWebView();
	}

	private void InitializeWebView()
	{
		webView = new WebView2
		{
			Dock = DockStyle.Fill
		};
		base.Controls.Add(webView);
		webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
		webView.NavigationCompleted += WebView_NavigationCompleted;
		webView.Source = new Uri(MANAGER_URL);
	}

	private void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
	{
		if (!e.IsSuccess)
		{
			MessageBox.Show("Erro ao inicializar o WebView2: " + e.InitializationException.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
	{
		if (e.IsSuccess)
		{
			lbLoading.Hide();
		}
		else
		{
			MessageBox.Show($"Erro ao navegar: {e.WebErrorStatus}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmWebSignatureManager));
		this.lbLoading = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lbLoading.AutoSize = true;
		this.lbLoading.BackColor = System.Drawing.SystemColors.Window;
		this.lbLoading.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbLoading.Location = new System.Drawing.Point(429, 362);
		this.lbLoading.Name = "lbLoading";
		this.lbLoading.Size = new System.Drawing.Size(192, 18);
		this.lbLoading.TabIndex = 2;
		this.lbLoading.Text = "Aguarde Carregando...";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1064, 845);
		base.Controls.Add(this.lbLoading);
		this.Font = new System.Drawing.Font("Verdana", 8.25f);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmWebSignatureManager";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Gerenciar assinatura";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
