using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public int curLevel { get; private set; }
    public int maxLevel { get; private set; }

    private NetworkService network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Mission manager starting....");

        network = service;

        UpdateData(0, 2);

        status = ManagerStatus.Started;

    }

    public void GoToNext()
    {
        if (curLevel < maxLevel)
        {
            curLevel++;
            string name = $"Level{curLevel}";
            Debug.Log($"Loading {name}");
            SceneManager.LoadScene(name);
        }
        else 
        {
            Debug.Log("Last level");
            SceneManager.LoadScene(5);
            Messenger.Broadcast(GameEvent.GAME_COMPLETE);
        }


    }

    public void ReachObjective()
    {
        Messenger.Broadcast(GameEvent.LEVEL_COMPLETE); 
    }

    public void RestartCurrentLevel()
    {
        
        string name = $"Level{curLevel}";
        Debug.Log($"Loading {name}");
        SceneManager.LoadScene(name);
    }

    public void UpdateData(int curLevel, int maxLevel)
    {
        this.curLevel = curLevel;
        this.maxLevel = maxLevel;
    }

    public void DeathSceneLoader()
    {
        SceneManager.LoadScene(6);
    }
}
