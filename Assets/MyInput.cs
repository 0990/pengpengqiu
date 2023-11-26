using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyInput : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    public event Action<Vector2> PointerDownEvent;
    public event Action<Vector2> PointerUpEvent;
    public event Action<Vector2> DragEvent;
    private int _curPointerId;
    private const int DefaultPointerId = -100;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Destroy();
    }

    public void Init()
    {
        ResetFingerId();
    }

    public void Destroy()
    {
    }

    private void ResetFingerId()
    {
        _curPointerId = DefaultPointerId;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("MyInput OnPointerDown");
        if (_curPointerId == DefaultPointerId)
        {
            _curPointerId = eventData.pointerId;
            PointerDownEvent?.Invoke(eventData.position);
        }
    }

    public void OnDrag(PointerEventData eventData){
        if (eventData.pointerId == _curPointerId)
        {
            DragEvent?.Invoke(eventData.position);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("MyInput OnPointerUp");
        if (eventData.pointerId == _curPointerId)
        {
            PointerUpEvent?.Invoke(eventData.position);
            ResetFingerId() ;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
