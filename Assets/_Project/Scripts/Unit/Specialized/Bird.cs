using System;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Transform CurPoint { get; set; }
    public BirdsController Controller { get; set; }

    [SerializeField] private bool agr = false;
    
    [SerializeField] private DestroyableObject destroyableObject;
    [SerializeField] private float flyHeight = 1f;
    [SerializeField] private float speed = 1f;

    [SerializeField] private Timer switchDelayTimer;
    
    [Header("Animation")]
    [SerializeField] private CustomAnimatorController animController;
    [SerializeField] private string idleAnim;
    [SerializeField] private float idleAnimTime;
    [SerializeField] private string flyAnim;
    [SerializeField] private float flyAnimTime;
    
    private Vector3 GetCurPos(Vector3 start, Vector3 end, float t, float jumpHeight)
    {
        Vector2 linear = Vector2.Lerp(start, end, t);
        float h = 4f * t * (1f - t) * jumpHeight;
        linear.y += h;
        return linear;
    }

    private int curHP = 0;
    private bool canSwitch = false;
    private Vector2 prevStartPos;
    
    private void Start()
    {
        curHP = destroyableObject.HP;
        prevStartPos = transform.position;
    }

    private void OnSwitchBlockEnd()
    {
        canSwitch = true;
    }

    private void OnEnable()
    {
        switchDelayTimer.OnTimerEnd += OnSwitchBlockEnd;
    }

    private void OnDisable()
    {
        switchDelayTimer.OnTimerEnd -= OnSwitchBlockEnd;
    }

    private float curAnimX = 0f;
    private void Update()
    {
        curAnimX += Time.deltaTime * speed;
        curAnimX = Mathf.Clamp01(curAnimX);
        if (animController)
        {
            if (curAnimX >= 1f) animController.PullAnimation(idleAnim, idleAnimTime);
            else animController.PullAnimation(flyAnim, flyAnimTime);
        }
        transform.position = GetCurPos(prevStartPos, CurPoint.position, curAnimX, flyHeight);
        
        if (curHP == destroyableObject.HP) return;
        if (curHP < destroyableObject.HP)
        {
            curHP = destroyableObject.HP;
            return;
        }
        curHP = destroyableObject.HP;
        
        Switch();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Controller>() && agr)
        {
            Switch();
        }
    }

    private void Switch()
    {
        if (!canSwitch) return;
        curAnimX = 0f;
        agr = true;
        canSwitch = false;;
        switchDelayTimer.StartTimer();
        prevStartPos = CurPoint.position;
        Controller.Switch(this);
    }
}
