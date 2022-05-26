using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager 
{
    public static event UnityAction<bool> GameOver;
    public static void OnGameOver(bool victory) => GameOver?.Invoke(victory);
    
    public static event UnityAction PlayerDroppedBomb;
    public static void OnPlayerDroppedBomb() => PlayerDroppedBomb?.Invoke();
    
    
}
