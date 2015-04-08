using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Engine
{
    public class Cell : INotifyPropertyChanged
    {
        private int _row;
        public int Row 
        {
            get 
            {
                return _row;
            }
            set
            {
                _row = value;
                RaisePropertyChanged();
            } 
        }

        private int _column;
        public int Column
        {
            get
            {
                return _column;
            }
            set
            {
                _column = value;
                RaisePropertyChanged();
            }
        }

        private int _value;
        public int Value { get; set; }

        private HashSet<int> _candidates;
        public HashSet<int> Candidates
        {
            get
            {
                return _candidates;
            }
            set
            {
                _candidates = value;
                RaisePropertyChanged();
            }
        }

        public int Chunk
        {
            get
            {
                int multiplier = ((Row-1) / 3);
                int offset = ((Column-1) / 3) + 1;

                int chunk = (multiplier * 3) + offset;

                return chunk;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
