using System.Collections.Generic;
using UnityEngine;

public interface IInputControler
{
    InputSystem GetInputSystem();
}

public class InputControlerService : IInputControler
{
    private List<InputSystem> curInputs = new List<InputSystem>();
    
    public InputSystem GetInputSystem()
    {
        InputSystem input = new InputSystem();
        input.Enable();
        curInputs.Add(input);
        SceneChangerService.OnSceneChange += OnSceneSwitch;
        
        return input;
    }

    private void OnSceneSwitch(string scene)
    {
        foreach (InputSystem i in curInputs)
        {
            i.Disable();
        }
        curInputs.Clear();
    }
}
