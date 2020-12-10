using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broland_Amplifier_Wpf.Model
{
    public class TreeNode
    {
        public int NodeID { get; set; }
        public int ParentID { get; set; }
        public string NodeName { get; set; }
        public string Icon { get; set; }
        public List<TreeNode> Children { get; set; }
        public TreeNode()
        {
            Children = new List<TreeNode>();
        }
    }
}
