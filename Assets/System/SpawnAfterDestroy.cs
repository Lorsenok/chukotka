using UnityEngine;

public class SpawnAfterDestroy : MonoBehaviour
{
    [SerializeField] private GameObject[] spawn;
    [SerializeField] private bool spawnOnThisPosition = true;

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        foreach (GameObject go in spawn)
        {
            Instantiate(go, spawnOnThisPosition ? transform.position : go.transform.position, go.transform.rotation);
        }
    }
}
