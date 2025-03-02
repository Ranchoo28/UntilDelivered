using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float moveSpeed = 50f;
    private float edgeThreshold = 100f;
    private float screenEdgeSpeedMultiplier = 1f;

    [SerializeField] private CameraController cameraController;
    private Rigidbody rb;

    private float minX = -300f;
    private float maxX = 300f;
    private float minZ = -300f;
    private float maxZ = 300f;

    private bool isUsingWASD = false; // Stato per WASD
    private bool isUsingMouseEdge = false; // Stato per il tasto 1 e movimento ai bordi

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody non trovato sul Placeholder!");
        }
        else if (!rb.isKinematic)
        {
            Debug.LogWarning("Rigidbody dovrebbe essere impostato come Kinematic!");
        }
    }

    void FixedUpdate()
    {
        if (cameraController == null || rb == null) return;

        CameraController.CameraDirection currentDirection = cameraController.GetCurrentDirection();

        // Calcola il movimento solo in base alla modalità attiva
        Vector3 move = Vector3.zero;

        // Verifica quale modalità è attiva
        if (IsWASDPressed() && !isUsingMouseEdge)
        {
            isUsingWASD = true;
            isUsingMouseEdge = false;
            move = CalculateMovement(currentDirection);
        }
        else if (Input.GetMouseButton(1) && !isUsingWASD)
        {
            isUsingMouseEdge = true;
            isUsingWASD = false;
            move = GetEdgeMovement(currentDirection);
        }

        // Calcola la nuova posizione
        Vector3 newPosition = rb.position + move;

        // Applica i limiti di movimento
        newPosition = ClampPositionToBounds(newPosition);

        // Muove il Rigidbody
        rb.MovePosition(newPosition);
    }

    private bool IsWASDPressed()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private Vector3 GetEdgeMovement(CameraController.CameraDirection direction)
    {
        Vector3 move = Vector3.zero;

        if (Input.mousePosition.x <= edgeThreshold)
        {
            move.x -= moveSpeed * screenEdgeSpeedMultiplier * Time.fixedDeltaTime;
        }
        else if (Input.mousePosition.x >= Screen.width - edgeThreshold)
        {
            move.x += moveSpeed * screenEdgeSpeedMultiplier * Time.fixedDeltaTime;
        }

        if (Input.mousePosition.y <= edgeThreshold)
        {
            move.z -= moveSpeed * screenEdgeSpeedMultiplier * Time.fixedDeltaTime;
        }
        else if (Input.mousePosition.y >= Screen.height - edgeThreshold)
        {
            move.z += moveSpeed * screenEdgeSpeedMultiplier * Time.fixedDeltaTime;
        }

        return AdjustMovementByDirection(move, direction);
    }

    private Vector3 CalculateMovement(CameraController.CameraDirection direction)
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 localMove = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.fixedDeltaTime;
        return AdjustMovementByDirection(localMove, direction);
    }

    private Vector3 AdjustMovementByDirection(Vector3 localMove, CameraController.CameraDirection direction)
    {
        switch (direction)
        {
            case CameraController.CameraDirection.North:
                return new Vector3(-localMove.x, 0, -localMove.z);
            case CameraController.CameraDirection.West:
                return new Vector3(localMove.z, 0, -localMove.x);
            case CameraController.CameraDirection.East:
                return new Vector3(-localMove.z, 0, localMove.x);
            case CameraController.CameraDirection.South:
                return new Vector3(localMove.x, 0, localMove.z);
            default:
                return localMove;
        }
    }

    private Vector3 ClampPositionToBounds(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.z = Mathf.Clamp(position.z, minZ, maxZ);
        return position;
    }

    void Update()
    {
        // Se nessun tasto è premuto, resetta gli stati
        if (!IsWASDPressed() && !Input.GetMouseButton(1))
        {
            isUsingWASD = false;
            isUsingMouseEdge = false;
        }
    }
}
