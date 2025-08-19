using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Monitor;

public class PanelTransparent : Panel
{
	private const int WS_EX_TRANSPARENT = 32;

	private int opacity = 50;

	private IContainer components;

	[DefaultValue(50)]
	public int Opacity
	{
		get
		{
			return opacity;
		}
		set
		{
			if (value < 0 || value > 100)
			{
				throw new ArgumentException("value must be between 0 and 100");
			}
			opacity = value;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams obj = base.CreateParams;
			obj.ExStyle |= 32;
			return obj;
		}
	}

	public PanelTransparent()
	{
		InitializeComponent();
		SetStyle(ControlStyles.Opaque, value: true);
	}

	public PanelTransparent(IContainer container)
	{
		container.Add(this);
		InitializeComponent();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		using (SolidBrush brush = new SolidBrush(Color.FromArgb(opacity * 255 / 100, BackColor)))
		{
			e.Graphics.FillRectangle(brush, base.ClientRectangle);
		}
		base.OnPaint(e);
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
		this.components = new System.ComponentModel.Container();
	}
}
