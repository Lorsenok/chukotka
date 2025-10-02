using UnityEngine;

public class GameLayerSwitchBlock : MonoBehaviour
{
    public static bool BlockUp { get; private set; } = false;
    public static bool BlockDown { get; private set; } = false;

    [SerializeField] private bool blockUp = true;
    [SerializeField] private bool blockDown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Controller>())
        {
            if (blockUp) BlockUp = true;
            if (blockDown) BlockDown = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Controller>())
        {
            if (blockUp) BlockUp = false;
            if (blockDown) BlockDown = false;
        }
    }
}
