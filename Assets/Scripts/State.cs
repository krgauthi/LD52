using System;
using Unidux;
using UnityEngine;

[Serializable]
public class State : StateBase
{
    public State(int initSheepCount, int initTime)
    {
        this.sheepCount = initSheepCount;
        this.timeLeft = initTime;
    }

    public Vector3 playerPosition;
    public int sheepCount;
    public int score = 0;
    public int timeLeft;
    public bool gameOver = false;

}