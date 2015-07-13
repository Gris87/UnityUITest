using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class EventsTestScript : MonoBehaviour
{
    private Image mImage;

    // Use this for initialization
    void Start()
    {
        mImage = GetComponent<Image>();
    }

    public void turnToRed()
    {
        mImage.color = Color.red;
    }

    public void turnToWhite()
    {
        mImage.color = Color.white;
    }
}
