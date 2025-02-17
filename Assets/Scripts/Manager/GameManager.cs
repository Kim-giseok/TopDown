using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController player { get; private set; }
    private ResourceController _playerRController;

    [SerializeField] private int currentWaveIndex = 0;

    private EnemyManager enemyManager;

    UIManager uiManager;
    public static bool isFirstLoading = true;

    void Awake()
    {
        instance = this;
        player = FindObjectOfType<PlayerController>();
        player.Init(this);

        uiManager = FindObjectOfType<UIManager>();

        _playerRController = player.GetComponent<ResourceController>();
        _playerRController.RemoveHealthChangeEvent(uiManager.ChangePlayerHP);
        _playerRController.AddHealthChangeEvent(uiManager.ChangePlayerHP);

        enemyManager = GetComponentInChildren<EnemyManager>();
        enemyManager.Init(this);
    }

    void Start()
    {
        if (!isFirstLoading)
            StartGame();
        else
            isFirstLoading = false;
    }

    public void StartGame()
    {
        uiManager.SetPlayGame();
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWaveIndex += 1;
        uiManager.ChangeWave(currentWaveIndex);
        enemyManager.StartWave(1 + currentWaveIndex / 5);
    }

    public void EndOfWave()
    {
        StartNextWave();
    }

    public void GameOver()
    {
        uiManager.SetGameOver();
        enemyManager.StopWave();
    }

}
