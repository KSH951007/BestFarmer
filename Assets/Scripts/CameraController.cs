using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera playerCam;
    private CinemachineFramingTransposer framingTransposer;
    private float rotateSpeed;
    private float maxDistance;
    private float minDistance;
    private float zoomSpeed;
    private void Awake()
    {
        rotateSpeed = 0.2f;
        maxDistance = 10f;
        minDistance = 3f;
        zoomSpeed = 2f;
        playerCam = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = playerCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    public void RotateCamera(Vector2 touchDelta)
    {

        Quaternion currentRotation = playerCam.transform.localRotation;
        Vector2 delta = new Vector2(touchDelta.x * rotateSpeed, touchDelta.y * rotateSpeed);

        // �θ��� ���� Y��(X�� �巡�� ��)�� X��(Y�� �巡�� ��)�� ȸ���ϵ��� �մϴ�.
        Quaternion yRotation = Quaternion.AngleAxis(delta.x, Vector3.up);   // Y�� ȸ��
        Quaternion xRotation = Quaternion.AngleAxis(delta.y, Vector3.right); // X�� ȸ��

        // �� ȸ���� �����ϰ�, Z���� �����մϴ�.
        Quaternion targetRotation = currentRotation * yRotation * xRotation;
        Vector3 targetEulerAngles = targetRotation.eulerAngles;
        //  targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, 0);

        targetEulerAngles.x = Mathf.Clamp(targetEulerAngles.x, 10f, 45f);
        //targetEulerAngles.y = Mathf.Clamp(targetEulerAngles.y, -180f, 180f);
        targetEulerAngles.z = 0; // Z�� ����
        playerCam.transform.localRotation = Quaternion.Euler(targetEulerAngles);

    }

    public void OperationCameraZoom(float distance)
    {
        framingTransposer.m_CameraDistance = Mathf.Clamp(framingTransposer.m_CameraDistance - distance * zoomSpeed, minDistance, maxDistance);

    }
}
