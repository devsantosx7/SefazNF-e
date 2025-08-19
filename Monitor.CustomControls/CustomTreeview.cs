using System;
using System.Drawing;
using System.Windows.Forms;

namespace Monitor.CustomControls;

public class CustomTreeview : TreeView
{
	protected override void WndProc(ref Message m)
	{
		if (m.Msg == 515)
		{
			Point localPos = PointToClient(Cursor.Position);
			if (HitTest(localPos).Location == TreeViewHitTestLocations.StateImage)
			{
				m.Result = IntPtr.Zero;
			}
			else
			{
				base.WndProc(ref m);
			}
		}
		else
		{
			base.WndProc(ref m);
		}
	}
}
