using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public FunicularController FunicularController { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        FunicularController = FindFirstObjectByType<FunicularController>();
    }

    public void GameOver()
    {
        print("game over man, game over");
        Time.timeScale = 0;
    }
}
