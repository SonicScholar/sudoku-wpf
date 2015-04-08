using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Engine;
using Sudoku.Engine.Algorithm;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            var newBoard = SudokuBoard.Load(@"C:\Users\ctewalt\Documents\Visual Studio 2013\Projects\Sudoku\csv\sudoku_2_001.csv");
            string boardString = newBoard.GetBoard();
            Console.WriteLine(boardString);

            var solver = new SudokuSolver(newBoard);

            bool solved = solver.Solve();
            if (solved)
                Console.WriteLine("Sudoku puzzle solved!");
            else
                Console.WriteLine("Unable to solve puzzle... :(");
            Console.ReadKey();
            Console.WriteLine(Environment.NewLine + Environment.NewLine);
            for (int i = 0; i < solver.Board.Cells.Count; i++)
            {
                var cell = solver.Board.Cells[i];
                bool hasCandidates = cell.Candidates.Count > 0;
                string candidates = hasCandidates ? cell.Candidates.Select(c => c.ToString()).Aggregate((a,b) => a+","+b): "";
                Console.WriteLine(String.Format("Cell {0}): candidates - {1}", i, candidates));
                Console.ReadKey();
            }


            Console.ReadKey();
            
        }
    }
}
