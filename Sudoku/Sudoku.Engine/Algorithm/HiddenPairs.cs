using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Engine.Algorithm
{
    /*
     * The algorithm. Look at every CellGroup. Evaluate sets of size 1 through of 8.
     * The iteration of the set size is N. Get every possible combination of distinct cell values of size N
     * 
     * So
     * 
     * 
     * 
     * 
     */
    public class HiddenSets : SudokuAlgorithmBase
    {  

        protected override bool DoCrank(ref SudokuBoard board)
        {
            bool modified = false;

            //key - the size of the set, value a list of all of the sets
            var powerSet = this.GetPowerSet(SudokuBoard.CELL_VALUES.ToList());
            var setGroups = powerSet.GroupBy(the => the.Length).Where(it => it.Key > 0).OrderBy(it => it.Key).ToArray();

            var rowsDictionary = board.Cells.ToLookup(c => c.Row, c => c);
            var colsDictionary = board.Cells.ToLookup(c => c.Column, c => c);
            var chunkDictionary = board.Cells.ToLookup(c => c.Chunk, c => c);

            foreach (var sets in setGroups)
            {
                int setSize = sets.Key;
                foreach (int groupNumber in SudokuBoard.CELL_VALUES)
                {
                    var row = rowsDictionary[groupNumber];
                    var column = colsDictionary[groupNumber];
                    var chunk = chunkDictionary[groupNumber];
                    var groups = new IEnumerable<Cell>[] { row, column, chunk};
                    foreach (var group in groups)
                    {
                        var validSets = sets.Where(set => set.All(elem => !group.Any(cell => cell.Value.Equals(elem)))).ToArray();
                        foreach (var set in validSets)
	                    {
                            var candidates = group.Where(c => c.Candidates.Any(cnd => set.Contains(cnd))).ToArray();
                            if (candidates.Length <= setSize)
                            {
                                foreach (var candidateCell in candidates)
                                {
                                    int eliminated = candidateCell.Candidates.RemoveWhere(cnd => !set.Contains(cnd));
                                    modified |= eliminated > 0;
                                    if (candidateCell.Candidates.Count == 1)
                                    {
                                        int value = candidateCell.Candidates.FirstOrDefault();
                                        candidateCell.Candidates = new HashSet<int>();
                                        var c = candidateCell;
                                        this.SetValue(value, ref c, ref board);
                                        modified = true;
                                    }
                                }
                            }
	                    }
                    }
                }
                
            }
            return modified;
        }
    }
}
