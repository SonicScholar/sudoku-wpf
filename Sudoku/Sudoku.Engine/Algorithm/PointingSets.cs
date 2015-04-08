using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Engine.Algorithm
{
    public class PointingSets : SudokuAlgorithmBase
    {
        /*
         * 
         * For each cell value 1 - 9
         *  - Get candidate cells for each row
         *    + do candidate cells share same chunk?
         *      + if yes, grab candidate cells for that chunk and remove current cell value for candidates
         *        not in that row
         *      
         *  - Get candidate cells for each column
         *    + do candidate cells share same chunk?
         *      + if yes, grab candidate cells for that chunk and remove current cell value for candidates
         *        not in that column
         *      
         *  - Get candidate cells for each chunk
         *    + do candidate cells share the same row?
         *      + if yes, grab candidate cells for that row and remove current cell value for candidates
         *        not in that chunk
         *    + do candidate cells share the same column?
         *      + if yes, grab candidate cells for that column and remove current cell value for candidates
         *        not in that chunk
         *    
         * 
         */
        protected override bool DoCrank(ref SudokuBoard board)
        {
            var boardRows = board.Cells.GroupBy(c => c.Row).ToArray();
            var boardCols = board.Cells.GroupBy(c => c.Column).ToArray();
            var boardChunks = board.Cells.GroupBy(c => c.Chunk).ToArray();

            bool modified = false;

            foreach (int currentValue in SudokuBoard.CELL_VALUES)
            {
                //chunk
                foreach (var chunk in boardChunks)
                {
                    var candidates = chunk.Where(c => c.Candidates.Contains(currentValue)).ToArray();
                    if (candidates.Length < 2)
                        continue;
                    var testCandidate = candidates.FirstOrDefault();
                    bool shareRow = candidates.All(c => c.Row.Equals(testCandidate.Row));
                    bool shareCol = candidates.All(c => c.Column.Equals(testCandidate.Column));

                    if (shareRow)
                    {
                        var row = boardRows.FirstOrDefault(r => r.Key.Equals(testCandidate.Row));
                        var trimCells = row.Where(c => c.Chunk != testCandidate.Chunk && c.Candidates.Contains(currentValue));
                        foreach (var trimCell in trimCells)
                        {
                            int eliminated = trimCell.Candidates.RemoveWhere(i => i.Equals(currentValue));
                            modified |= eliminated > 0;
                        }
                    }
                    if (shareCol)
                    {
                        var col = boardCols.FirstOrDefault(clmn => clmn.Key.Equals(testCandidate.Column));
                        var trimCells = col.Where(c => c.Chunk != testCandidate.Chunk && c.Candidates.Contains(currentValue));
                        foreach (var trimCell in trimCells)
                        {
                            int eliminated = trimCell.Candidates.RemoveWhere(i => i.Equals(currentValue));
                            modified |= eliminated > 0;
                        }
                    }
                    
                }

                //row
                foreach (var row in boardRows)
                {
                    var candidates = row.Where(c => c.Candidates.Contains(currentValue)).ToArray();
                    if (candidates.Length < 2)
                        continue;
                    var testCandidate = candidates.FirstOrDefault();
                    bool shareChunk = candidates.All(c => c.Chunk.Equals(testCandidate.Chunk));
                    if (shareChunk)
                    {
                        var chunk = boardChunks.FirstOrDefault(ch => ch.Key.Equals(testCandidate.Chunk));
                        var trimCells = chunk.Where(c => c.Row != testCandidate.Row && c.Candidates.Contains(currentValue));
                        foreach (var trimCell in trimCells)
                        {
                            int eliminated = trimCell.Candidates.RemoveWhere(i => i.Equals(currentValue));
                            modified |= eliminated > 0;
                        }
                    }

                }

                //col
                foreach (var col in boardCols)
                {
                    var candidates = col.Where(c => c.Candidates.Contains(currentValue)).ToArray();
                    if (candidates.Length < 2)
                        continue;
                    var testCandidate = candidates.FirstOrDefault();
                    bool shareChunk = candidates.All(c => c.Chunk.Equals(testCandidate.Chunk));
                    if (shareChunk)
                    {
                        var chunk = boardChunks.FirstOrDefault(ch => ch.Key.Equals(testCandidate.Chunk));
                        var trimCells = chunk.Where(c => c.Column != testCandidate.Column && c.Candidates.Contains(currentValue));
                        foreach (var trimCell in trimCells)
                        {
                            int eliminated = trimCell.Candidates.RemoveWhere(i => i.Equals(currentValue));
                            modified |= eliminated > 0;
                        }
                    }
                }
            }
            return modified;
        }
    }
}
