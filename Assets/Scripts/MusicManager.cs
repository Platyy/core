using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioSource m_BGM1, m_BGM2, m_BGM3;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        int _r = Random.Range(0, 2);
        if (_r == 0)
            m_BGM1.Play();
        else
            m_BGM2.Play();
    }


}
