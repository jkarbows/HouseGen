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

        public void Merge (Room room)
        {
            for (int i = 0; i < room.cells.Count; i++)
            {
                Add(room.cells[i]);
            }
        }

        public void Show()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].Show();
            }
        }

        public void Hide()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].Hide();
            }
        }
    }
}
