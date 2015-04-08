using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Engine
{
    public class SudokuBoard
    {
        public List<Cell> Cells { get; set; }
        public static readonly int[] CELL_VALUES = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public SudokuBoard()
        {
            Cells = new List<Cell>(81);
            for (int r = 1; r <= 9; r++)
            {
                for (int c = 1; c <= 9; c++)
                {
                    Cells.Add(new Cell() { Row = r, Column = c });
                }
            }
        }

        public List<Cell> GetGroupCells(Cell cell, CellGroupType type)
        {
            var property = typeof(Cell).GetProperty(Enum.GetName(typeof(CellGroupType), type));
            var value = property.GetValue(cell);
            return Cells.Where(c => property.GetValue(c).Equals(value)).ToList();
        }

        public string GetBoard()
        {
            var rows = Cells.GroupBy(the => the.Row).OrderBy(the => the.Key).ToArray();
            string result = rows.Select(row =>
                {
                    var rowValues = row.OrderBy(c => c.Column).ToArray();
                    string rowString = rowValues.Select(c => c.Value.ToString())
                        .Aggregate((c1, c2) => String.Format("{0} , {1}", c1, c2));
                    return rowString;
                }).Aggregate((r1, r2) => r1 + Environment.NewLine + r2);
            return result;
        }

        public static SudokuBoard Load(string filePath)
        {
            var cells = new List<Cell>();

            var lines = File.ReadAllLines(filePath).Where(str => str.Length > 0).ToArray();
            if (lines.Length != 9)
            {
                throw new Exception("File malformatted: Not Nine Rows");
            }
            for (int r = 1; r <= lines.Length; r++)
            {
                var line = lines[r - 1];
                var values = line.Split(',');
                if (values.Length != 9)
                {
                    throw new Exception("File malformatted: row " + r + " does not have 9 values.");
                }
                for (int c = 1; c <= values.Length; c++)
                {
                    var value = values[c - 1].ToString();
                    var cell = new Cell() { Row = r, Column = c, Value = !String.IsNullOrEmpty(value) ? int.Parse(value) : 0 };
                    cells.Add(cell);
                }
            }

            var board = new SudokuBoard() { Cells = cells };
            board.InitializeCandidates();
            return board;
        }

        public void InitializeCandidates()
        {
            foreach (var cell in Cells)
            {
                var nonCandidates = Enum.GetValues(typeof(CellGroupType)).Cast<CellGroupType>()
                    .Select(type => this.GetGroupCells(cell, type).Where(val => CELL_VALUES.Contains(val.Value)))
                    .SelectMany(it => it).Distinct();

                var candidates = CELL_VALUES.Contains(cell.Value) ? new HashSet<int>() :
                    CELL_VALUES.Where(val => !nonCandidates.Any(c => val.Equals(c.Value)));
                cell.Candidates = new HashSet<int>(candidates);
            }
        }
    }
}
