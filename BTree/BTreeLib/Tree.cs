using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTreeLib
{
    using System.IO;

    public class Tree
    {
        public Node Root { get; set; }
        public int Height { get; set; }

        public Node Add(string key_, Node parentNode_=null, NodeAt nodeAt_=NodeAt.Left)
        {
            var newNode = new Node();
            newNode.Key = key_;

            if (parentNode_ == null)
            {
                newNode.Level = 1;
                this.Height = 1;
                newNode.NodeAt = NodeAt.Root;
                this.Root = newNode;
            }
            else if (nodeAt_ == NodeAt.Left)
            {
                newNode.Level = parentNode_.Level + 1;
                newNode.NodeAt = NodeAt.Left;
                this.Height = Math.Max(this.Height, newNode.Level);
                parentNode_.LeftNode = newNode;
            }
            else
            {
                newNode.Level = parentNode_.Level + 1;
                newNode.NodeAt = NodeAt.Right;
                this.Height = Math.Max(this.Height, newNode.Level);
                parentNode_.RightNode = newNode;
            }

            return newNode;
        }

        /// <summary>
        /// CenteredText : returns value in the center of its column
        /// e.g. : '    XX     '  or '    L:XX     ' 
        /// </summary>
        /// <param name="displayValue_"></param>
        /// <param name="columnWidth_"></param>
        /// <param name="fill_"></param>
        /// <returns></returns>
        private string CenteredText(string displayValue_, int columnWidth_, char fill_ = ' ' )
        {
            var valStr = displayValue_;
            var itemWidth = displayValue_.Length;
            var totalSpaces = columnWidth_ - itemWidth;
            var spacesLeft = Convert.ToInt32(totalSpaces / 2.0);
            var spacesRight = totalSpaces - spacesLeft;

            var leftSpace = " ".PadLeft(spacesLeft, fill_);
            var rightSpace = " ".PadLeft(spacesRight, fill_);
            var itemFilled = leftSpace + valStr + rightSpace;

            return itemFilled;
        }

        /// <summary>
        /// PrinteTreeByLevel - in 2 dimension,
        /// 
        /// To Console or to a File
        /// </summary>
        /// <param name="saveToFile_"></param>
        /// <param name="showLeftOrRight_"></param>
        /// <param name="showRowNumber_"></param>
        public void PrintTreeByLevel(string saveToFile_ = null, bool showLeftOrRight_ = false, bool showRowNumber_ = false)
        {
            if (!string.IsNullOrEmpty(saveToFile_))
            {
                var standardOutput = new StreamWriter(Console.OpenStandardOutput());

                try
                {
                    var fileFolder = Path.GetDirectoryName(saveToFile_);
                    if (!string.IsNullOrEmpty(fileFolder) && !Directory.Exists(fileFolder))
                    {
                        Directory.CreateDirectory(fileFolder);
                    }

                    using (var writer = new StreamWriter(saveToFile_))
                    {
                        Console.SetOut(writer);
                        writer.AutoFlush = true;
                        this.PrintTreeByLevel(showLeftOrRight_, showRowNumber_);
                    }
                }
                catch (Exception ex)
                {
                    standardOutput.AutoFlush = true;
                    Console.SetOut(standardOutput);

                    Console.WriteLine(ex);
                }

                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }
            else
            {
                PrintTreeByLevel(showLeftOrRight_, showRowNumber_);
            }
        }

        /// <summary>
        /// 
        /// PrintTreeByLevel
        /// 
        /// Prints tree in 2-dimension, and display the corresponding node key value.
        /// 
        /// Options:
        ///   1) display the child type Left/Right and/or 
        ///   2) show the corresponding row/level number
        /// 
        /// Example 1; no options
        /// 
        ///                J
        ///        D               S
        ///    A       G       M       Z
        ///                      K
        ///                       F
        /// 
        /// Example 2: row numbers and left/right types;
        /// 
        /// 1.                              H:J
        /// 2.              L:D                             R:S
        /// 3.      L:A             R:G             L:M             R:Z
        /// 4.                                          R:K
        /// 5.                                             R:F
        /// 
        /// Note: the method assigns rectangle space with total width derived from the height of the tree.
        ///       The higher the height the wider is the rectangle.
        ///       The rectangle space consists of rows with variable number of cells/columns
        ///       The number of cells/columns assigned to each row is 2^row-1
        ///       Each cell in a row is assigned the value of the corresponding node that is situated in the tree (in its 2-dimension coordinates).
        ///       If a node is missing from the graph, the corresponding cell is filled with empty-space
        /// 
        /// </summary>
        /// <param name="showLeftOrRight_"></param>
        /// <param name="showRowNumber_"></param>
        ///
        private void PrintTreeByLevel(bool showLeftOrRight_ = false, bool showRowNumber_ = false)
        {
            var height = this.Height;
            var maxGaps = Math.Pow(2, height-1);
            var width = 2;
            if (showLeftOrRight_) width += 2;

            var totalWidth = maxGaps * width;
            var nodes = new Queue<Node>();
            var rows = new Queue<int>();
            var cells = new Queue<long>();

            var curRow = 1;
            nodes.Enqueue(this.Root);
            rows.Enqueue(curRow);
            var rootCell = Convert.ToInt32(Math.Pow(2, curRow - 1)) - 1;
            cells.Enqueue(rootCell);

            while (nodes.Count > 0)
            {
                var maxRowCells = Convert.ToInt32(Math.Pow(2, curRow - 1)); // maxRowCells = 2^row-1
                var cellWidth = Convert.ToInt32(totalWidth / maxRowCells);
                var rowDisplay = new StringBuilder();
                if (showRowNumber_) rowDisplay.Append(curRow).Append(".");
                var currLevelNodes = new List<Node>();
                var currLevelCells = new List<long>();

                var idx = -1;
                foreach (var node in nodes)
                {
                    ++idx;
                    var nodeRow = rows.ElementAt(idx);
                    if (curRow != nodeRow) continue; // collect only rows of current level

                    currLevelNodes.Add(node);
                    var cell = cells.ElementAt(idx);
                    currLevelCells.Add(cell);
                }

                var cellNumber = 0;
                idx = -1;
                foreach (var node in currLevelNodes)
                {
                    ++idx;
                    var itemVal = node.Key;
                    var nodeAt = node.NodeAt;
                    var nodeAtChar = nodeAt.ToString()[0];
                    nodeAtChar = nodeAt.Equals(NodeAt.Root) ? 'H' : nodeAtChar;
                    var emptySpace = "".PadLeft(cellWidth, ' ');
                    var currLevelCell = currLevelCells[idx];

                    // fill up gaps for cells missing data (in case tree/row is not balanced)
                    while (cellNumber++ < currLevelCell)
                    {
                        rowDisplay.Append(emptySpace);
                    }

                    var displayedItem = itemVal;
                    displayedItem = showLeftOrRight_ == false ? displayedItem : nodeAtChar + ":" + displayedItem;
                    var filledItem = this.CenteredText(displayedItem, cellWidth);
                    rowDisplay.Append(filledItem);

                    var parentNode = nodes.Dequeue();
                    var parentRow = rows.Dequeue();
                    var parentCell = cells.Dequeue();

                    curRow = parentRow + 1;

                    if (parentNode.LeftNode != null)
                    {
                        nodes.Enqueue(parentNode.LeftNode);
                        rows.Enqueue(parentRow + 1);
                        var leftChildCell = parentCell == 0 ? 0 : parentCell << 1;  // left child-cell = parentCell*2
                        cells.Enqueue(leftChildCell);
                    }
                    if (parentNode.RightNode != null)
                    {
                        nodes.Enqueue(parentNode.RightNode);
                        rows.Enqueue(parentRow + 1);
                        var rightChildCell = parentCell << 1;
                        rightChildCell |= 0x1;                   // right child-cell = parentCell*2 + 1
                        cells.Enqueue(rightChildCell);
                    }
                }

                Console.WriteLine(rowDisplay);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// SwapLeftRightNode - recursively
        /// 
        /// For Example;
        /// 
        /// From:
        ///                 J
        ///        D               S
        ///    A       G       M       Z
        ///                      K
        ///                       F
        /// To:
        ///                 J
        ///        S               D
        ///    Z       M       G       A
        ///          K
        ///         F
        /// 
        /// </summary>
        /// <param name="start_"></param>
        public void SwapLeftRightNode(Node start_)
        {
            if (start_ == null) return;

            if (start_.LeftNode != null && start_.RightNode != null)
            {
                var temp = start_.LeftNode;
                start_.LeftNode = start_.RightNode;
                start_.LeftNode.NodeAt = NodeAt.Left;
                start_.RightNode = temp;
                start_.RightNode.NodeAt = NodeAt.Right;

                this.SwapLeftRightNode(start_.LeftNode);
                this.SwapLeftRightNode(start_.RightNode);
            }
            else if (start_.LeftNode != null && start_.RightNode == null)
            {
                start_.RightNode = start_.LeftNode;
                start_.RightNode.NodeAt = NodeAt.Right;
                start_.LeftNode = null;
                this.SwapLeftRightNode(start_.RightNode);
            }
            else if (start_.RightNode != null && start_.LeftNode == null)
            {
                start_.LeftNode = start_.RightNode;
                start_.LeftNode.NodeAt = NodeAt.Left;
                start_.RightNode = null;
                this.SwapLeftRightNode(start_.LeftNode);
            }
        }
    }
}
