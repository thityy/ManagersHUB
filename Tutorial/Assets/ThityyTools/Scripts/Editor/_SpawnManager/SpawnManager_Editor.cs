using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ManagersHUB
{
    [CustomEditor(typeof(SpawnManager))]
    public class SpawnManager_Editor : BaseCustomEditor
    {
        private struct SpawnManagerTxt
        {
            public SpawnManagerTxt(string a_ManagerFree)
            {
                ManagerFree = a_ManagerFree;

                m_IsInit = true;
            }

            public string ManagerFree;

            private bool m_IsInit;
            public bool IsInit
            {
                get { return m_IsInit; }
            }
        }

        private SpawnManagerTxt m_EnglishTxt;
        private SpawnManagerTxt m_FrenchTxt;
        private SpawnManagerTxt m_InUseTxt;

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            SetText();
            ShowGUI();
            UpdateProperty();
        }

        private void SetText()
        {
            if(!m_EnglishTxt.IsInit)
            {
                m_EnglishTxt = new SpawnManagerTxt("Spawn Manager is settings free. Enjoy!");
                
            }
            if(!m_FrenchTxt.IsInit)
            {
                m_FrenchTxt = new SpawnManagerTxt("Spawn Manager ne contient aucun paramètre. Profitez!");
            }

            if(SceneAutoLoader.IsUsingEnglish)
            {
                m_InUseTxt = m_EnglishTxt;
            }
            else
            {
                m_InUseTxt = m_FrenchTxt;
            }
        }

        private void ShowGUI()
        {

            BeginInspector(eManagers.SpawnManager, "Spawn Manager");

            ShowGroup("", ShowMessage);
            
            EditorGUILayout.EndVertical();

        }

        private void ShowMessage()
        {
            ShowAdvancedLabelCenter(m_InUseTxt.ManagerFree);
            DrawSeparator(false);
        }
    }
}


