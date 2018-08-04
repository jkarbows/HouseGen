using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;


namespace ProcGenKit.WorldBuilding
{

    public partial class InstancePainterEditor : Editor
    {
        [MenuItem("GameObject/Create Other/Instance Painter")]
        static void CreateInstancePainter()
        {
            var g = new GameObject("Instance Painter", typeof(InstancePainter));
            Selection.activeGameObject = g;
        }

        void RefreshPaletteImages(InstancePainter ip)
        {
            if (palleteImages == null || palleteImages.Length != ip.prefabPallete.Length)
            {
                palleteImages = new Texture2D[ip.prefabPallete.Length];
                for (var i = 0; i < ip.prefabPallete.Length; i++)
                    palleteImages[i] = AssetPreview.GetAssetPreview(ip.prefabPallete[i]);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (ip.rootTransform == null)
            {
                EditorGUILayout.HelpBox("You must assign the root transform for new painted instances.", MessageType.Error);
                ip.rootTransform = (Transform)EditorGUILayout.ObjectField("Root Transform", ip.rootTransform, typeof(Transform), true);
                return;
            }

            // editor ui change check code
            /*using (var check = new EditorGUI.ChangeCheckScope()) {
                base.OnInspectorGUI();
                if (check.changed) {
                    // reset stamp
                }
            }*/

            EditorGUILayout.HelpBox("Stamp: Left Click\nErase: Ctrl + Left Click\nRotate: Shift + Scroll\nBrush Size: Alt + Scroll or [ and ]\nDensity: - =\nSpace: Randomize", MessageType.Info);
            base.OnInspectorGUI();
            GUILayout.Space(16);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PrefixLabel("Align to Normal");
                ip.alignToNormal = GUILayout.Toggle(ip.alignToNormal, GUIContent.none);
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PrefixLabel("Follow Surface");
                ip.followOnSurface = GUILayout.Toggle(ip.followOnSurface, GUIContent.none);
            }
        }

    }
}