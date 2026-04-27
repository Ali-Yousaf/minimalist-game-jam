using UnityEngine;

public class MazeExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Maze Completed!");
            MazeManager.Instance.MazeComplete();
        }
    }
}