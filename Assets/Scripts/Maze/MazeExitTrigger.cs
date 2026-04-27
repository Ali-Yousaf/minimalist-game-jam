using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Maze Completed!");
            MazeManager.Instance.MazeComplete();
            DialougeManager.Instance.ShowDialogue("You completed the maze!!", 4f, true, DialougeManager.DialogueColorType.Blue);

            StartCoroutine(LoadMenu());
        }
    }

    private IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("MainMenu");
    }
}