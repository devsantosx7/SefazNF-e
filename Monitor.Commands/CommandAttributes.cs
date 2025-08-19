using System;
using System.ComponentModel.DataAnnotations;

namespace Monitor.Commands;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CommandAttributes : Attribute
{
	[Required]
	public string CommandName { get; set; } = "";

	public string CommandDesc { get; set; } = "";
}
