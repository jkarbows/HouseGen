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
    }
}
