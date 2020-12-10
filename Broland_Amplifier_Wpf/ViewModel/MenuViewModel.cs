using Broland_Amplifier_Wpf.Helper;
using Broland_Amplifier_Wpf.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Broland_Amplifier_Wpf.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
        public virtual List<TreeNode> ChildNodes { get; set; }

        public ICommand SelectedCommand => new RelayCommand<TreeNode>((node) =>
        {
            ExcuteSendCommand(node);
        });
        public MenuViewModel()
        {


            List<TreeNode> FDList = new List<TreeNode>()
            {
                new TreeNode(){ NodeName="带内增益", Icon="" ,ParentID=1},
                new TreeNode(){ NodeName="带内增益平坦度", Icon="" ,ParentID=1},
                new TreeNode(){ NodeName="输出功率1dB压缩点", Icon=""  ,ParentID=1},
                new TreeNode(){ NodeName="饱和输入功率", Icon="" ,ParentID=1},
                new TreeNode(){ NodeName="反向隔离度", Icon="" ,ParentID=1 },
                new TreeNode(){ NodeName="输入/输出端口驻波", Icon=""  ,ParentID=1},
                new TreeNode(){ NodeName="噪声系数", Icon="" , ParentID=1},
                new TreeNode(){ NodeName="谐波抑制", Icon="" , ParentID=1 }
            };

            List<TreeNode> SJList = new List<TreeNode>()
            {
                new TreeNode(){ NodeName="衰减量", Icon="" , ParentID=2}
            };

            List<TreeNode> MenuList = new List<TreeNode>()
            {
                new TreeNode(){ NodeName="放大器板卡测试", Icon="" , ParentID=0, Children = FDList},
                new TreeNode(){ NodeName="衰减器板卡测试", Icon="" , ParentID=0, Children = SJList}
            };

            ChildNodes = MenuList;
        }
        /// <summary>
        /// 当前显示的模块
        /// </summary>
        public virtual UserControl SelectedView { get; set; }

        private void ExcuteSendCommand(TreeNode NodeName)
        {
            if (NodeName.ParentID != 0)
            {
                SelectedView = InitAutofac.GetFromFac<UserControl>(NodeName.NodeName);
                this.RaisePropertyChanged(nameof(SelectedView));
            }
        }
    }
}
