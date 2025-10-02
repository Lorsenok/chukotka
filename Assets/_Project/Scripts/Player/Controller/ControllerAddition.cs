using UnityEngine;

public class ControllerAddition : MonoBehaviour
{
    public int Addition { get; set; } = 0;
    public bool Block { get; set; } = false;
    public Vector2 AdditionalSpeed { get; set; } = Vector2.zero;
    public Vector2 SpeedMultiplier { get; set; } = new Vector2(1f, 1f);
}
