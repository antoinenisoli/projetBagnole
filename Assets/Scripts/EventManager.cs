using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public class IntEvent : UnityEvent<int> { }
    public class CoordinateEvent : UnityEvent<Vector2Int> { }

    public IntEvent onVictoryCheck = new IntEvent();
    public IntEvent onVictory = new IntEvent();
    public CoordinateEvent onNewMove = new CoordinateEvent();
    public UnityEvent onGameOver = new UnityEvent();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
