using System;
using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private bool globalPos;
    private Vector3 startPos;

    private void Start()
    {
        startPos = globalPos ? transform.position : transform.localPosition;
    }

    private void Update()
    {
        if (globalPos) transform.position = startPos;
        else transform.localPosition = startPos;
    }
}


/*
 * _        _
  ( `-.__.-' )
   `-.    .-'
      \  /
       ||
       ||
      //\\
     //  \\
    ||    ||
    ||____||
    ||====||
     \\  //
      \\//
       ||
       ||
       ||
       ||
       ||
       ||
       ||
       ||
       []
 */