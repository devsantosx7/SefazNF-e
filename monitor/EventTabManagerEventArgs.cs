using System;

namespace Monitor;

public class EventTabManagerEventArgs : EventArgs
{
	public string UserAction { get; set; }

	public string TaskProgress { get; set; }

	public decimal? TotalValue { get; set; }

	public long? TotalQuant { get; set; }
}
