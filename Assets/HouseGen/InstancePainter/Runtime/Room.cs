using UnityEngine;
using System.Collections.Generic;

namespace ProcGenKit.WorldBuilding
{
    public class Room : ScriptableObject
    {
        public int settingsIndex;
        public RoomSettings settings;

        private List<BaseCell> cells = new List<BaseCell>();

        public void Add (BaseCell cell)
        {
            cell.room = this;
            cells.Add(cell);
        }
    }
}
