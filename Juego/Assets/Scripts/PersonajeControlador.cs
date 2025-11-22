using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PersonajeControlador : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float acceleration = 14f;

    [Header("Salto y Gravedad")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity;
    private float currentSpeed;
    private bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // -------- SUELO --------
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // -------- INPUT --------
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // -------- CORRER --------
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        controller.Move(move * currentSpeed * Time.deltaTime);

        // -------- SALTO --------
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // -------- GRAVEDAD --------
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ==============================================
        //              ROTACIÓN ESTILO FORTNITE
        //          La cámara decide el frente del personaje
        // ==============================================
        RotateWithCamera();
    }

    void RotateWithCamera()
    {
        // Dirección plana de la cámara
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;

        if (camForward.sqrMagnitude < 0.1f) return;

        // Rotación suave
        Quaternion targetRotation = Quaternion.LookRotation(camForward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
}
