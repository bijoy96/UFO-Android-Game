using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    int levelID=1;
    public float timeLeft;
    int foodCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLevel(int lvl)
    {
        levelID = lvl;
    }

    public int GetLevel()
    {
        return levelID;
    }

    public float GetTime()
    {
        timeLeft = 20/levelID;
        return timeLeft;
    }

    public int GetFoodCount()
    {

        foodCount = levelID * 2;
        return foodCount;
    }
}
