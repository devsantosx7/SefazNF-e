using System;

namespace Monitor;

public class EventPanelManagerEventArgs : EventArgs
{
	public string UserAction { get; set; }

	public object ObjectItem { get; set; }
}
