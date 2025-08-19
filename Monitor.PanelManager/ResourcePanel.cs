using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Monitor.PanelManager;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class ResourcePanel
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("Monitor.PanelManager.ResourcePanel", typeof(ResourcePanel).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static Bitmap image_backup_data => (Bitmap)ResourceManager.GetObject("image_backup_data", resourceCulture);

	internal static string strPanelFiscalServer => ResourceManager.GetString("strPanelFiscalServer", resourceCulture);

	internal static string strPanelPerformance => ResourceManager.GetString("strPanelPerformance", resourceCulture);

	internal static string strPanelSearchCFeSat => ResourceManager.GetString("strPanelSearchCFeSat", resourceCulture);

	internal static string strPanelXmlValidation => ResourceManager.GetString("strPanelXmlValidation", resourceCulture);

	internal ResourcePanel()
	{
	}
}
