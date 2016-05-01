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

            Console.WriteLine("============= :Original Tree (childtype & level): ============= ");
            tree.PrintTreeByLevel(showLeftOrRight_: true, showRowNumber_:true);

            Console.WriteLine("============= :Original Tree: ============= ");
            tree.PrintTreeByLevel(showLeftOrRight_: false, showRowNumber_: false);
            tree.SwapLeftRightNode(tree.Root);

            Console.WriteLine("============= :Swapped Tree: ============= ");
            tree.PrintTreeByLevel();

            Console.WriteLine("============= :Save Result Tree: ============= ");
            var saveOnFile = @"C:\temp\Tree\PrintTree.txt";
            Console.WriteLine();
            Console.WriteLine(saveOnFile);
            tree.PrintTreeByLevel(showLeftOrRight_: true, saveToFile_: saveOnFile, showRowNumber_: true);

            Console.ReadLine();
        }
    }
}
