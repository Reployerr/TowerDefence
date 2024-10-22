using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent EndRound;

    [Header("References")]
    [SerializeField] private GameObject levelCompletedPanel;

    public static LevelManager main;

    [Header("Level Path")]
    public Transform startPoint;
    public Transform [] path;

    private void Awake()
    {
        main = this;
    }

    public void CompletingStage()
	{
        Time.timeScale = 0f;
        AudioListener.pause = true;

        ///back to map logic///
        /////
        ///
	}

}
