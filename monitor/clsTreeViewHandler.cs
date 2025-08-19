using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class clsTreeViewHandler
{
	public class NodeIndex
	{
		public string NodeName { get; set; }

		public int NodeIndx { get; set; }

		public object ChildList { get; set; }
	}

	public class NodeItem
	{
		public string NodeName { get; set; }

		public TreeNode NodeObjt { get; set; }

		public bool Expanded { get; set; }
	}

	private List<NodeItem> varBufferList = new List<NodeItem>();

	private List<NodeIndex> varIndexList = new List<NodeIndex>();

	public async Task<TreeView> funcSyncBufferAsync(TreeView pTreeView, string pTagValue)
	{
		if (pTreeView == null)
		{
			return pTreeView;
		}
		string varTreeTag = (string)pTreeView.Tag;
		if (varTreeTag == null)
		{
			return pTreeView;
		}
		pTreeView.Enabled = false;
		pTreeView.BeginUpdate();
		pTreeView.SuspendLayout();
		if (clsFunction.IsEqual(varTreeTag, "NEWONE"))
		{
			pTreeView = funcCreateIndex(pTreeView);
		}
		pTreeView = funcMountDefault(pTreeView);
		pTreeView = funcIndexDefault(pTreeView);
		pTreeView = funcRemoveNodesFromHeader(pTreeView, pTagValue);
		pTreeView = funcRemoveNodesFromItens(pTreeView, pTagValue);
		TreeView treeView = pTreeView;
		treeView.SelectedNode = await funcGetNodeSelected(pTreeView);
		pTreeView.EndUpdate();
		pTreeView.ResumeLayout();
		pTreeView.Enabled = true;
		return pTreeView;
	}

	private TreeView funcCreateIndex(TreeView pTreeView)
	{
		varBufferList.Clear();
		varIndexList.Clear();
		foreach (TreeNode varHeadItem in pTreeView.Nodes)
		{
			NodeIndex varNodeHead = new NodeIndex
			{
				NodeName = varHeadItem.Name,
				NodeIndx = varHeadItem.Index
			};
			List<NodeIndex> varChildList = new List<NodeIndex>();
			foreach (TreeNode varSubItem in varHeadItem.Nodes)
			{
				NodeIndex varNodeSub = new NodeIndex
				{
					NodeName = varSubItem.Name,
					NodeIndx = varSubItem.Index
				};
				varChildList.Add(varNodeSub);
			}
			varNodeHead.ChildList = varChildList;
			varIndexList.Add(varNodeHead);
		}
		pTreeView.Tag = string.Empty;
		return pTreeView;
	}

	private TreeView funcMountDefault(TreeView pTreeView)
	{
		foreach (TreeNode varHeadItem in pTreeView.Nodes)
		{
			foreach (TreeNode varSubItem in varHeadItem.Nodes)
			{
				NodeItem varBufferSubItem = new NodeItem();
				varBufferSubItem.NodeName = varSubItem.Name;
				varBufferSubItem.NodeObjt = (TreeNode)varSubItem.Clone();
				varBufferSubItem.Expanded = varSubItem.IsExpanded;
				varBufferList.Add(varBufferSubItem);
			}
			NodeItem varBufferHeadItem = new NodeItem();
			varBufferHeadItem.NodeName = varHeadItem.Name;
			varBufferHeadItem.NodeObjt = (TreeNode)varHeadItem.Clone();
			varBufferHeadItem.Expanded = varHeadItem.IsExpanded;
			varBufferHeadItem.NodeObjt.Nodes.Clear();
			varBufferList.Add(varBufferHeadItem);
		}
		return pTreeView;
	}

	private TreeView funcIndexDefault(TreeView pTreeView)
	{
		pTreeView.Nodes.Clear();
		foreach (NodeIndex varHeadIndex in varIndexList)
		{
			NodeItem varBufferHeadItem = varBufferList.FirstOrDefault((NodeItem r) => r.NodeName.Equals(varHeadIndex.NodeName));
			if (varBufferHeadItem == null)
			{
				continue;
			}
			TreeNode varHeaderItem = (TreeNode)varBufferHeadItem.NodeObjt.Clone();
			foreach (NodeIndex varSubIndex in (List<NodeIndex>)varHeadIndex.ChildList)
			{
				NodeItem varBufferSubItem = varBufferList.FirstOrDefault((NodeItem r) => r.NodeName.Equals(varSubIndex.NodeName));
				if (varBufferSubItem != null)
				{
					TreeNode varSubNodeItem = (TreeNode)varBufferSubItem.NodeObjt.Clone();
					if (varBufferSubItem.Expanded)
					{
						varSubNodeItem.Expand();
					}
					varHeaderItem.Nodes.Insert(varSubIndex.NodeIndx, varSubNodeItem);
				}
			}
			if (varBufferHeadItem.Expanded)
			{
				varHeaderItem.Expand();
			}
			pTreeView.Nodes.Insert(varHeadIndex.NodeIndx, varHeaderItem);
		}
		varBufferList.Clear();
		return pTreeView;
	}

	private TreeView funcRemoveNodesFromHeader(TreeView pTreeView, string pTagValue)
	{
		foreach (NodeItem varNodeHeader in funcCloneNodes(pTreeView.Nodes))
		{
			if (varNodeHeader.NodeObjt == null)
			{
				continue;
			}
			string varTagItem = (string)varNodeHeader.NodeObjt.Tag;
			if (varTagItem == null)
			{
				continue;
			}
			bool varMustRemove = false;
			if (varTagItem.Contains(pTagValue + "#"))
			{
				varMustRemove = true;
			}
			else if (varTagItem.EndsWith(pTagValue))
			{
				varMustRemove = true;
			}
			if (!varMustRemove)
			{
				continue;
			}
			foreach (NodeItem varNodeItem in funcCloneNodes(varNodeHeader.NodeObjt.Nodes))
			{
				pTreeView.Nodes[varNodeHeader.NodeObjt.Name].Nodes.RemoveByKey(varNodeItem.NodeName);
				varBufferList.Add(varNodeItem);
			}
			varNodeHeader.NodeObjt.Nodes.Clear();
			pTreeView.Nodes.RemoveByKey(varNodeHeader.NodeName);
			varBufferList.Add(varNodeHeader);
		}
		return pTreeView;
	}

	private TreeView funcRemoveNodesFromItens(TreeView pTreeView, string pTagValue)
	{
		foreach (TreeNode varNodeHeader in pTreeView.Nodes)
		{
			foreach (NodeItem varNodeItem in funcCloneNodes(varNodeHeader.Nodes))
			{
				string varTagItem = (string)varNodeItem.NodeObjt.Tag;
				if (varTagItem != null)
				{
					bool varMustRemove = false;
					if (varTagItem.Contains(pTagValue + "#"))
					{
						varMustRemove = true;
					}
					else if (varTagItem.EndsWith(pTagValue))
					{
						varMustRemove = true;
					}
					if (varMustRemove)
					{
						pTreeView.Nodes[varNodeHeader.Name].Nodes.RemoveByKey(varNodeItem.NodeName);
						varBufferList.Add(varNodeItem);
					}
				}
			}
		}
		return pTreeView;
	}

	private List<NodeItem> funcCloneNodes(TreeNodeCollection pNodeList)
	{
		List<NodeItem> varNodeList = new List<NodeItem>();
		foreach (TreeNode varNodeObj in pNodeList)
		{
			NodeItem varNodeItem = new NodeItem();
			varNodeItem.NodeName = varNodeObj.Name;
			varNodeItem.NodeObjt = (TreeNode)varNodeObj.Clone();
			varNodeItem.Expanded = varNodeObj.IsExpanded;
			varNodeList.Add(varNodeItem);
		}
		return varNodeList;
	}

	public async Task<TreeNode> funcGetNodeSelected(TreeView pTreeView)
	{
		string varDataNode = await new clsDataParameter().funcGetAsync("DataFilterNodeFocus");
		if (string.IsNullOrEmpty(varDataNode))
		{
			return null;
		}
		foreach (TreeNode varTreeNodeLevel01 in pTreeView.Nodes)
		{
			if (varTreeNodeLevel01.Name.Equals(varDataNode))
			{
				return varTreeNodeLevel01;
			}
			foreach (TreeNode varTreeNodeLevel2 in varTreeNodeLevel01.Nodes)
			{
				if (varTreeNodeLevel2.Name.Equals(varDataNode))
				{
					return varTreeNodeLevel2;
				}
				foreach (TreeNode varTreeNodeLevel3 in varTreeNodeLevel2.Nodes)
				{
					if (varTreeNodeLevel3.Name.Equals(varDataNode))
					{
						return varTreeNodeLevel3;
					}
				}
			}
		}
		return null;
	}
}
