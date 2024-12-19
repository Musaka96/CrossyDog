
using UnityEngine.Events;

public class GameManager : MonoSingleton<GameManager>
{
    public static int horizontalSize = 6;

    public static UnityAction onGamePlay;

    public static UnityAction onGamePause;

    private void Start() {
        onGamePlay?.Invoke();
    }
}
