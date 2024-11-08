using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;


public class CameraControllUI : MonoBehaviour
{

    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;
    public UnityEvent<Vector2> onIsDrag;
    public UnityEvent<float> onOperateZoom;

    private List<int> touchIndeces;
    private float initDistance;
    private void Awake()
    {
        touchIndeces = new List<int>(2);
        initDistance = 0f;
    }
    void Update()
    {
        if (Touchscreen.current == null)
            return;

        DetectTouch();
        CameraControll();
    }
    public void DetectTouch()
    {
        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            if (Touchscreen.current.touches[i].press.wasPressedThisFrame == false)
                continue;

            Vector2 touchPosition = Touchscreen.current.touches[i].position.ReadValue();

            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = touchPosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            if (results.Count <= 0)
                return;
            if (results[0].gameObject == this.gameObject)
            {

                if (touchIndeces.Count < 2)
                    touchIndeces.Add(i);

                return;
            }

        }

    }
    public void CameraControll()
    {
        if (touchIndeces.Count <= 0)
            return;

        Touchscreen touchscreen = Touchscreen.current;

        for (int i = 0; i < touchIndeces.Count; i++)
        {
            if (touchscreen.touches[touchIndeces[i]].press.wasReleasedThisFrame == true)
            {
                initDistance = 0f;
                touchIndeces.RemoveAt(i);
                return;
            }
        }

        if (touchIndeces.Count == 1 && touchscreen.touches[touchIndeces[0]].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            Rotate(touchscreen.touches[touchIndeces[0]]);
        }
        else if (touchIndeces.Count == 2)
        {
            TouchControl firstTouch = touchscreen.touches[touchIndeces[0]];
            TouchControl secondTouch = touchscreen.touches[touchIndeces[1]];

            if(firstTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved && 
                secondTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(firstTouch.position.ReadValue(), secondTouch.position.ReadValue());

                float DifferenceDistance = currentDistance - initDistance;

                onOperateZoom?.Invoke(DifferenceDistance);
                Debug.Log(DifferenceDistance);
                initDistance = currentDistance;
            }

        
        }
    }

    public void Rotate(TouchControl touchControl)
    {

        Vector2 touchDelta = touchControl.delta.ReadValue();



        onIsDrag?.Invoke(touchDelta);


    }
}
