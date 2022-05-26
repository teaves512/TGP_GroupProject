using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AchievementSavable
{

    [SerializeField] private int Objects;
    public int ObjectsDestroyed => Objects;
    public void AddObjectsDestroyed ()=> Objects += 1;
    

    [SerializeField] private int EnemiesKilled;
    public int EnemiesSpliffed => EnemiesKilled;
    public void AddEnemiesSpliffed ()=> EnemiesKilled += 1;
    
    
    [SerializeField] private float DistanceWalked;
    public float Distance 
    {
        get => DistanceWalked;
        set => DistanceWalked += value;
    }
    
    [SerializeField] private int BombsDropped;
    public int Bombs => BombsDropped;
    public void AddBombsDropped()=> BombsDropped += 1;
    

    public AchievementSavable()
    {
        Objects = 0;
        EnemiesKilled = 0;
        DistanceWalked = 0;
        BombsDropped = 0;
    }
    
    
    
}