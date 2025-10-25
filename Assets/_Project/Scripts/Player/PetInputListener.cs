using UnityEngine;
using UnityEngine.InputSystem;

public class PetInputListener : MonoBehaviour
{
    [SerializeField] private Pet pet; 

    private InputSystem input;

    private void Start()
    {
        input = new InputSystem();
        input.Enable();
    }
    private void OnEnable()
    {
        if (input == null)
        {
            input = new InputSystem();
            input.Enable();
        }

        input.Player.Feed.performed += OnFeedPressed;
    }
    private void OnDisable()
    {
        input.Player.Feed.performed -= OnFeedPressed;
    }
    private void OnFeedPressed(InputAction.CallbackContext context)
    {
        if (pet != null)
        {
            pet.Action();
        }
    }
}