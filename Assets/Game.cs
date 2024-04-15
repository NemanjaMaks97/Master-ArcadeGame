using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public static float MusicVolume = 1;
    public static float EffectVolume = 1;
    public static int GameDificulty = 0;

    public delegate void PlayerHit(object source, int strength);
    public static event PlayerHit OnPlayerHit;

    public delegate void PlayerHeal(object source);
    public static event PlayerHeal OnPlayerHeal;

    public delegate void EnemyDestroyed(object source, int tier);
    public static event EnemyDestroyed OnEnemyDestroyed;

    public delegate void PlayerDestroyed(object source);
    public static event PlayerDestroyed OnPlayerDestroyed;

    public static void InvokePlayerHit(object source, int strength)
    {
        var handle = OnPlayerHit;
        if (handle != null)
        {
            handle.Invoke(source, strength);
        }
    }

    public static void InvokePlayerHeal(object source)
    {
        var handle = OnPlayerHeal;
        if (handle != null)
        {
            handle.Invoke(source);
        }
    }

    public static void InvokePlayerDestroyed(object source)
    {
        var handle = OnPlayerDestroyed;
        if (handle != null)
        {
            handle.Invoke(source);
        }
    }

    public static void InvokeEnemyDestroyed(object source, int tier)
    {
        var handle = OnEnemyDestroyed;
        if (handle != null)
        {
            handle.Invoke(source, tier);
        }
    }


}
