using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject musicSlider;
    [SerializeField] private GameObject soundSlider;
    [SerializeField] private Sprite[] sprites;
    private Button[] musicCells;
    private Button[] soundCells;
    void Start()
    {
        musicCells = musicSlider.GetComponentsInChildren<Button>();
        soundCells = soundSlider.GetComponentsInChildren<Button>();
        for (int i = 0; i < musicCells.Length; i++)
        {
            int index = i;
            musicCells[i].onClick.AddListener(() => ChaneAudio(musicCells, index));
            soundCells[i].onClick.AddListener(() => ChaneAudio(soundCells, index)); 
        }
    }

    /// <summary>
    /// Изменить уровень громкости аудио
    /// </summary>
    /// <param name="buttons">Массив кнопок</param>
    /// <param name="index">Индекс кнопки</param>
    void ChaneAudio(Button[] buttons, int index)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i <= index)
            {
                buttons[i].image.sprite = sprites[1];
            } else
            {
                buttons[i].image.sprite = sprites[0];
            }
        }
    }
}
