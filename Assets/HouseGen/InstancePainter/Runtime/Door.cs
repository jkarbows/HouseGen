using UnityEngine;

namespace ProcGenKit.WorldBuilding
{
    public class Door : CellPassage
    {
        public Transform hinge;
        private bool isMirrored;
        private static Quaternion
            normalRotation = Quaternion.Euler(0f, -90f, 0f),
            mirroredRotation = Quaternion.Euler(0f, 90f, 0f);

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
                isMirrored = true;
            }
        }

        public void Open()
        {
            OtherSide.hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
            OtherSide.cell.room.Show();
        }
    }
}
