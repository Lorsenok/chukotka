using UnityEngine;

public class GameLayerSwitchBlock : MonoBehaviour
{
    public static bool Block { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Controler>())
        {
            Block = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Controler>())
        {
            Block = false;
        }
    }
}
