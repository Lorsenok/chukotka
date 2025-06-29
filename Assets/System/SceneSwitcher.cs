using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float speed = 0.7f;

    private float opacityTarget = 0;
    private string newSceneIndexName;

    private void OnEnable()
    {
        SceneChangerService.OnSceneChange += ChangeScene;
    }

    private void OnDisable()
    {
        SceneChangerService.OnSceneChange -= ChangeScene;
    }

    public void ChangeScene(string index)
    {
        newSceneIndexName = index;
        opacityTarget = 1;
    }

    private void Update()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, opacityTarget, speed * Time.deltaTime));

        if (image.color.a >= 1)
        {
            opacityTarget = 0;
            SceneManager.LoadScene(newSceneIndexName);
        }
    }
}
