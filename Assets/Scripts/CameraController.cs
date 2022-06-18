using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed=0.5f;
    [SerializeField] private float rotateSpeed=0.5f;
    [SerializeField] private CharacterController characterController;

    private void FixedUpdate()
    {
        // Rotate around y - axis
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

        // Move forward / backward
        var forward = transform.TransformDirection(Vector3.forward);
        var curSpeed = speed * Input.GetAxis("Vertical");
        characterController.SimpleMove(forward * curSpeed);
        
        /*if (Input.GetKey(KeyCode.W))
        {
            characterController.Move(transform.forward * speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            characterController.Move(-transform.forward * speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            characterController.Move(-transform.right * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            characterController.Move(transform.right * speed);
        }
        
        var h = speed * Input.GetAxis("Mouse X");
        var v = -speed * Input.GetAxis("Mouse Y");
        var rot = (h * Vector3.up) + (v* Vector3.right);
        transform.Rotate(rot);*/
        
    }
}