using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ManagersHUB
{
    [CustomEditor(typeof(SpawnUser))]
    public class SpawnUser_Editor : BaseCustomEditor
    {
        private SerializedProperty m_UserTeam;              //Enum
        private SerializedProperty m_UserSquad;             //Enum
        private SerializedProperty m_RadiusNeededToSpawn;   //Float
        private SerializedProperty m_AutoSpawn;             //Bool
        private SerializedProperty m_AutoSpawnType;         //Enum

        private int m_ShowTip = 0;

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
                    "Debugging",
                    "Select gameplay type ( for tips only )",
                    "Solo", 
                    "Coop", 
                    "Multiplayer", 
                    "User",
                    "Select the <i>team</i> of the user.", 
                    "If you're doing a <b>" + ColorManager.GetColorText("'one player'", eManagers.SpawnManager, true) + "</b> experience, just select 'Team01' for the player GameObject. You can next choose the others teams for ennemis.",
                    "If you're doing a <b>" + ColorManager.GetColorText("'coop'", eManagers.SpawnManager, true) + "</b> experience, just select 'Team01' for all players GameObject. You can next choose the others teams for ennemis.",
                    "If you're doing a <b>" + ColorManager.GetColorText("'multiplayer pvp'", eManagers.SpawnManager, true) + "</b> experience, you'll probably need to edit player team by code when matchmaking are done, so you can skip this step.",
                    "Select the <i>squad</i> of the user.", 
                    "If you're doing a <b>" + ColorManager.GetColorText("'one player'", eManagers.SpawnManager, true) + "</b> experience, you can select the squad to divide NPC Allies or/and NPC Ennemis into different divison.",
                    "If you're doing a <b>" + ColorManager.GetColorText("'coop'", eManagers.SpawnManager, true) + "</b> experience, you can select a different squad for each players, so you can chose where they spawn more accurately with 'Spawn_InitGame' function.",
                    "If you're doing a <b>" + ColorManager.GetColorText("'multiplayer pvp'", eManagers.SpawnManager, true) + "</b> experience, you'll probably need to edit player squad by code when matchmaking are done, so you can skip this step.",
                    "Select the user <i>radius</i>.", 
                    "Radius",
                    "Select the radius needed for the user to be able to spawn. [<b>" + ColorManager.GetColorText("To high radius", eManagers.SpawnManager, true) + "</b> : Will block the user to spawn] [<b>" + ColorManager.GetColorText("To small radius", eManagers.SpawnManager, true) + "</b> : Will make the user to spawn into other user]",
                    "Automating",
                    "Enable/Disable the automatic spawn",
                    "Automatic Spawn",
                    "The automatic spawn will use a spawn point <b>" + ColorManager.GetColorText("each time", eManagers.SpawnManager, true) + "</b> the object get activate [OnEnable]. Get also called when the object get instantiated.",
                    "Select the automatic spawn <i>type</i>",
                    "The spawn type allows you to decide how the player/enemy/NPC/Object will appear in game during the automatic spawn."
                }
                , true);

                NewEditorText(new string[]
                {
                    "Débogage",
                    "Sélectionnez le type de jeu ( pour astuces seulement )",
                    "Solo",
                    "Coop",
                    "Multijoueur",
                    "Utilisateur",
                    "Sélectionnez l'<i>équipe</i> de l'utilisateur.",
                    "Si vous créez une expérience <b>" + ColorManager.GetColorText("'un joueur'", eManagers.SpawnManager, true) + "</b>, simplement sélectionnez 'Team01' pour le joueur. Vous pouvez par la suite choisir les autres équipes pour les ennemis",
                    "Si vous créez une expérience <b>" + ColorManager.GetColorText("'coopérative'", eManagers.SpawnManager, true) + "</b>, simplement sélectionnez 'Team01' pour tout les joueurs. Vous pouvez par la suite choisir les autres équipes pour les ennemis",
                    "Si vous créez une expérience <b>" + ColorManager.GetColorText("'multijoueur jcj'", eManagers.SpawnManager, true) + "</b>, vous devrez probablement modifier les équipes des joueurs par code suite au matchmaking, alors vous pouvez passer cette étape.",
                    "Sélectionnez l'<i>escouade</i> de l'utilisateur.",
                    "Si vous créez une expérience <b>" + ColorManager.GetColorText("'un joueur'", eManagers.SpawnManager, true) + "</b>, vous pouvez sélectionnez des escouades pour diviser les alliées et ennemis en différentes divisions.",
                    "Si vous créez une expérience <b>" + ColorManager.GetColorText("'coopérative'", eManagers.SpawnManager, true) + "</b>, vous pouvez sélectionnez différentes escouades pour chaque joueur, vous permettant ainsi de faire apparraître les joueurs de façon plus précise à l'aide de la fonction 'Spawn_InitGame'",
                    "Si vous créez une expérience <b>" + ColorManager.GetColorText("'multijoueur jcj'", eManagers.SpawnManager, true) + "</b>, vous devrez probablement modifier les escouades des joueurs par code suite au matchmaking, alors vous pouvez passer cette étape.",
                    "Sélectionnez le <i>rayon</i> de l'utilisateur",
                    "Rayon",
                    "Sélectionnez le rayon néccéssaire pour que le joueur puisse 'spawn' en jeu. [<b>" + ColorManager.GetColorText("Rayon trop grand", eManagers.SpawnManager, true) + "</b> : Va empêcher l'utilisateur d'apparraître en jeu] [<b>" + ColorManager.GetColorText("Rayon trop petit", eManagers.SpawnManager, true) + "</b> : Va faire apparraître l'utilisateur dans un autre utilisateur]",
                    "Automatisation",
                    "Activez/Désactivez le <i>spawn automatique</i>",
                    "Spawn automatique",
                    "Le spawn automatique permet d'utiliser un point de spawn à <b>" + ColorManager.GetColorText("chaque fois", eManagers.SpawnManager, true) + "</b> que l'objet est activé [OnEnable]. Aussi appelé lorsque l'objet est instancié.",
                    "Sélectionnez le <i>type</i> de spawn automatique",
                    "Le type de spawn permet de décider comment le joueur/ennemi/PNJ/objet va apparaître en jeu lors du spawn automatique",
                }
                , false);
            }
        }

        private void GetDatas()
        {
            GetData(ref m_UserTeam, nameof(m_UserTeam));
            GetData(ref m_UserSquad, nameof(m_UserSquad));
            GetData(ref m_RadiusNeededToSpawn, nameof(m_RadiusNeededToSpawn));
            GetData(ref m_AutoSpawn, nameof(m_AutoSpawn));
            GetData(ref m_AutoSpawnType, nameof(m_AutoSpawnType));
        }

        private void ShowGUI()
        {

            BeginInspector(eManagers.SpawnManager, "Spawn User");

            ShowGroup("", ShowDebugSettings);
            DrawSeparator(true);
            ShowGroup("", ShowPlayerSettings);
            DrawSeparator(true);
            ShowGroup("", ShowAutoSpawn);

            EditorGUILayout.EndVertical();

        }

        private void ShowDebugSettings()
        {
            ShowTitle(GetEditorText(0), eManagers.SpawnManager);
            ShowAdvancedLabelCenter(GetEditorText(1), m_LabelTxtStyleLeft);
            m_ShowTip = ShowAdvancedBtnChoice(new string[] { GetEditorText(2), GetEditorText(3), GetEditorText(4) }, m_ShowTip, 2);
        }

        private void ShowPlayerSettings()
        {
            ShowTitle(GetEditorText(5), eManagers.SpawnManager);
            ShowTeamSelection();
            DrawSeparatorInvisible(true);
            ShowSquadSelection();
            DrawSeparatorInvisible(true);
            ShowRadiusSelection();
        }


        private void ShowTeamSelection()
        {
            ShowAdvancedLabelCenter(GetEditorText(6), m_LabelTxtStyleLeft);
            GUI.color = ColorManager.GetSpawnColor(m_UserTeam.enumValueIndex);
            m_UserTeam.enumValueIndex = ShowAdvancedEnumPopup(m_UserTeam.enumValueIndex, typeof(eSpawnM_Teams), 5);
            GUI.color = Color.white;
            if (m_ShowTip == 0)
            {
                ShowAdvancedToolTip(GetEditorText(7));
            }
            else if (m_ShowTip == 1)
            {
                ShowAdvancedToolTip(GetEditorText(8));
            }
            else
            {
                ShowAdvancedToolTip(GetEditorText(9));
            }


        }

        private void ShowSquadSelection()
        {
            ShowAdvancedLabelCenter(GetEditorText(10), m_LabelTxtStyleLeft);
            GUI.color = ColorManager.GetSpawnColor(m_UserSquad.enumValueIndex);
            m_UserSquad.enumValueIndex = ShowAdvancedEnumPopup(m_UserSquad.enumValueIndex, typeof(eSpawnM_Squads), 5);
            GUI.color = Color.white;
            if (m_ShowTip == 0)
            {
                ShowAdvancedToolTip(GetEditorText(11));
            }
            else if (m_ShowTip == 1)
            {
                ShowAdvancedToolTip(GetEditorText(12));
            }
            else
            {
                ShowAdvancedToolTip(GetEditorText(13));
            }

        }

        private void ShowRadiusSelection()
        {
            ShowAdvancedLabelCenter(GetEditorText(14), m_LabelTxtStyleLeft);
            m_RadiusNeededToSpawn.floatValue = ShowAdvancedSlider(GetEditorText(15), "Min", "Max", 0.1f, 100.0f, m_RadiusNeededToSpawn.floatValue, 3);
            ShowAdvancedToolTip(GetEditorText(16));
        }

        private void ShowAutoSpawn()
        {
            ShowTitle(GetEditorText(17), eManagers.SpawnManager);
            ShowAdvancedLabelCenter(GetEditorText(18), m_LabelTxtStyleLeft);
            m_AutoSpawn.boolValue = ShowAdvancedToggle(GetEditorText(19), m_AutoSpawn.boolValue);
            ShowAdvancedToolTip(GetEditorText(20));

            DrawSeparatorInvisible(true);

            EditorGUI.BeginDisabledGroup(!m_AutoSpawn.boolValue);
            ShowAdvancedLabelCenter(GetEditorText(21), m_LabelTxtStyleLeft);
            m_AutoSpawnType.enumValueIndex = ShowAdvancedEnumPopup(m_AutoSpawnType.enumValueIndex, typeof(eSpawnM_Types), 5);
            ShowAdvancedToolTip(GetEditorText(22));
            EditorGUI.EndDisabledGroup();

            ShowUsefulManagerMessage(eManagers.PoolManager);
        }
    }
}


