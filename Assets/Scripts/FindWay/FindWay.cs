using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindWay : MonoBehaviour
{
    [SerializeField] private GameObject nextLevel, textWin;
    [SerializeField] private Timer scriptTimer;
    [SerializeField] private Button resetButton;
    [SerializeField] private Image image;
    private Button[] cells;
    private float cellSize;
    private Button currentCell;
    private List<Button> rightWay = new();
    private List<Button> way = new();

    void Start()
    {
        textWin.SetActive(false);
        scriptTimer.gameObject.SetActive(true);
        scriptTimer.SetTimer(3f);
        cells = gameObject.GetComponentsInChildren<Button>();
        cellSize = gameObject.GetComponent<GridLayoutGroup>().cellSize.x;
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].interactable = false;
            if (cells[i].tag == "Start") currentCell = cells[i];
            Button button = cells[i];
            cells[i].onClick.AddListener(() => Step(button));
            Image[] images = cells[i].GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                if (image.name == "complete" && image.color.a > 0)
                {
                    rightWay.Add(cells[i]);
                }
            }
        }
        way.Add(currentCell);
        resetButton.interactable = false;
        resetButton.onClick.AddListener(() => Reset());
        Invoke("Reset", 3f);
        Invoke("OnButton", 3f);
    }

    void Update()
    {
        if (scriptTimer.TimeExpired)
        {
            scriptTimer.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// —делать шаг
    /// </summary>
    /// <param name="cell">ячейка</param>
    void Step(Button cell)
    {
        if (cell.tag == "Obstacle")
        {
            Reset();
        } else
        {

            Vector2 differencePosition = currentCell.transform.localPosition - cell.transform.localPosition;
            int x = Mathf.Abs(Mathf.RoundToInt(differencePosition.x));
            int y = Mathf.Abs(Mathf.RoundToInt(differencePosition.y));
            if ((x == cellSize && y == 0) || (x == 0 && y == cellSize))
            {
                Image[] images = cell.GetComponentsInChildren<Image>();
                foreach(Image image in images)
                {
                    if (image.name == "complete" && image.color.a == 0)
                    {
                        Color color = image.color;
                        color.a = 0.5f;
                        image.color = color;
                        currentCell = cell;
                        way.Add(cell);
                        if (cell.tag == "Finish")
                        {
                            if (CheckWay())
                            {
                                StartCoroutine(NextLevel());
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// —бросить путь
    /// </summary>
    void Reset()
    {
        foreach (Button cell in cells)
        {
            if (cell.tag == "Start")
            {
                currentCell = cell;
            } else
            {
                Image[] images = cell.GetComponentsInChildren<Image>();
                foreach (Image image in images)
                {
                    if (image.name == "complete")
                    {
                        Color color = image.color;
                        color.a = 0f;
                        image.color = color;
                    }
                }
            }
            way.Clear();
            way.Add(currentCell);
        }
    }

    bool CheckWay()
    {
        if (rightWay.Count != way.Count) return false;
        for (int i = 0; i < way.Count; i++)
        {
            if (rightWay.IndexOf(way[i]) == -1) return false;
        }
        return true;
    }


    /// <summary>
    /// ¬ключить все кнопки
    /// </summary>
    void OnButton()
    {
        resetButton.interactable = true;
        foreach (Button cell in cells)
        {
            cell.interactable = true;
        }
    }

    /// <summary>
    /// —ледующий уровень
    /// </summary>
    /// <returns></returns>
    IEnumerator NextLevel()
    {
        StartCoroutine(Animation(1, 0.3f));
        textWin.SetActive(true);
        scriptTimer.SetTimer(4f);
        scriptTimer.gameObject.SetActive(true);
        resetButton.interactable = false;
        foreach (Button cell in cells)
        {
            cell.interactable = false;
        }
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
        nextLevel.SetActive(true);
    }

    IEnumerator Animation(float alpha, float timeAnimation)
    {
        float timer = 0f;
        Color color = image.color;
        float startAlpha = color.a;

        while (timer < timeAnimation)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Lerp(startAlpha, alpha, timer / timeAnimation);
            color.a = progress;
            image.color = color;
            yield return null;
        }
        color.a = alpha;
        image.color = color;
        if (alpha > 0)
        {
            StartCoroutine(Animation(0, timeAnimation*3));
        }
    }
}
