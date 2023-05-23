using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] GameObject scorePanel;
    [SerializeField] LevelData data;
    [SerializeField] ScoreStar[] stars;
    float gameTimer;

    private void Start()
    {
        EventManager.Instance.onVictory.AddListener(ComputeScore);
    }

    public void ComputeScore(int index)
    {
        scorePanel.SetActive(true);
        int scoreIndex = 0;
        if (gameTimer < data.timerC)
            scoreIndex = 3;
        else if (gameTimer < data.timerB)
            scoreIndex = 2;
        else if (gameTimer < data.timerA)
            scoreIndex = 1;

        for (int i = 0; i < stars.Length; i++)
            stars[i].Set(i <= scoreIndex);
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;
    }
}
