using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Engine.Algorithm
{
    public class SudokuSolver
    {
        public SudokuBoard Board { get; set; }
        private List<SudokuAlgorithmBase> Algorithms;
        public SudokuSolver(SudokuBoard board)
        {
            Algorithms = new List<SudokuAlgorithmBase>();
            Algorithms.Add(new XWingSetAlgorithm());
            Algorithms.Add(new SingleCandidatesAlgorithm());
            Algorithms.Add(new PointingSets());
            Algorithms.Add(new HiddenSets());
            Board = board;
        }

        public bool Solve()
        {
            bool success = false;
            bool crank = true;
            while (crank)
            {
                crank = false;
                foreach(var algorithm in Algorithms)
                {
                    var board = Board;
                    crank |= algorithm.Crank(ref board);
                    if (board.Cells.All(c => SudokuBoard.CELL_VALUES.Contains(c.Value)))
                    {
                        //all cells have a value, return true
                        return true;
                    }
                }
            }

            //using current algorithms, problem is not solveable
            return false;
        }
    }
}
