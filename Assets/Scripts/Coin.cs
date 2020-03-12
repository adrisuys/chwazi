using UnityEngine;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 10f;
    private int rdm;
    private int nbFlip = 0;
    private bool canFlip = false;
    public delegate void FlippingDone();
    public static event FlippingDone OnFlipDone;

    public void StartFlipping(int random)
    {
        transform.eulerAngles = new Vector3(90, 90, 90);
        nbFlip = 0;
        rdm = random;
        canFlip = true;
    }

    private void Update()
    {
        if (canFlip)
        {
            if (nbFlip < rdm)
            {
                transform.rotation *= Quaternion.Euler(0, 18, 0);
                if (transform.eulerAngles.x == 90 || transform.eulerAngles.x == 270)
                {
                    nbFlip++;
                }
            }
            else
            {
                OnFlipDone();
                canFlip = false;
            }
        }
    }
}
