using System.Drawing;

namespace Monitor;

public abstract class clsAbsTabData
{
	protected bool _IsLoaded;

	protected bool _Disposed;

	public string TabName { get; protected set; }

	public string TabText { get; protected set; }

	public Color TabColor { get; protected set; }

	public string FeatExtId { get; protected set; }

	public event EventTabManagerHandler EventTabManager;

	public clsAbsTabData()
	{
		_IsLoaded = false;
	}

	protected virtual void OnEventTabManager(EventTabManagerEventArgs e)
	{
		this.EventTabManager?.Invoke(this, e);
	}

	public bool IsLoaded()
	{
		return _IsLoaded;
	}
}
