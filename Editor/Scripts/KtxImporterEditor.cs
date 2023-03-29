// Copyright (c) 2019-2022 Andreas Atteneder, All Rights Reserved.

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEditor;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace KtxUnity
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
