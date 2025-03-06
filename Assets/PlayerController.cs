using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movement = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement = Vector3.down;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement = Vector3.right;
        }

        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
