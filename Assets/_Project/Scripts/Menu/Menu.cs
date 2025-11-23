using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Menu : MonoBehaviour 
{ 
    public string MenuName; 
    public bool Open; 
    [SerializeField] private float speed = 5f; 
    [SerializeField] private Vector3 openPosition; 
    [SerializeField] private Vector3 closePosition; 
     private Coroutine moveCoroutine; 
    public void ChangeState(bool state) 
    { 
        Open = state;  
        if (Open) Time.timeScale = 0f; 
        else Time.timeScale = 1f;  
        if (moveCoroutine != null) StopCoroutine(moveCoroutine); 
        moveCoroutine = StartCoroutine(MoveMenu(Open ? openPosition : closePosition)); 
    } 
    private IEnumerator MoveMenu(Vector3 targetPosition) 
    { 
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f) 
        { 
            transform.localPosition = Vector3.Lerp( transform.localPosition, targetPosition, Time.unscaledDeltaTime * speed); 
            yield return null; 
        } 
        transform.localPosition = targetPosition; 
    } 
}