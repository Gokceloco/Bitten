using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        var direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        var speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }

        MovePlayer(direction, speed);

    }

    void MovePlayer(Vector3 dir, float speed)
    {
        _rb.linearVelocity = dir.normalized * speed;
    }
}
