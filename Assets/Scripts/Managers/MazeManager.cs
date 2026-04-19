using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public static MazeManager Instance;

    [SerializeField] private int thresholdForMaze = 501;
    [SerializeField] private GameObject mazeGameObject;
    
    
    private MovieScreens movieScreens;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        
        else
            Destroy(gameObject);

        movieScreens = FindAnyObjectByType<MovieScreens>();
    }

    void Update()
    {
        if(PlayerController.Instance.killCounter >= thresholdForMaze)
        {
            EnableMaze();
            PlayLightBlinkingAnim();
        }
    }

    private void EnableMaze()
    {
        mazeGameObject.SetActive(false);
        Spawner.Instance.spawningEnabled = false;
    }

    private void PlayLightBlinkingAnim()
    {
        
    }
}
