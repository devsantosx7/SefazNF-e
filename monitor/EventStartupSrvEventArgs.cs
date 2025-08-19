using System;
using util.fiscal.io;

namespace Monitor;

public class EventStartupSrvEventArgs : EventArgs
{
	public string RetMessage { get; set; }

	public clsReturn ReturnFunc { get; set; }

	public bool ShowStatus { get; set; }
}
