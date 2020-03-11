using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    public ToggleBehaviour toggleSound;
    public ToggleBehaviour toggleDarkMode;
    public Button teamsBtn;
    public Button individualsBtn;
    public Image pickOne;
    public Image pickTeam;
    public Text catchPhrase;

    private void Start()
    {
        toggleSound.SetValue(CrossSceneInfos.soundIsOn);
        toggleDarkMode.SetValue(PlayerPrefs.GetInt("darkMode", 1) == 1);
        if (PlayerPrefs.GetInt("darkMode", 1) == 1)
        {
            SetDarkMode();
        }
        else
        {
            SetLightMode();
        }
        ChooseMode(CrossSceneInfos.mode == Mode.INDIVIDUAL);
    }

    private void OnEnable()
    {
        ToggleBehaviour.ToggleSwitched += OnToggleSwitched;
    }

    private void OnDisable()
    {
        ToggleBehaviour.ToggleSwitched -= OnToggleSwitched;
    }

    private void OnToggleSwitched()
    {
        if (toggleDarkMode.isOn)
        {
            SetDarkMode();
        }
        else
        {
            SetLightMode();
        }
        PlayerPrefs.SetInt("darkMode", toggleDarkMode.isOn ? 1 : 0);
    }

    private void SetDarkMode()
    {
        Camera.main.backgroundColor = Color.black;
    }

    private void SetLightMode()
    {
        Camera.main.backgroundColor = Color.white;
    }

    public void OnNumberOfWinnerPicked(int i)
    {
        CrossSceneInfos.NbOfWinners = i;
        CrossSceneInfos.soundIsOn = toggleSound.isOn;
        SceneManager.LoadScene(1);
    }

    public void ChooseMode(bool isIndividualMode)
    {
        if (isIndividualMode)
        {
            pickOne.gameObject.SetActive(true);
            pickTeam.gameObject.SetActive(false);
            CrossSceneInfos.mode = Mode.INDIVIDUAL;
            catchPhrase.text = "How many picks?";
        }
        else
        {
            pickOne.gameObject.SetActive(false);
            pickTeam.gameObject.SetActive(true);
            CrossSceneInfos.mode = Mode.TEAM;
            catchPhrase.text = "How many team do you want?";
        }
    }
}
