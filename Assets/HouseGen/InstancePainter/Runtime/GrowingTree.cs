using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProcGenKit.WorldBuilding
{
    public class GrowingTree : GeneratorAlgorithm
    {
        public IntVector2 size;
        public BaseCell[,] cells;
        public List<Room> rooms = new List<Room>();

        public override GameObject Generate(InstancePainter instancePainter, GameObject newStamp)
        {
            base.Generate(instancePainter, newStamp);
            // todo: repeat for each floor, 2x2
            // each cell is 4x4
            size.x = ip.brushRadius / 4;
            size.z = ip.brushRadius / 4;
            cells = new BaseCell[size.x, size.z];
            List<BaseCell> activeCells = new List<BaseCell>();
            IntVector2 coordinates = RandomCoordinates;
            // do the first generation step so we have something to iterate from
            PerformFirstGenerationStep(activeCells);
            while (activeCells.Count > 0)
            {
                PerformNextGenerationStep(activeCells);
            }
            return stamp;
        }

        private void PerformFirstGenerationStep(List<BaseCell> activeCells)
        {
            BaseCell newCell = CreateCell(RandomCoordinates);
            newCell.Initialize(CreateRoom(-1));
            activeCells.Add(newCell);
        }

        private void PerformNextGenerationStep(List<BaseCell> activeCells)
        {
            int index = activeCells.Count - 1;
            BaseCell currentCell = activeCells[index];
            if (currentCell.IsFullyInitialized)
            {
                activeCells.RemoveAt(index);
                return;
            }
            CompassDirection direction = currentCell.RandomUninitializedDirection;
            IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
            if (ContainsCoordinates(coordinates))
            {
                BaseCell neighbor = GetCell(coordinates);
                if (neighbor == null)
                {
                    neighbor = CreateCell(coordinates);
                    CreatePassage(currentCell, neighbor, direction);
                    activeCells.Add(neighbor);
                }
                // if we match on room.settingsIndex instead we can merge same rooms
                else if (currentCell.room == neighbor.room)
                {
                    CreatePassageInSameRoom(currentCell, neighbor, direction);
                }
                else
                {
                    CreateWall(currentCell, neighbor, direction);
                }
            }
            else
            {
                CreateWall(currentCell, null, direction);
            }
        }

        private BaseCell CreateCell(IntVector2 coordinates)
        {
            var child = new GameObject("Dummy");
            child.transform.parent = stamp.transform;
            child.transform.localPosition = new Vector3(coordinates.x * 4 - size.x * 4 * 0.5f + 0.5f, 0f, coordinates.z * 4 - size.z * 4 * 0.5f + 0.5f);
            child.transform.localEulerAngles = Vector3.zero;
            BaseCell cell = PrefabUtility.InstantiatePrefab(ip.baseCell) as BaseCell;
            foreach (var c in cell.GetComponentsInChildren<Collider>())
                c.enabled = false;
            cells[coordinates.x, coordinates.z] = cell;
            cell.coordinates = coordinates;
            cell.transform.parent = child.transform;
            cell.transform.localPosition = Vector3.zero;
            cell.transform.localRotation = Quaternion.identity;
            // originally set the parent to the maze's transform
            // then set localposition to the new vector3
            // instead of any of the child stuff
            return cell;
        }

        private Room CreateRoom(int indexToExclude)
        {
            Room newRoom = ScriptableObject.CreateInstance<Room>();
            newRoom.settingsIndex = Random.Range(0, ip.roomSettings.Length);
            if (newRoom.settingsIndex == indexToExclude)
            {
                newRoom.settingsIndex = (newRoom.settingsIndex + 1) % ip.roomSettings.Length;
            }
            newRoom.settings = ip.roomSettings[newRoom.settingsIndex];
            rooms.Add(newRoom);
            return newRoom;
        }

        private void CreatePassage(BaseCell cell, BaseCell otherCell, CompassDirection direction)
        {
            CellPassage prefab = Random.value < ip.doorProbability ? ip.doorPrefab : ip.passagePrefab;
            CellPassage passage = Instantiate(prefab) as CellPassage;
            passage.Initialize(cell, otherCell, direction);
            passage = Instantiate(prefab) as CellPassage;
            if (passage is Door)
            {
                otherCell.Initialize(CreateRoom(-1 /*cell.room.settingsIndex*/));
            }
            else
            {
                otherCell.Initialize(cell.room);
            }
            passage.Initialize(otherCell, cell, direction.GetOpposite());
        }

        private void CreatePassageInSameRoom(BaseCell cell, BaseCell otherCell, CompassDirection direction)
        {
            CellPassage passage = Instantiate(ip.passagePrefab) as CellPassage;
            passage.Initialize(cell, otherCell, direction);
            passage = Instantiate(ip.passagePrefab) as CellPassage;
            passage.Initialize(otherCell, cell, direction.GetOpposite());
            if (cell.room != otherCell.room)
            {
                Room roomToMerge = otherCell.room;
                cell.room.Merge(roomToMerge);
                rooms.Remove(roomToMerge);
                Destroy(roomToMerge);
            }
        }

        private void CreateWall(BaseCell cell, BaseCell otherCell, CompassDirection direction)
        {
            CellWall wall = Instantiate(ip.wallPrefab) as CellWall;
            wall.Initialize(cell, otherCell, direction);
            if (otherCell != null)
            {
                wall = Instantiate(ip.wallPrefab) as CellWall;
                wall.Initialize(otherCell, cell, direction.GetOpposite());
            }
        }

        public IntVector2 RandomCoordinates
        {
            get
            {
                return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
            }
        }

        public bool ContainsCoordinates(IntVector2 coordinate)
        {
            return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
        }

        public BaseCell GetCell(IntVector2 coordinates)
        {
            return cells[coordinates.x, coordinates.z];
        }
    }
}