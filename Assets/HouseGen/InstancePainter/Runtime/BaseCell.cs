using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGenKit.WorldBuilding
{
    public class BaseCell : MonoBehaviour
    {
        public IntVector2 coordinates;
        public Room room;
        private CellEdge[] edges = new CellEdge[CompassDirections.Count];
        private int initializedEdgeCount;

        public void Initialize (Room room)
        {
            room.Add(this);
            // set the room's wall/floor/ceiling prefabs
        }

        public CellEdge GetEdge (CompassDirection direction)
        {
            return edges[(int)direction];
        }

        public void SetEdge (CompassDirection direction, CellEdge edge)
        {
            edges[(int)direction] = edge;
            initializedEdgeCount += 1;
        }

        public bool IsFullyInitialized
        {
            get
            {
                return initializedEdgeCount == CompassDirections.Count;
            }
        }

        public CompassDirection RandomUninitializedDirection
        {
            get
            {
                int skips = Random.Range(0, CompassDirections.Count - initializedEdgeCount);
                for (int i = 0; i < CompassDirections.Count; i++)
                {
                    if (edges[i] == null)
                    {
                        if (skips == 0)
                        {
                            return (CompassDirection)i;
                        }
                        skips -= 1;
                    }
                }
                throw new System.InvalidOperationException("BaseCell has no uninitialized directions left!");
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }

    public abstract class CellEdge : MonoBehaviour
    {
        public BaseCell cell, otherCell;
        public CompassDirection direction;

        public virtual void Initialize (BaseCell cell, BaseCell otherCell, CompassDirection direction)
        {
            this.cell = cell;
            this.otherCell = otherCell;
            this.direction = direction;
            cell.SetEdge(direction, this);
            transform.parent = cell.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = direction.ToRotation();
        }
    }
}
