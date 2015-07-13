using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class SliderValueTestScript : MonoBehaviour
{
    public Slider slider;
    private Text  text;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();

        OnValueChanged(slider.value);
    }

    public void OnValueChanged(float value)
    {
        if (text != null)
        {
            text.text = Mathf.RoundToInt (value * 100).ToString () + " %";
        }
    }
}
