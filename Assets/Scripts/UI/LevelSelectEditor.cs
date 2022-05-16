using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSelect))]
public class LevelSelectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Reset layout"))
        {
            var levelSelect = serializedObject.targetObject as LevelSelect;

            DestroyExistingButtons(levelSelect);
            SetButtonPositions(levelSelect);
        }
    }

    private static void DestroyExistingButtons(LevelSelect levelSelect)
    {
        var currentButtons = levelSelect.GetComponentsInChildren<ChangeSceneButton>();
        foreach (ChangeSceneButton button in currentButtons)
            DestroyImmediate(button.gameObject);
    }

    private static void SetButtonPositions(LevelSelect levelSelect)
    {
        RectTransform rectTransform = levelSelect.GetComponent<RectTransform>();
        Vector2 position = new Vector2();
        int numOnLine = 0;
        foreach (Level level in levelSelect.m_Levels)
        {
            ChangeSceneButton button = Instantiate(levelSelect.m_Prefab, Vector3.zero, Quaternion.identity);
            button.Initialise(level);
            button.transform.SetParent(rectTransform);
            button.GetComponent<RectTransform>().anchoredPosition = position;

            position.x += levelSelect.m_Offset.x;
            numOnLine++;

            if (numOnLine >= levelSelect.m_LevelsPerLine)
            {
                numOnLine = 0;

                position.x = 0;
                position.y -= levelSelect.m_Offset.y;
            }
        }
    }
}
