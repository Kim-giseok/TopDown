using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController player { get; private set; }
    private ResourceController _playerRController;

    [SerializeField] private int currentStageIndex = 0;
    [SerializeField] private int currentWaveIndex = 0;

    private EnemyManager enemyManager;

    UIManager uiManager;
    public static bool isFirstLoading = true;

    CameraShake cameraShake;

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

        cameraShake = FindObjectOfType<CameraShake>();
        MainCameraShake();
    }

    public void MainCameraShake()
    {
        cameraShake.ShakeCamera(1, 1, 1);
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
        //StartNextWave();
        StartStage();
    }

    void StartNextWave()
    {
        currentWaveIndex += 1;
        uiManager.ChangeWave(currentWaveIndex);
        enemyManager.StartWave(1 + currentWaveIndex / 5);
    }

    public void EndOfWave()
    {
        //StartNextWave();
        StartNextWaveInStage();
    }

    public void GameOver()
    {
        uiManager.SetGameOver();
        enemyManager.StopWave();
    }

    public void StartStage()
    {
        StageInfo stageInfo = GetStageInfo(currentStageIndex);

        if (stageInfo == null)
        {
            Debug.Log("�������� ������ �����ϴ�.");
            return;
        }

        uiManager.ChangeWave(currentStageIndex + 1);

        enemyManager.StartStage(stageInfo.waves[currentWaveIndex]);
    }

    public void StartNextWaveInStage()
    {
        StageInfo stageInfo = GetStageInfo(currentStageIndex);
        if (stageInfo.waves.Length - 1 > currentWaveIndex)
        {
            currentWaveIndex++;
            StartStage();
        }
        else
        {
            CompleteStage();
        }
    }

    public void CompleteStage()
    {
        currentStageIndex++;
        currentWaveIndex = 0;
        StartStage();
    }

    private StageInfo GetStageInfo(int stageKey)
    {
        foreach (var stage in StageData.Stages)
        {
            if (stage.stageKey == stageKey) return stage;
        }
        return null;
    }
}
