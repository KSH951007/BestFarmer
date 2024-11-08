using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    #region Compenet

    private PlayerInput inputs;
    private Animator animator;
    private Rigidbody rb;
    [SerializeField] private CinemachineVirtualCamera playerCam;
    #endregion
    private Vector2 moveDir;
    public float rotateSpeed { get; private set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rotateSpeed = 0.2f;


    }
    private void OnEnable()
    {
        inputs = new PlayerInput();
        if (inputs != null)
        {
            inputs.Enable();
        }
    }

    private void OnDisable()
    {
        if (inputs != null)
        {
            inputs.Disable();
        }
    }
  
    void FixedUpdate()
    {
        Move();

    }

    
    void Rotate(Vector3 moveDirection)
    {
        Vector3 forward = transform.forward;

        forward.y = 0f;


        float angle = Vector3.SignedAngle(transform.forward, moveDirection, Vector3.up);
        Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.up);


        transform.rotation *= angleAxis;

    }
    void Move()
    {
        moveDir = inputs.Player.Move.ReadValue<Vector2>();
        moveDir.Normalize();
        animator.SetFloat("MoveLength", Vector3.Magnitude(moveDir));



        Vector3 forward = playerCam.transform.forward;
        Vector3 right = playerCam.transform.right;

        // 높이(위치) 축의 영향을 제거
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // 이동 벡터 계산
        Vector3 moveDirection = (forward * moveDir.y + right * moveDir.x).normalized;

        Rotate(moveDirection);
        // 캐릭터 이동
        //animator.SetFloat("MoveDirX", moveDirection.x);
        //animator.SetFloat("MoveDirY", moveDirection.z);
        
        rb.MovePosition(rb.transform.position + moveDirection * 5 * Time.deltaTime);
    }
}
