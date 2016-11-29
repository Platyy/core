using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonTween : MonoBehaviour,  ISelectHandler, IDeselectHandler {

    private RectTransform m_Rect;

    private Vector3 m_Start;

    void Start()
    {
        m_Rect = GetComponent<RectTransform>();
        m_Start = m_Rect.localPosition;
    }


    public void OnSelect(BaseEventData _eventData)
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(m_Start.x + 60, m_Rect.localPosition.y, 0), "time", 0.25f, "isLocal", true));
    }

    public void OnDeselect(BaseEventData _eventData)
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(m_Start.x, m_Rect.localPosition.y, 0), "time", 0.25f, "isLocal", true));
    }

}
