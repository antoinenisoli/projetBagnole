using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingAnimation : MonoBehaviour
{
    [SerializeField] Transform _transform;
    [SerializeField] Ease ease;
    [SerializeField] float startFloatingValue, endFloatingValue = 3f, floatingDuration = 0.3f;

    private void Awake()
    {
        _transform.localPosition = new Vector3(_transform.localPosition.x, startFloatingValue, _transform.localPosition.z);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_transform.DOLocalMoveY(endFloatingValue, floatingDuration/2).SetEase(ease));
        sequence.Append(_transform.DOLocalMoveY(startFloatingValue, floatingDuration/2).SetEase(ease));
        sequence.SetLoops(-1, LoopType.Restart);
    }
}
