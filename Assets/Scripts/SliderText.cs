using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SliderText : MonoBehaviour {

    public Slider m_Slider;
    private Text m_Text;
    public bool m_Preset = false;

    private enum SelectedPreset
    {
        DEFAULT,
        SNIPER,
        TACTICAL,
        BULLET_HELL,
        SUDDEN_DEATH
    }

    private SelectedPreset m_SelectedPreset;

    void Start()
    {
        m_Text = GetComponent<Text>();
    }
    void Update()
    {
        if(!m_Preset)
        {
            m_Text.text = m_Slider.value.ToString();
        }
        else
        {
            m_Text.text = ((SelectedPreset)m_Slider.value - 1).ToString();
        }

    }

}
