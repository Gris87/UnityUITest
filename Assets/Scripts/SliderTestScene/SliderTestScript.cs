using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class SliderTestScript : MonoBehaviour
{
    public float delay       = 0.1f;
    public float reduceValue = 10f;

    private Slider slider;
    private float  nextRestoreTime = 0f;

    // Use this for initialization
    void Start()
    {
        slider = GetComponent<Slider>();

        nextRestoreTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float curTime = Time.time;

        if (curTime >= nextRestoreTime)
        {
            nextRestoreTime = curTime + delay;
            slider.value++;
        }
    }

    public void OnButtonClick()
    {
        slider.value -= reduceValue;
    }
}
