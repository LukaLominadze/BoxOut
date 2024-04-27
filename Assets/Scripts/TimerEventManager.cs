using UnityEngine;
using UnityEngine.Events;

public class TimerEventManager : MonoBehaviour
{
    [SerializeField] UnityEvent warning, seaRise, sink;

    private int elapsedTime = 0;

    void FixedUpdate()
    {
        elapsedTime = LevelManager.Singleton.GetElapsedSeconds();

        if (elapsedTime == 0) return;

        switch (elapsedTime % 60)
        {
            case 35:
                warning?.Invoke();
                break;
            case 40:
                seaRise?.Invoke();
                break;
            case 0:
                sink?.Invoke();
                break;
        }
    }
}
