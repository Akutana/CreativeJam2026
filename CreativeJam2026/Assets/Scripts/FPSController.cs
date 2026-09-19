using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor.Rendering;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Player controls")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float mouseSensitivity = 0.1f;
    [SerializeField] Transform cameraTransform;

    [Header("Interaction settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private TextMeshProUGUI interactionPrompt;
    [SerializeField] private TextMeshProUGUI interactionMessage;

    CharacterController controller;
    float verticalVelocity;
    float cameraPitch;

    private bool inputEnabled = true;
    private bool displayingMessages = false;

    private Interactable interactable;
    private KeyCode nextMessageKey;

    private string[] messages;
    private int currentMessageIndex;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!inputEnabled)
            return;

        // Movement
        Vector2 input = Keyboard.current != null
            ? new Vector2(
                (Keyboard.current.dKey.isPressed ? 1 : 0) -
                (Keyboard.current.aKey.isPressed ? 1 : 0),
                (Keyboard.current.wKey.isPressed ? 1 : 0) -
                (Keyboard.current.sKey.isPressed ? 1 : 0))
            : Vector2.zero;

        Vector3 move = transform.right * input.x + transform.forward * input.y;

        // Gravity
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        move.y = verticalVelocity;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Mouse look
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);

            cameraPitch -= mouseDelta.y * mouseSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

            cameraTransform.localRotation =
                Quaternion.Euler(cameraPitch, 0f, 0f);
        }

        if (!displayingMessages)
            CheckInteractionRaycast();
        if (interactable && !displayingMessages)
            CheckInteractionKey();
        if (displayingMessages)
            CheckNextMessageKey();
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
    }

    private void CheckInteractionRaycast()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            interactable = hit.collider.GetComponentInParent<Interactable>();

            if (interactable != null)
            {
                interactionPrompt.text = interactable.GetInteractionText();
                interactionPrompt.gameObject.SetActive(true);

                return;
            }
        }

        interactable = null;
        interactionPrompt.gameObject.SetActive(false);
    }

    private void CheckInteractionKey()
    {
        if (Input.GetKeyDown(interactable.GetInteractionKey()))
        {
            StartInteractionMessages();
        }
    }

    private void CheckNextMessageKey()
    {
        if (Input.GetKeyDown(nextMessageKey))
        {
            currentMessageIndex++;

            if (currentMessageIndex < messages.Length)
            {
                interactionMessage.text = messages[currentMessageIndex];
            }
            else
            {
                interactionMessage.text = "";
                interactionMessage.gameObject.SetActive(false);

                nextMessageKey = KeyCode.None;
                displayingMessages = false;
            }
        }
    }

    private void StartInteractionMessages()
    {
        nextMessageKey = interactable.GetNextMessageKey();
        messages = interactable.GetInteractionMessage();
        currentMessageIndex = 0;

        displayingMessages = true;

        interactionPrompt.gameObject.SetActive(false);
        interactionMessage.gameObject.SetActive(true);

        if (messages.Length > 0)
            interactionMessage.text = messages[currentMessageIndex];
    }
}