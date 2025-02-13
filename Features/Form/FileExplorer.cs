using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Microsoft.VisualBasic.FileIO;
using System.Xml.Linq;

namespace Elbis.Features
{
    /* Class  :FileExplorer
     * Author : Chandana Subasinghe
     * Date   : 10/03/2006
     * Discription : This class use to create the tree view and load 
     *               directories and files in to the tree
     *          
     */
    class FileExplorer
    {
        public FileExplorer()
        {

        }

        /* Method :CreateTree
         * Author : Chandana Subasinghe
         * Date   : 10/03/2006
         * Discription : This is use to creat and build the tree
         *          
         */

        public bool CreateTree(TreeView treeView, List<string> directoryList)
        {
            bool returnValue = false;
                 
            try
            {
                foreach (string drv in directoryList)
                {
                    
                    TreeNode fChild = new TreeNode();
                    fChild.ImageIndex = 0;
                    fChild.SelectedImageIndex = 0;
                    fChild.Text = drv;
                    fChild.Nodes.Add("");
                    treeView.Nodes.Add(fChild);
                    returnValue = true;
                }

            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;
            
        }

        /* Method :EnumerateDirectory
         * Author : Chandana Subasinghe
         * Date   : 10/03/2006
         * Discription : This is use to Enumerate directories and files
         *          
         */
        private bool IsDataAlreadyExists(TreeNode parentNode, string data)
        {
            foreach (TreeNode node in parentNode.Nodes)
            {
                if (node.Text == data)
                {
                    // Veri zaten var
                    return true;
                }
            }

            // Veri yok
            return false;
        }
        public TreeNode EnumerateDirectory(TreeNode parentNode,List<string> directoryList, List<string> filesList)
        {
          
            try
            {
                //parentNode.Nodes[0].Remove();
                parentNode.Nodes.Clear();
                foreach (var dir in directoryList)
                {
                    bool chck = IsDataAlreadyExists(parentNode, dir);
                    if (!chck)
                    {
                        TreeNode node = new TreeNode();
                        node.Text = dir;
                        node.Nodes.Add("");
                        parentNode.Nodes.Add(node);
                    }

                }
                //Fill files
                foreach (var file in filesList)
                {
                    bool chck = IsDataAlreadyExists(parentNode, file);
                    if (!chck)
                    {
                        TreeNode node = new TreeNode();
                        node.Text = file;
                        node.ImageIndex = 2;
                        node.SelectedImageIndex = 2;
                        parentNode.Nodes.Add(node);
                    }
                }



            }

            catch (Exception ex)
            {
                //TODO : 
            }
           
            return parentNode;
        }
    }
}
