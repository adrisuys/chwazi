using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MobileTouchHandler : MonoBehaviour
{
    private Vector2 startPosition;
    private Vector2 endPosition;
    private AudioSource audioSourcePick;
    private AudioSource audioSourceTap;
    public GameLogic logic;
    public Image backArrow;

    private void Start()
    {
        Screen.SetResolution(1080, 2280, true);
        audioSourcePick = GetComponents<AudioSource>()[0];
        audioSourceTap = GetComponents<AudioSource>()[1];
        Camera.main.backgroundColor = PlayerPrefs.GetInt("darkMode", 1) == 1 ? Color.black : Color.white;
        backArrow.color = PlayerPrefs.GetInt("darkMode", 1) == 1 ? Color.white : Color.black;
    }

    private void Update()
    {
        if (!logic.DelayPassed)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    logic.SaveTouch(touch);
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    logic.DeleteTouch(touch);
                }
                else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    logic.UpdateTouch(touch);
                }
            }
            logic.CheckForResult();
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayPickSound()
    {
        if (CrossSceneInfos.soundIsOn) audioSourcePick.Play();
    }

    public void PlayTapSound()
    {
        if (CrossSceneInfos.soundIsOn) audioSourceTap.Play();
    }
}
