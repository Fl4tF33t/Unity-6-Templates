using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    //Use this class as a Globals class
    //Where you need information across scenes or scripts

    public static string playerName = "Player";
    public static bool gameStarted = false;
    public static int score = 0;
    public static float health = 100f;
    public static int[] intArray = new int[]{};
    public static List<float> floatList = new List<float>();

    public enum GameStates
    {
        Start,
        End
    }
    public enum Collectable
    {
        Coin,
        Gem,
        Heart,
    }

    public static void DoSomething()
    {
        Debug.Log("Ensure that something is happening");
    }
}
