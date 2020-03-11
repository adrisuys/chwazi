using UnityEngine;
using UnityEngine.UI;

public class ToggleBehaviour : MonoBehaviour
{
    public bool isOn;
    public Color onColor;
    public Color offColor;
    public Image button;
    public Image circle;
    public float speed = 1.0f;
    private bool isSwitching = false;
    private float t = 0;
    private const int FAR_RIGHT = 30;
    private const int FAR_LEFT = -113;

    public delegate void OnToggleSwitched();
    public static event OnToggleSwitched ToggleSwitched;

    private void Update()
    {
        if (isSwitching)
        {
            Switch();
        }
    }

    private void Switch()
    {
        if (isOn)
        {
            circle.transform.localPosition = MoveCircle(FAR_RIGHT, FAR_LEFT);
        }
        else
        {
            circle.transform.localPosition = MoveCircle(FAR_LEFT, FAR_RIGHT);
        }
    }

    private Vector3 MoveCircle(float start, float end)
    {
        Vector3 pos = new Vector3(Mathf.Lerp(start, end, t += speed * Time.deltaTime), 0, 0);
        if (t > 1)
        {
            isSwitching = false;
            t = 0;
            if (start < end)
            {
                isOn = true;
                button.GetComponent<Image>().color = onColor;
            }
            else
            {
                isOn = false;
                button.GetComponent<Image>().color = offColor;
            }
            ToggleSwitched();
        }
        return pos;
    }

    public void OnButtonClicked()
    {
        isSwitching = true;
    }

    public void SetValue(bool isSoundOn)
    {
        isOn = isSoundOn;
        button.GetComponent<Image>().color = isOn ? onColor : offColor;
        circle.transform.localPosition = isOn ? new Vector3(FAR_RIGHT, 0, 0) : new Vector3(FAR_LEFT, 0, 0);
    }
}
