using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AchievementSavable
{

    [SerializeField] private int Walls;
    public int WallsDestroyed => Walls;
    public void AddWallsDestroyed ()=> Walls += 1;
    

    [SerializeField] private int PlayersKilled;
    public int PlayersSpliffed => PlayersKilled;
    public void AddPlayersSpliffed ()=> PlayersKilled += 1;
    
    
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
        Walls = 0;
        PlayersKilled = 0;
        DistanceWalked = 0;
        BombsDropped = 0;
    }
    
    
    
}