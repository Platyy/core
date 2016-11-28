using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonTween : MonoBehaviour,  ISelectHandler, IDeselectHandler {

    private RectTransform m_Rect;

    void Start()
    {
        m_Rect = GetComponent<RectTransform>();
    }


    public void OnSelect(BaseEventData _eventData)
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(m_Rect.localPosition.x + 60, m_Rect.localPosition.y, 0), "time", 0.5f, "isLocal", true));
    }

    public void OnDeselect(BaseEventData _eventData)
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(m_Rect.localPosition.x - 60, m_Rect.localPosition.y, 0), "time", 0.5f, "isLocal", true));
    }

}
