using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int level = 1;

    /// <summary>
    /// Установить уровень
    /// </summary>
    /// <param name="lvl">Уровень</param>
    public void SetLevel(int lvl)
    {
        level = lvl;
    }
}
