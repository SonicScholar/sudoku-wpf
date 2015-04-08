using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Engine.Algorithm
{
    public abstract class SudokuAlgorithmBase
    {
        /// <summary>
        /// Performs one iteration of the algorithm for every cell on the board.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool Crank(ref SudokuBoard board)
        {
            var modified = DoCrank(ref board);
            if (modified)
            {
                Console.WriteLine("Board was changed by algorithm: " + this.GetType());
                Console.WriteLine(board.GetBoard());
                Console.WriteLine();
            }

            EliminateCandidates(ref board);
            return modified;
        }

        /// <summary>
        /// Returns true if the algorithm modified any cell values or candidates
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        protected abstract bool DoCrank(ref SudokuBoard board);
        private void EliminateCandidates(ref SudokuBoard board)
        {
            foreach (var cell in board.Cells)
            {
                //value is set, make sure that candidates are cleared out
                if (SudokuBoard.CELL_VALUES.Contains(cell.Value))
                {
                    cell.Candidates = new HashSet<int>();
                }
            }
        }

        protected void SetValue(int value, ref Cell cell, ref SudokuBoard board)
        {
            cell.Value = value;
            var cellCopy = cell;
            var relatedCells = board.Cells.Where(c => c.Row == cellCopy.Row || c.Column == cellCopy.Column || c.Chunk == cellCopy.Chunk).ToList();
            foreach (var rC in relatedCells)
            {
                rC.Candidates.Remove(value);
            }

        }

        public T[][] GetPowerSet<T>(List<T> list)
        {
            var result = Enumerable.Range(0, 1 << list.Count)
                .Select(m =>
                {
                    var bitIndexes = Enumerable.Range(0, list.Count);
                    return bitIndexes.Where(i => (m & (1 << i)) != 0)
                        .Select(i => list[i]).ToArray();
                }).ToArray();
            return result;
        }
    }
}
