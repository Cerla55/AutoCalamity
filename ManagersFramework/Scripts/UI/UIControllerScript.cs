using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIControllerScript : MonoBehaviour
{
    [SerializeField] TMP_Text healthLabel;
    [SerializeField] InventoryPopup popup;
    [SerializeField] TMP_Text levelEnding;

    void OnEnable()
    {
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.AddListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, OnLevelComplete);
        Messenger.AddListener(GameEvent.GAME_COMPLETE, OnGameComplete);
    }
    void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.RemoveListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, OnLevelComplete);
        Messenger.RemoveListener(GameEvent.GAME_COMPLETE, OnGameComplete);
    }

    // Start is called before the first frame update
    void Start()
    {
        OnHealthUpdated();

        levelEnding.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isShowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isShowing);
            popup.Refresh();
        }
    }

    private void OnHealthUpdated()
    {
        Debug.Log("OnHealthUpdated fired");

        if (healthLabel == null)
        {
            Debug.LogError("healthLabel is NULL");
            return;
        }

        if (Managers.Player == null)
        {
            Debug.LogError("Managers.Player is NULL");
            return;
        }

        Debug.Log("Both references are valid");

        healthLabel.text =
            $"Health: {Managers.Player.health}/{Managers.Player.maxHealth}";
    }

    void Awake()
    {
        Debug.Log($"UI running on: {gameObject.name} | healthLabel null? {healthLabel == null}");
    }

    private void OnLevelComplete()
    {
        StartCoroutine(CompleteLevel());
    }
    private IEnumerator CompleteLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Complete";

        yield return new WaitForSeconds(2);

        Managers.Mission.GoToNext();
    }

    private void OnLevelFailed()
    {
        StartCoroutine(FailLevel());
    }

    private IEnumerator FailLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Failed";
        yield return new WaitForSeconds(2);
        Managers.Player.Respawn();
        Managers.Mission.RestartCurrentLevel();
    }

    private void OnGameComplete()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "You Finished the Game!";
    }

    public void SaveGame()
    {
        Managers.Data.SaveGameState();
    }

    public void LoadGame()
    {
        Managers.Data.LoadGameState();
    }
}
