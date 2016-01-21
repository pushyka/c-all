using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    /* A custom eventArgs for the board changed event. This 
    eventArgs maintains a list of positions which have been changed
    on the board. So when the View is told to update, it can only spend
    time updating the positions which have actually been changed, rather 
    than parsing the entirety of the model's board. */
    public class BoardChangedEventArgs : EventArgs
    {
        private List<Tuple<int, int>>positionsChanged = new List<Tuple<int,int>>();


        /* Add a position to the positionsChanged List. */
        public void Add(Tuple<int, int> pos)
        {
            this.positionsChanged.Add(pos);
        }


        /* Return the positionsChanged List. */
        public List<Tuple<int,int>> PositionsChanged
        {
            get
            {
                return this.positionsChanged;
            }
        }
    }
}
