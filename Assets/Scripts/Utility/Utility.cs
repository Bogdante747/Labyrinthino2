using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utility : MonoBehaviour
{
    /// <summary>
    /// Перемешать массив
    /// </summary>
    /// <typeparam name="T">Любые данные</typeparam>
    /// <param name="array">Любой массив</param>
    public static void ShuffleArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            T temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    /// <summary>
    /// Изменить прозрачность изображения
    /// </summary>
    /// <param name="image">Изображение</param>
    /// <param name="alpha">Прозрачность</param>
    /// <param name="animationTime">Время анимации</param>
    public static IEnumerator ChangeTransparency(Image image, float alpha, float animationTime)
    {
        float timer = 0;
        Color color = image.color;
        float startAlpha = color.a;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Lerp(startAlpha, alpha, timer / animationTime);
            color.a = progress;
            image.color = color;
            yield return null;
        }
        color.a = alpha;
        image.color = color;
    }



    /// <summary>
    /// Из Hex в Color
    /// </summary>
    /// <param name="hex">hex</param>
    /// <returns>Color</returns>
    public static Color FromHexInColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color)) 
        {
            return color;
        }

        return Color.white;
    }
}
