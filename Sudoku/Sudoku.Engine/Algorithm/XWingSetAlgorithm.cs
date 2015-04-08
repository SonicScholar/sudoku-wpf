using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Engine.Algorithm
{
    public class XWingSetAlgorithm : SudokuAlgorithmBase
    {
        /*
         * For each cell value 1 - 9,
         * Get All cells that have candidates for the current value.
         * Start with Xwing of size one and continue up to 9
         *  - for each row/column
         *  - are the row/column candidates less than or equal to the XWing size we're looking for?
         *  - if yes, this row MIGHT be an XWing Set candidate. 
         *    - Are there at least "setSize" row/column(s) that 
         *        a) have less than or equal to the number of elements in our xwing set candidate
         *        b) every value in the row/column exist in the candidate row/column
         *        If yes, we have an Xwing!
         *        - out of all the rows that meet this criteria, skip the first "setSize", and take the rest
         *        - remove candidate values from the rest of the rows, ONLY IN THE SAME COLUMNS AS THE CANDIDATE ROW/Column
         *    */
        protected override bool DoCrank(ref SudokuBoard board)
        {
            bool modified = false;
            foreach (int currentValue in SudokuBoard.CELL_VALUES)
            {
                var candidateCells = board.Cells.Where(c => c.Candidates.Contains(currentValue)).ToList();

                var boardRows = board.Cells.GroupBy(the => the.Row).ToArray();
                var boardColumns = board.Cells.GroupBy(the => the.Column).ToArray();

                var candidateRows = candidateCells.GroupBy(c => c.Row);
                var candidateColumns = candidateCells.GroupBy(c => c.Column);

                var uniqueColumns = candidateCells.Select(c => c.Column).Distinct().ToList();
                var uniqueRows = candidateCells.Select(c => c.Row).Distinct().ToList();

                //that is, the column values to test for the sudukoboard rows
                var rowSets = this.GetPowerSet(uniqueColumns).Distinct();

                //vice versa
                var colSets = this.GetPowerSet(uniqueRows).Distinct();


                foreach(int xWingSize in SudokuBoard.CELL_VALUES)
                {
                    var xWingTestRows = rowSets.Where(r => r.Count() == xWingSize).ToArray();
                    foreach(var xWingValues in xWingTestRows)
                    {
                        int rowCount = xWingValues.Count();
                        var rowsToTest = candidateRows.Where(r => xWingValues.Contains(r.Key)).ToArray();
                        if (rowsToTest.Length < rowCount)
                        {
                            continue;
                        }

                        var setUnionOfCandidateColumns = rowsToTest.SelectMany(r => r.Select(c => c.Column)).Distinct().ToArray();
                        if (setUnionOfCandidateColumns.Length != rowCount)
                            continue;
                        var xWingTrimColumns = boardColumns.Where(clmn => setUnionOfCandidateColumns.Contains(clmn.Key)).ToArray();
                        foreach (var trimCol in xWingTrimColumns)
                        {
                            var trimCells = trimCol.Where(c => !xWingValues.Contains(c.Row) && c.Candidates.Contains(currentValue)).ToArray();
                            foreach (var cell in trimCells)
                            {
                                modified |= cell.Candidates.Remove(currentValue);
                            }
                        }
                        break;
                    }

                    //now do it for columns
                    var xWingTestCols = colSets.Where(clmn => clmn.Count() == xWingSize).ToArray();
                    foreach (var xWingValues in xWingTestCols)
                    {
                        int colCount = xWingValues.Count();
                        var columnsToTest = candidateColumns.Where(clmn => xWingValues.Contains(clmn.Key)).ToArray();
                        if (columnsToTest.Length < colCount)
                        {
                            continue;
                        }

                        var setUnionOfCandidateRows = columnsToTest.SelectMany(clmn => clmn.Select(c => c.Row)).Distinct().ToArray();
                        if (setUnionOfCandidateRows.Length != colCount)
                            continue;

                        var xWingTrimRows = boardRows.Where(r => setUnionOfCandidateRows.Contains(r.Key)).ToArray();

                        foreach (var trimRow in xWingTrimRows)
                        {
                            var trimCells = trimRow.Where(c => !xWingValues.Contains(c.Column) && c.Candidates.Contains(currentValue)).ToArray();
                            foreach (var cell in trimCells)
                            {
                                modified |= cell.Candidates.Remove(currentValue);
                            }
                        }
                        break;
                    }
                }

            }

            return modified;
        }
    }
}
