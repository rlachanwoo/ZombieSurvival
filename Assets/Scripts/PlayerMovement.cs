using UnityEngine;
using UnityEngine.Analytics;

public class PlayerMovement : MonoBehaviour
{
    public static readonly int HashMove = Animator.StringToHash("Move");

    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;

    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()   // 성능이 허용되는 한 많이 호출 time.deltaTime을 씀
    {
        playerAnimator.SetFloat(HashMove, playerInput.Move);
    }

    private void FixedUpdate()  // 가능한 정해진 간격으로 호출 / 물리관련된 일을 매 프레임마다 하려면 FixedUpdate
    {
        float angle = playerInput.Rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(0f, angle, 0f));

        Vector3 delta = playerInput.Move * transform.forward * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + delta);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            IItem item = other.GetComponent<IItem>();
            if (item != null)
            {
                item.Use(gameObject);
            }
        }
    }
}
