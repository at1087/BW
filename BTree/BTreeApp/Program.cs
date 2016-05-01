using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTreeApp
{
    using System.Diagnostics;

    using BTreeLib;

    class Program
    {
        static void Main(string[] args)
        {
            var tree = new Tree();

            var rootNode = tree.Add("J");

            var nodeS = tree.Add("S", rootNode, NodeAt.Right);
            var nodeD = tree.Add("D", rootNode, NodeAt.Left);

            var nodeA = tree.Add("A", nodeD, NodeAt.Left);
            var nodeG = tree.Add("G", nodeD, NodeAt.Right);

            var nodeM = tree.Add("M", nodeS, NodeAt.Left);
            var nodeZ = tree.Add("Z", nodeS, NodeAt.Right);

            var nodeK = tree.Add("K", nodeM, NodeAt.Right);
            var nodeF = tree.Add("F", nodeK, NodeAt.Right);

            Console.WriteLine("============= :Original Tree (show Left/Right & rows): ============= ");
            const string OriginalFile = @"C:\temp\Tree\OriginalTree.txt";
            tree.PrintTreeByLevel(showLeftOrRight_: true, showRowNumber_:true);

            tree.PrintTreeByLevel(showLeftOrRight_: true, showRowNumber_: true, saveToFile_:OriginalFile);
            Process.Start("Notepad.exe", OriginalFile);

            Console.WriteLine("============= :Original Tree: ============= ");
            tree.PrintTreeByLevel(showLeftOrRight_: false, showRowNumber_: false);
            tree.SwapLeftRightNode(tree.Root);

            Console.WriteLine("============= :Swapped Tree: ============= ");
            tree.PrintTreeByLevel();

            const string ResultFile = @"C:\temp\Tree\SwappedTree.txt";
            tree.PrintTreeByLevel(showLeftOrRight_: true, saveToFile_: ResultFile, showRowNumber_: true);

            Process.Start("Notepad.exe", ResultFile);

            Console.ReadLine();
        }
    }
}
