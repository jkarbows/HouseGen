using UnityEngine;

namespace ProcGenKit.WorldBuilding
{

    public enum CompassDirection
    {
        North,
        East,
        South,
        West
    }

    public static class CompassDirections
    {
        public const int Count = 4;

        private static IntVector2[] vectors = {
            new IntVector2(0, 1),
            new IntVector2(1, 0),
            new IntVector2(0, -1),
            new IntVector2(-1, 0)
        };

        private static CompassDirection[] opposites =
        {
            CompassDirection.South,
            CompassDirection.West,
            CompassDirection.North,
            CompassDirection.East
        };

        private static Quaternion[] rotations =
        {
            Quaternion.identity,
            Quaternion.Euler(0f, 90f, 0f),
            Quaternion.Euler(0f, 180f, 0f),
            Quaternion.Euler(0f, 270f, 0f)
        };

        public static CompassDirection RandomValue
        {
            get
            {
                return (CompassDirection)Random.Range(0, Count);
            }
        }

        public static IntVector2 ToIntVector2 (this CompassDirection direction)
        {
            return vectors[(int)direction];
        }

        public static CompassDirection GetOpposite (this CompassDirection direction)
        {
            return opposites[(int)direction];
        }

        public static Quaternion ToRotation (this CompassDirection direction)
        {
            return rotations[(int)direction];
        }
    }
}
