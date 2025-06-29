using UnityEngine;

public interface IInputControler
{
    InputSystem GetInputSystem();
}

public class InputControlerService : IInputControler
{
    public InputSystem GetInputSystem()
    {
        InputSystem input = new InputSystem();
        input.Enable();

        return input;
    }
}
