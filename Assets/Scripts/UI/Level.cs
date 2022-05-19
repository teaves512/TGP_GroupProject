using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    [SerializeField] public string scene;
    [SerializeField] public string levelName;
    [SerializeField] public Sprite previewImage;
}
