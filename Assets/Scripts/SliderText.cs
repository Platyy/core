using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderText : MonoBehaviour {

    public Slider m_Slider;
    private Text m_Text;

    void Start()
    {
        m_Text = GetComponent<Text>();
    }
    void Update()
    {
        m_Text.text = m_Slider.value.ToString();
    }

}
