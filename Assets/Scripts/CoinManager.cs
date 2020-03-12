using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public Image backArrow;
    public Coin coin;
    [SerializeField] private int random;
    public Text result;
    public Text intro;

    private void Start()
    {
        Screen.SetResolution(1080, 2280, true);
        Camera.main.backgroundColor = PlayerPrefs.GetInt("darkMode", 1) == 1 ? Color.black : Color.white;
        backArrow.color = PlayerPrefs.GetInt("darkMode", 1) == 1 ? Color.white : Color.black;
        Coin.OnFlipDone += OnFlippingDone;
    }

    private void OnDestroy()
    {
        Coin.OnFlipDone -= OnFlippingDone;
    }

    private void Update()
    {
        if (/*Input.touchCount > */ Input.GetMouseButtonDown(0))
        {
            random = UnityEngine.Random.Range(4, 10);
            intro.text = "";
            coin.StartFlipping(random);
        }
    }

    private void OnFlippingDone()
    {
        if (random % 2 == 0) // heads
        {
            result.text = "HEAD !";
        }
        else
        {
            result.text = "TAIL !";
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
