using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public class BoardChangedEventArgs : EventArgs
    {
        private List<Tuple<int, int>>positionsChanged = new List<Tuple<int,int>>();

        public void Add(Tuple<int, int> pos)
        {
            this.positionsChanged.Add(pos);
        }

        public List<Tuple<int,int>> PositionsChanged
        {
            get
            {
                return this.positionsChanged;
            }
        }
    }


}
