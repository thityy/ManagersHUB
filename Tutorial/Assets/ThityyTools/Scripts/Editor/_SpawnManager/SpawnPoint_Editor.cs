using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ManagersHUB
{
    [CustomEditor(typeof(SpawnPoint))]
    public class SpawnPoint_Editor : BaseCustomEditor
    {
        private SerializedProperty m_IsUseForInit;          //Bool
        private SerializedProperty m_InitTeam;              //Enum
        private SerializedProperty m_BecameNormalSpawn;     //Bool
        private SerializedProperty m_InitSquad;             //Enum
        private SerializedProperty m_UseSquadInit;          //Bool
        private SphereCollider m_HostileZone;               //Collider
        private SerializedProperty m_HostileSize;

        private LayerMask m_GroundLayer;

        private SpawnPoint m_SpawnPoint;

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            SetText();
            GetDatas();
            ShowGUI();
            UpdateProperty();
        }

        private void SetText()
        {
            if (!EditorTextIsSet())
            {
                NewEditorText(new string[]
                {
                    "Spawn",
                    "Use for initialisation spawn",
                    "Activate this option if you want this point to be use for an initialisation spawn.<b> " + ColorManager.GetColorText("Initialisation spawn", eManagers.SpawnManager, false) + "</b> are use to instantiate user the first time the scene is load.",
                    "Use squad for initialisation",
                    "Activate this option if you want the initialisation spawn to use <b>" + ColorManager.GetColorText("squad selection", eManagers.SpawnManager, false) + "</b>. [Add a level of precision to tell which user can spawn on it]",
                    "Reset Spawn",
                    "Enable this option if you want this point to be <b>" + ColorManager.GetColorText("reset to a normal spawn", eManagers.SpawnManager, false) + "</b> after the initialisation phase.",
                    "Select the init team",
                    "Select the team that will be able to use this point to spawn during the initialisation phase.",
                    "Select the init squad",
                    "Select the squad that will be able to use this point to spawn during the initialisation phase.",
                    "Disable with initialisation spawn",
                    "Hostile Zone",
                    "Hostile Radius",
                    "Set the radius of the hostile zone. Hostile zone trigger user, then sorts them, then tell other user if the spawn is <b>" + ColorManager.GetColorText("safe or hostile", eManagers.SpawnManager, false) + "</b>.",
                    "Environment",
                    "Layer(s) to stick on",
                    "Stick",
                    "Stick this point on the ground so users won't spawn in the ground or in the air.",
                    "Debugging",
                    "Show spawn point",
                    "Show hostile zone"
                }
                , true);

                NewEditorText(new string[]
                {
                    "Spawn",
                    "Utilisé pour le spawn initial",
                    "Activé cette option si vous désirez utiliser ce point pour un spawn d'initialisation. Les <b>" + ColorManager.GetColorText("spawn d'initialisation", eManagers.SpawnManager, false) + "</b> sont utilisé pour instancier les utilisateurs la première fius la scène est chargé.",
                    "Utilisé l'escouade pour le spawn initial",
                    "Activé cette option si vous désirez que le spawn d'initialisation utilise la <b>" + ColorManager.GetColorText("selection d'escouade", eManagers.SpawnManager, false) + "</b>. [Ajoute un niveau de précision pour dire qu'elles utilisateurs peut utiliser ce spawn]",
                    "Réinitialiser le spawn",
                    "Activé cette option si vous désirez que ce point sois <b>" + ColorManager.GetColorText("réinitialiser à un spawn standard", eManagers.SpawnManager, false) + "</b> suite à la phase d'initialisation.",
                    "Sélectionnez l'équipe d'initialisation",
                    "Sélectionnez l'équipe qui pourra utiliser ce point pour spawn lors de la phase d'initialisation.",
                    "Sélectionnez l'escouade d'initialisation",
                    "Sélectionnez l'escouade qui pourra utiliser ce point pour spawn lors de la phase d'initialisation.",
                    "Désactivé avec le spawn d'initialisation",
                    "Zone Hostile",
                    "Rayon Hostile",
                    "Défini le rayon de la zone hostile. La zone hostile détecte les utilisateurs, les trie, et finalement dit aux autres utilisateurs si le spawn est <b>" + ColorManager.GetColorText("sécuritaire ou hostile", eManagers.SpawnManager, false) + "</b>.",
                    "Environnement",
                    "Layer(s) à coller",
                    "Coller",
                    "Colle ce point sur le sol pour que les utilisateurs ne spawn pas dans le sol ou dans les airs.",
                    "Débogage",
                    "Afficher les spawns",
                    "Afficher les zones hostiles"
                }
                , false);
            }
        }

        private void GetDatas()
        {
            m_SpawnPoint = (SpawnPoint)target;
            GetData(ref m_IsUseForInit, nameof(m_IsUseForInit));
            GetData(ref m_InitTeam, nameof(m_InitTeam));
            GetData(ref m_BecameNormalSpawn, nameof(m_BecameNormalSpawn));
            GetData(ref m_UseSquadInit, nameof(m_UseSquadInit));
            GetData(ref m_InitSquad, nameof(m_InitSquad));
            GetData(ref m_HostileSize, nameof(m_HostileSize));
            m_HostileZone = m_SpawnPoint.gameObject.GetComponent<SphereCollider>();
        }

        private void ShowGUI()
        {

            BeginInspector(eManagers.SpawnManager, "Spawn Point");
            ShowGroup("", ShowInitOption);
            DrawSeparator(true);
            ShowGroup("", ShowHostileZone);
            DrawSeparator(true);
            ShowGroup("", ShowEnvironnementSettings);
            DrawSeparator(true);
            ShowGroup("", ShowDebugOptions);

            EditorGUILayout.EndVertical();

        }

        private void ShowInitOption()
        {
            ShowTitle(GetEditorText(0), eManagers.SpawnManager);
            //ShowAdvancedLabelCenter(m_InUseTxt.SpawnOption);
            ShowSpawnOption();

            if (m_IsUseForInit.boolValue)
            {
                DrawSeparatorInvisible(true);
                ShowSelectTeam();
                DrawSeparatorInvisible(true);
                ShowSelectSquad();
            }
        }

        private void ShowSpawnOption()
        {
            m_IsUseForInit.boolValue = ShowAdvancedToggle(GetEditorText(1), m_IsUseForInit.boolValue);
            ShowAdvancedToolTip(GetEditorText(2));
            if (!m_IsUseForInit.boolValue)
            {
                m_BecameNormalSpawn.boolValue = false;
                m_UseSquadInit.boolValue = false;
            }
            DrawSeparatorInvisible(true);

            EditorGUI.BeginDisabledGroup(!m_IsUseForInit.boolValue);
            m_UseSquadInit.boolValue = ShowAdvancedToggle(GetEditorText(3), m_UseSquadInit.boolValue);
            ShowAdvancedToolTip(GetEditorText(4));
            DrawSeparatorInvisible(true);
            m_BecameNormalSpawn.boolValue = ShowAdvancedToggle(GetEditorText(5), m_BecameNormalSpawn.boolValue);
            ShowAdvancedToolTip(GetEditorText(6));
            EditorGUI.EndDisabledGroup();
        }

        private void ShowSelectTeam()
        {
            ShowAdvancedLabelCenter(GetEditorText(7), m_LabelTxtStyleLeft);
            GUI.color = ColorManager.GetSpawnColor(m_InitTeam.enumValueIndex);
            m_InitTeam.enumValueIndex = ShowAdvancedEnumPopup(m_InitTeam.enumValueIndex, typeof(eSpawnM_Teams), 5);
            GUI.color = Color.white;
            ShowAdvancedToolTip(GetEditorText(8));
        }

        private void ShowSelectSquad()
        {
            EditorGUI.BeginDisabledGroup(!m_UseSquadInit.boolValue);
            ShowAdvancedLabelCenter(GetEditorText(9), m_LabelTxtStyleLeft);
            GUI.color = ColorManager.GetSpawnColor(m_InitSquad.enumValueIndex);
            m_InitSquad.enumValueIndex = ShowAdvancedEnumPopup(m_InitSquad.enumValueIndex, typeof(eSpawnM_Squads), 5);
            GUI.color = Color.white;
            ShowAdvancedToolTip(GetEditorText(10));
            EditorGUI.EndDisabledGroup();
        }

        private void ShowHostileZone()
        {
            if (m_IsUseForInit.boolValue && !m_BecameNormalSpawn.boolValue)
            {
                ShowAdvancedWarningMessage(GetEditorText(11));
            }
            EditorGUI.BeginDisabledGroup(m_IsUseForInit.boolValue && !m_BecameNormalSpawn.boolValue);
            ShowTitle(GetEditorText(12), eManagers.SpawnManager);
            m_HostileSize.floatValue = ShowAdvancedSlider(GetEditorText(13), "Min", "Max", 0.5f, 100.0f, m_HostileSize.floatValue, 3);
            ShowAdvancedToolTip(GetEditorText(14));
            m_HostileZone.radius = m_HostileSize.floatValue;
            m_HostileZone.isTrigger = true;
            m_HostileZone.enabled = false;
            EditorGUI.EndDisabledGroup();
        }

        private void ShowEnvironnementSettings()
        {
            ShowTitle(GetEditorText(15), eManagers.SpawnManager);
            EditorGUILayout.BeginHorizontal();
            LayerMask tempLayers = EditorGUILayout.MaskField(GetEditorText(16), UnityEditorInternal.InternalEditorUtility.LayerMaskToConcatenatedLayersMask(EditorPrefs.GetInt("Spawn.GroundLayer", 0)), UnityEditorInternal.InternalEditorUtility.layers, m_ListBtnSelect);
            EditorPrefs.SetInt("Spawn.GroundLayer", UnityEditorInternal.InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempLayers));
            if (GUILayout.Button(GetEditorText(17), m_ButtonStyle))
            {
                StickSpawnToGround();
            }
            EditorGUILayout.EndHorizontal();
            ShowAdvancedToolTip(GetEditorText(18));
        }

        private void ShowDebugOptions()
        {
            ShowTitle(GetEditorText(19), eManagers.SpawnManager);
            EditorPrefs.SetBool("SpawnPoint_DebugPoint", ShowAdvancedToggle(GetEditorText(20), EditorPrefs.GetBool("SpawnPoint_DebugPoint", false)));
            EditorPrefs.SetBool("SpawnPoint_DebugHostileZone", ShowAdvancedToggle(GetEditorText(21), EditorPrefs.GetBool("SpawnPoint_DebugHostileZone", false)));
        }

        private void StickSpawnToGround()
        {
            RaycastHit hitInfo;
            for (int i = 0; i < 10; i++)
            {
                if (Physics.Raycast(m_SpawnPoint.transform.position + (Vector3.up * i), Vector3.down, out hitInfo, 10f, EditorPrefs.GetInt("Spawn.GroundLayer", 0)))
                {
                    m_SpawnPoint.transform.position = hitInfo.point;
                    m_SpawnPoint.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal.normalized);
                    return;
                }
            }

        }

    }
}


