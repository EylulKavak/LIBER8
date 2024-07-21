using UnityEngine;
using UnityEngine.InputSystem;


public class InputsHandler : MonoBehaviour
{
    [HideInInspector] public PlayerInputs playerInputs;
    Movement movement;
    DialogueTriggerer dialogueTriggerer;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        dialogueTriggerer = GetComponent<DialogueTriggerer>();
    }
    public void SetInputs(bool enable)
    {
        if (enable)
        {
            playerInputs.Movement.Enable();
            playerInputs.Movement.Interact.performed += Interact_performed;
            playerInputs.Movement.Run.performed += Run_performed;
            playerInputs.Movement.Run.canceled += Run_performed;
            playerInputs.Movement.Move.performed += Move_performed;
            playerInputs.Movement.Move.canceled += Move_performed;
        }
        else
        {
            playerInputs.Movement.Interact.performed -= Interact_performed;
            playerInputs.Movement.Run.performed -= Run_performed;
            playerInputs.Movement.Run.canceled -= Run_performed;
            playerInputs.Movement.Move.performed -= Move_performed;
            playerInputs.Movement.Move.canceled -= Move_performed;
            playerInputs.Movement.Disable();
        }
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        if (dialogueTriggerer.enabled)
        {
            dialogueTriggerer.StartDialgoue();
        }
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        movement.horizontal = obj.ReadValue<Vector2>().x;
        movement.vertical = obj.ReadValue<Vector2>().y;
    }

    private void Run_performed(InputAction.CallbackContext obj)
    {
        if (obj.performed) movement.RunPerformed();
        if (obj.canceled) movement.RunCancled();
    }

    private void OnEnable()
    {
        playerInputs = new PlayerInputs();
        SetInputs(true);
    }
    private void OnDisable()
    {
        SetInputs(false);
    }
}
