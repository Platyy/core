using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {


    private enum CurrentlyPlaying
    {
        TRACK1 = 0,
        TRACK2 = 1,
        TRACK3 = 2
    }

    private CurrentlyPlaying m_CurrentlyPlaying;

    public AudioSource m_BGM1, m_BGM2, m_BGM3;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        int _r = Random.Range(0, 2);
        if (_r == 0)
        {
            m_BGM1.Play();
            m_CurrentlyPlaying = CurrentlyPlaying.TRACK1;
        }
        else
        {
            m_BGM2.Play();
            m_CurrentlyPlaying = CurrentlyPlaying.TRACK2;
        }
    }


    void Update()
    {
        if(!m_BGM1.isPlaying && m_CurrentlyPlaying == CurrentlyPlaying.TRACK1)
        {
            m_BGM3.Play();
            m_CurrentlyPlaying = CurrentlyPlaying.TRACK3;
        }
        else if(!m_BGM2.isPlaying && m_CurrentlyPlaying == CurrentlyPlaying.TRACK2)
        {
            m_BGM3.Play();
            m_CurrentlyPlaying = CurrentlyPlaying.TRACK3;
        }
    }

}
