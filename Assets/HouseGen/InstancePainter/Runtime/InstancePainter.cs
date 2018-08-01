using System.Collections;
using UnityEngine;

namespace ProcGenKit.WorldBuilding
{

    [ExecuteInEditMode]
    public class InstancePainter : MonoBehaviour
    {
        public enum CollisionTest
        {
            RendererBounds,
            ColliderBounds
        }

        public LayerMask layerMask;
        public Transform rootTransform;
        [Range(4, 32)]
        public int brushRadius = 16;
        [Range(4, 32)]
        public int brushHeight = 4;
        public float roomDensity = 0.25f;
        public CollisionTest collisionTest;
        [Range(0, 1)]
        public float maxIntersectionVolume = 0;
        [Range(0, 360)]
        [HideInInspector] public float maxSlope = 180;

        [HideInInspector] public bool alignToNormal = true;
        [HideInInspector] public bool followOnSurface = true;
        [HideInInspector] public int selectedPrefabIndex = 0;

        public GameObject[] prefabPallete;
        public BaseCell baseCell;
        public CellPassage passagePrefab;
        public CellWall wallPrefab;

        public GameObject SelectedPrefab
        {
            get
            {
                return prefabPallete == null || prefabPallete.Length == 0 ? null : prefabPallete[selectedPrefabIndex];
            }
        }

        [ExecuteInEditMode]
        void OnValidate()
        {
            brushRadius = (int) Mathf.Floor(brushRadius / 4) * 4;
            brushHeight = (int) Mathf.Floor(brushHeight / 4) * 4;
        }

        [ContextMenu("Delete Children")]
        void DeleteChildren()
        {
            while (transform.childCount > 0) DestroyImmediate(transform.GetChild(0).gameObject);
        }

    }
}