using System;

public class clsObjecSearch : ICloneable
{
	public int DcIcon { get; set; }

	public string DocKey { get; set; }

	public string DcType { get; set; }

	public string Action { get; set; }

	public string Status { get; set; }

	public string Filial { get; set; }

	public clsObjecSearch GetClone()
	{
		return (clsObjecSearch)MemberwiseClone();
	}

	object ICloneable.Clone()
	{
		return (clsObjecSearch)MemberwiseClone();
	}
}
