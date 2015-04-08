using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Engine.Algorithm
{
    /// <summary>
    /// Find the cells with only one candidate and assign that value to them
    /// </summary>
    public class SingleCandidatesAlgorithm : SudokuAlgorithmBase
    {
        protected override bool DoCrank(ref SudokuBoard board)
        {
            bool modified = false;
            foreach (var cell in board.Cells)
            {
                if (cell.Candidates.Count == 1)
                {
                    modified = true;
                    var c = cell;
                    this.SetValue(cell.Candidates.FirstOrDefault(), ref c, ref board);
                    cell.Candidates = new HashSet<int>();
                }
            }
            return modified;
        }
    }
}
