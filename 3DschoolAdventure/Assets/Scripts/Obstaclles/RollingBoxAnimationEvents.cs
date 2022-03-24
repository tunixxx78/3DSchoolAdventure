using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBoxAnimationEvents : MonoBehaviour
{
    [SerializeField] RollingObstacle[] rollingObstacle;
    private void Awake()
    {
        rollingObstacle = GetComponentsInChildren<RollingObstacle>();
    }
    public void TurnCanRotateToTrue()
    {
        foreach (RollingObstacle obstacle in rollingObstacle)
        {
            obstacle.CanRotateToTrue();
        }
        //rollingObstacle.CanRotateToTrue();
    }
    public void TurnAnimationIsPlayingToFalseAgain()
    {
        foreach (RollingObstacle obstacle in rollingObstacle)
        {
            obstacle.animationIsPlaying = false;
        }
    }
}
