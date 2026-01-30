using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    /// <summary>
    /// Загрузить сцену
    /// </summary>
    /// <param name="name">Название сцены</param>
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Выйти из игры
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
