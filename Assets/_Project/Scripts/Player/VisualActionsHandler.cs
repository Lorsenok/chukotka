using UnityEngine;

public static class VisualActionsHandler
{
    public static void Action(VisualAction va)
    {
        CameraMovement.Shake(va.shakePower);
    }
}
