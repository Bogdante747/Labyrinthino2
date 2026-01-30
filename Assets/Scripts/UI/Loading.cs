using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private Text loadingText;
    private float minTimeAnimation = 2.5f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        float progress = Mathf.Lerp(0, 100, timer / minTimeAnimation);
        loadingSlider.value = progress;
        loadingText.text = Mathf.Ceil(progress).ToString("0") + "%";

        if (progress > 50) loadingText.color = Color.white;
        if (progress >= 100) SceneManager.LoadScene("MainMenu");
    }
}
