using UnityEngine;

public static class ProjMath
{
    public static float EaseOutBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x - 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x - 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x - 2.625f / d1) * x + 0.984375f;
        }
    }

    public static float EaseInBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }

    public static float EaseOutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }

    public static float RotateTowardsPosition(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    public static Vector3 MoveTowardsAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0);
    }

    public static float SinTime(float m = 1f, bool canBeNegative = false)
    {
        if (!canBeNegative) return Mathf.Sin(Time.timeSinceLevelLoad * m) * (Mathf.Sin(Time.timeSinceLevelLoad * m) > 0 ? 1f : -1f);
        else return Mathf.Sin(Time.timeSinceLevelLoad * m);
    }

    public static Vector2 MousePosition()
    {
        if (Camera.main == null)
        {
            Debug.LogError("No main camera found");
            return Vector2.zero;
        }
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
    }
}
