// SPDX-FileCopyrightText: 2023 Unity Technologies and the KTX for Unity authors
// SPDX-License-Identifier: Apache-2.0


using UnityEditor;
using UnityEditor.AssetImporters;

namespace KtxUnity.Editor
{
    [CustomEditor(typeof(KtxImporter))]
    class KtxImporterEditor : ScriptedImporterEditor
    {
        SerializedProperty m_ReportItems;

        public override void OnEnable()
        {
            base.OnEnable();
            m_ReportItems = serializedObject.FindProperty("reportItems");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var reportItemCount = m_ReportItems.arraySize;
            for (int i = 0; i < reportItemCount; i++)
            {
                EditorGUILayout.HelpBox(m_ReportItems.GetArrayElementAtIndex(i).stringValue, MessageType.Error);
            }

            ApplyRevertGUI();
        }
    }
}
