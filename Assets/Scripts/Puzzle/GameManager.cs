using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.onGameOver.AddListener(() => { StartCoroutine(GameOver()); });

        EventManager.Instance.onVictory.AddListener(
            (int index) =>
            {
                StartCoroutine(Victory(index));
            }
            );
    }

    IEnumerator Victory(int index)
    {
        yield return new WaitForSeconds(5f);
        LoadNextLevel();
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5f);
        Reload();
    }

    public void Reload() //called by button
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCount)
            Reload();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }
}
