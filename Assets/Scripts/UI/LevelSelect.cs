using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] public List<Level> m_Levels;
    [SerializeField] public ChangeSceneButton m_Prefab;
    [SerializeField] public Vector2 m_Offset;
    [SerializeField] public int m_LevelsPerLine;
}
