//https://youtu.be/rxa4N4z65pg?si=taIkys733s9CT16q
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 5f,
        movementSpeed = 5f;

    public Vector2 movementLimits;

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Input.GetMouseButton(1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Rotation();
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Movement()
    {
        Vector3 input = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(movementSpeed * Time.deltaTime * input);
    }

    void Rotation()
    {
        Vector3 mouseInput = new(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        transform.Rotate(50 * sensitivity * Time.deltaTime * mouseInput);
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
    }
}
