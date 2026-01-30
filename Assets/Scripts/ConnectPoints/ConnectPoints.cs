using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConnectPoints : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private GameObject nextLevel, gameOver;
    [SerializeField] private Timer scriptTimer;
    [SerializeField] private int amountConnecting;
    private float cellSize;
    private GameObject currentCell;
    private bool startDrag = false;
    private LineRenderer line;
    private Dictionary<string, string> colorDictionary = new()
    {
        {"yellow", "#ECF84D" },
        {"blue", "#587AFF" },
        {"red", "#FF5858" },
        {"orange", "#FF8800" },
    };

    void Start()
    {
        scriptTimer.SetTimer(40f);
        scriptTimer.Pause(false);
        cellSize = gameObject.GetComponent<GridLayoutGroup>().cellSize.x;
    }

    void Update()
    {
        if (scriptTimer.TimeExpired)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Обрабатывает нажатие
    /// </summary>
    /// <param name="eventData">Событие</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject cell = GetCell(eventData.position);
        Point scriptCell = cell.GetComponent<Point>();
        if (scriptCell.Color != "" && !scriptCell.IsConnected && cell.tag != "Finish")
        {
            startDrag = true;
            currentCell = cell;
            if (scriptCell.NextCell)
            {
                DeleteLine(cell);
            }
            StartLine(cell);
        }
    }

    /// <summary>
    /// Обрабатывает перемещении при нажатии
    /// </summary>
    /// <param name="eventData">Событие</param>
    public void OnDrag(PointerEventData eventData)
    {
        if (startDrag)
        {
            GameObject cell = GetCell(eventData.position);
            if (cell && currentCell)
            {
                Point scriptPoint = cell.GetComponent<Point>();
                Vector2 differencePosition = currentCell.transform.localPosition - cell.transform.localPosition;
                int x = Mathf.Abs(Mathf.RoundToInt(differencePosition.x));
                int y = Mathf.Abs(Mathf.RoundToInt(differencePosition.y));
                if ((x == cellSize && y == 0) || (x == 0 && y == cellSize))
                {
                    if (currentCell != cell && cell != currentCell.GetComponent<Point>().PrevCell && (scriptPoint.Color == "" || (cell.tag == "Finish" && scriptPoint.Color == currentCell.GetComponent<Point>().Color)))
                    {
                        line.SetPosition(1, cell.transform.position);
                        currentCell.GetComponent<Point>().NextCell = cell;
                        scriptPoint.PrevCell = currentCell;
                        scriptPoint.Color = currentCell.GetComponent<Point>().Color;
                        currentCell = cell;
                        StartLine(cell);
                        if (cell.tag == "Finish")
                        {
                            amountConnecting--;
                            ConnectLine(cell);
                            startDrag = false;
                            if (amountConnecting == 0)
                            {
                                NextLevel();
                            }
                        }
                    } else if (cell == currentCell.GetComponent<Point>().PrevCell)
                    {
                        currentCell = cell;
                        DeleteLine(cell);
                    } else if (currentCell != cell && cell != currentCell.GetComponent<Point>().PrevCell && scriptPoint.Color != "" && cell.tag is not "Start" and not "Finish")
                    {
                        GameOver();
                    }
                }
            }
        }
    }


    /// <summary>
    /// Срабатывает при отпускании нажатия
    /// </summary>
    /// <param name="eventData">Событие</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        startDrag = false;
    }


    /// <summary>
    /// Создает линию
    /// </summary>
    /// <param name="cell">Ячейка</param>
    void StartLine(GameObject cell)
    {
        line = cell.GetComponent<LineRenderer>();
        if (!line)
        {
            line = cell.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startWidth = 0.2f;
            line.endWidth = 0.2f;
            line.startColor = Utility.FromHexInColor(colorDictionary[cell.GetComponent<Point>().Color]);
            line.endColor = Utility.FromHexInColor(colorDictionary[cell.GetComponent<Point>().Color]);
            line.endWidth = 0.2f;
            line.sortingOrder = 2;
            line.numCapVertices = 5;
            line.positionCount = 2;
            line.SetPosition(0, cell.transform.position);
            line.SetPosition(1, cell.transform.position);
        }
    }


    /// <summary>
    /// Защелкнуть линию
    /// </summary>
    /// <param name="cell">Ячейка</param>
    void ConnectLine(GameObject cell)
    {
        Point scriptCell = cell.GetComponent<Point>();
        scriptCell.IsConnected = true;
        if (scriptCell.PrevCell)
        {
            ConnectLine(scriptCell.PrevCell);
        }
    }


    /// <summary>
    /// Удаление линии
    /// </summary>
    /// <param name="cell">Ячейка</param>
    void DeleteLine(GameObject cell)
    {
        GameObject prevCell = cell.GetComponent<Point>().PrevCell;
        GameObject nextCell = cell.GetComponent<Point>().NextCell;
        if (nextCell)
        {
            DeleteLine(nextCell);
        } else
        {
            LineRenderer lineRenderer = cell.GetComponent<LineRenderer>();
            if (cell != currentCell)
            {
                prevCell.GetComponent<Point>().NextCell = null;
                cell.GetComponent<Point>().PrevCell = null;
                cell.GetComponent<Point>().Color = "";

                Destroy(lineRenderer);
                DeleteLine(prevCell);
            } else
            {
                line = lineRenderer;
                line.SetPosition(1, cell.transform.position);
            }
        }
    }

    /// <summary>
    /// Находит ячейку
    /// </summary>
    /// <param name="screenPosition">Позиция на экране</param>
    /// <returns>Возвращает ячейку</returns>
    GameObject GetCell(Vector2 screenPosition)
    {
        PointerEventData pointerData = new(EventSystem.current)
        {
            position = screenPosition,
            pointerId = -1,
        };

        List<RaycastResult> raycastResult = new();
        EventSystem.current.RaycastAll(pointerData, raycastResult);

        foreach(RaycastResult result in raycastResult)
        {
            if (result.gameObject.tag is "Point" or "Start" or "Finish")
            {
                return result.gameObject;
            }
        }
        return null;
    }


    /// <summary>
    /// Следующий уровень
    /// </summary>
    /// <returns></returns>
    void NextLevel()
    {
        scriptTimer.Pause(true);
        GameManager.level++;
        gameObject.SetActive(false);
        nextLevel.SetActive(true);
    }

    /// <summary>
    /// Поражение
    /// </summary>
    void GameOver()
    {
        scriptTimer.Pause(true);
        gameObject.SetActive(false);
        gameOver.SetActive(true);
    }
}
