using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static event Action<Ball> OnBallDeath;

    public void Die()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }
}
