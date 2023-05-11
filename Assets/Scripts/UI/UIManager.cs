using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] Transform victoryTitle, gameOverTitle;
    [SerializeField] Ease ease;
    [SerializeField] UnityEvent onAnimationCompleted;
    Vector3 basePosition;

    private void Start()
    {
        EventManager.Instance.onVictory.AddListener((int i)=> { VictoryAnimation(i, true); });
        EventManager.Instance.onGameOver.AddListener(()=> { VictoryAnimation(0, false); });
        basePosition = victoryTitle.localPosition;
    }

    public void VictoryAnimation(int i, bool victory = true)
    {
        Transform t = victory ? victoryTitle : gameOverTitle;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(t.DOLocalMove(Vector3.zero, 1f).SetEase(ease));
        sequence.AppendInterval(2f);
        sequence.Append(t.DOLocalMove(basePosition, 1f).SetEase(ease));
    }
}
