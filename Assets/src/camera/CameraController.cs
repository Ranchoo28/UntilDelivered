using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{
    [Header("Camera References")]
    private CinemachineTransposer transposer;
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform placeholder;

    [Header("Movement Settings")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minFOV = 20f;
    [SerializeField] private float maxFOV = 60f;

    private Vector3 targetFollowOffset;
    private Vector3 lastMousePosition;

    private Boolean isRotating = false;

    public enum CameraDirection
    {
        North,
        West,
        East,
        South
    }

    private CameraDirection currentDirection = CameraDirection.West;

    public CameraDirection GetCurrentDirection()
    {
        return currentDirection;
    }

    private Dictionary<CameraDirection, float[]> directionData = new Dictionary<CameraDirection, float[]>
    {
        { CameraDirection.North, new float[] { 0f, 75f, 180f } },
        { CameraDirection.West, new float[] { -75f, 0f, 90f } },
        { CameraDirection.East, new float[] { 75f, 0f, -90f } },
        { CameraDirection.South, new float[] { 0f, -75f, 0f } }
    };

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera == null) return;
        virtualCamera.m_Lens.FieldOfView = maxFOV;
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = transposer.m_FollowOffset;
    }

    private void Update()
    {
        if (transposer == null) return;

        HandleZooming();

        if (Input.GetKeyDown(KeyCode.R) && !isRotating)
        {
            isRotating = true;
            // Cycle through directions: West -> North -> East -> South -> West
            currentDirection = GetNextDirection(currentDirection);
            SetCameraPosition(currentDirection);
        }
    }

    private IEnumerator SmoothTransition(CinemachineTransposer transposer, float targetX, float targetZ, float targetYRotation)
    {
        Vector3 initialOffset = transposer.m_FollowOffset;
        Quaternion initialRotation = virtualCamera.transform.rotation;

        Vector3 targetOffset = new Vector3(targetX, 75, targetZ);
        Quaternion targetRotation = Quaternion.Euler(45, targetYRotation, 0);

        float duration = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            transposer.m_FollowOffset = Vector3.Lerp(initialOffset, targetOffset, t);
            virtualCamera.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            yield return null;
        }

        transposer.m_FollowOffset = targetOffset;
        virtualCamera.transform.rotation = targetRotation;
        isRotating = false;
    }

    public void SetCameraPosition(CameraDirection direction)
    {
        if (directionData.TryGetValue(direction, out float[] values))
        {
            StartCoroutine(SmoothTransition(transposer, values[0], values[1], values[2]));
        }
    }

    private CameraDirection GetNextDirection(CameraDirection currentDirection)
    {
        switch (currentDirection)
        {
            case CameraDirection.West:
                return CameraDirection.North;
            case CameraDirection.North:
                return CameraDirection.East;
            case CameraDirection.East:
                return CameraDirection.South;
            case CameraDirection.South:
                return CameraDirection.West;
            default:
                return CameraDirection.West;
        }
    }

    private void HandleZooming()
    {
        float scrollDelta = Input.mouseScrollDelta.y;

        if (scrollDelta != 0)
        {
            // Modifica il FOV invece della posizione
            float currentFOV = virtualCamera.m_Lens.FieldOfView;
            float newFOV = currentFOV - scrollDelta * zoomSpeed;
            virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV);
        }
    }

    public void ResetCameraView()
    {
        virtualCamera.m_Lens.FieldOfView = maxFOV;
    }
}
