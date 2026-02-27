using System.Collections;
using UnityEngine;

public class HitPause : MonoBehaviour
{
    public static HitPause Instance;

    private void Awake()
    {
        Instance = this;
    }

    public  void Pause(float duration, float TimeScale = 0f)
    {
        Instance.StartCoroutine(Instance.PauseRoutine(duration, TimeScale));
    }

    private IEnumerator PauseRoutine(float duration, float TimeScale)
    {
        float originalTimeScale = Time.timeScale;

        Time.timeScale = TimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = 0.02f;
    }
}
