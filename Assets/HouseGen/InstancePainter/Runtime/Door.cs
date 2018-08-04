using UnityEngine;

namespace ProcGenKit.WorldBuilding
{
    public class Door : CellPassage
    {
        public Transform hinge;

        private Door OtherSide
        {
            get
            {
                return otherCell.GetEdge(direction.GetOpposite()) as Door;
            }
        }

        public override void Initialize (BaseCell primary, BaseCell other, CompassDirection direction)
        {
            base.Initialize(primary, other, direction);
            if (OtherSide != null)
            {
                // do something with the transform I guess
            }
        }
    }
}
