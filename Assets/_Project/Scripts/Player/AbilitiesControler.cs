using System.Collections.Generic;
using UnityEngine;

public class AbilitiesControler : MonoBehaviour
{
    
    [SerializeField] private List<Ability> abilities;

    private void Update()
    {
        int finalJumps = 0;
        
        float finalAc = 0f;
        float finalSpeed = 0f;
        float finalDec = 0f;
        
        foreach (Ability ability in abilities)
        {
            
        }
    }
}
