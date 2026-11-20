using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IGameManager
{

    public ManagerStatus status { get; private set; }

    public int enemyHealth { get; private set; }
    public int enemymaxHealth { get; private set; }

    public List<GameObject> activeEnemies = new List<GameObject>();

    private NetworkService network;
    public void Startup(NetworkService service)
    {
        Debug.Log("Enemy manager starting...");

        network = service;

        

        // any long-running startup tasks go here, and set status to 'Initializing' until those tasks are complete
        status = ManagerStatus.Started;
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!activeEnemies.Contains(enemy)) activeEnemies.Add(enemy);
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
    }

    
    
}
