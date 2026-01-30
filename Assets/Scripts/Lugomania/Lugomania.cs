using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lugomania : MonoBehaviour
{
    [SerializeField] private GameObject nextLevel, gameOver;
    [SerializeField] private Timer scriptTimer;
    [SerializeField] private Sprite[] sprites;
    private Button[] cells;
    private float cellSize;
    private Button currentCell, finishCell;
    private int currentColumn, currentRow;
    private Dictionary<(int, int), Button> cellsDictionary = new();

    void Start()
    {
        scriptTimer.SetTimer(40f);
        scriptTimer.Pause(false);
        cells = gameObject.GetComponentsInChildren<Button>();
        cellSize = gameObject.GetComponent<GridLayoutGroup>().cellSize.x;
        int columns = Mathf.FloorToInt(gameObject.GetComponent<RectTransform>().rect.width / cellSize);
        for (int i = 0; i < cells.Length; i++)
        {
            int column = i % columns;
            int row = i / columns;
            if (cells[i].tag == "Start")
            {
                currentCell = cells[i];
                currentColumn = column;
                currentRow = row;
            } else if (cells[i].tag == "Finish")
            {
                finishCell = cells[i];
            }
            Button button = cells[i];
            cells[i].onClick.AddListener(() => Step(button));
            cellsDictionary.Add((column, row), button);
            
        }
    }

    void Update()
    {
        if (scriptTimer.TimeExpired)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Сделать шаг
    /// </summary>
    /// <param name="cell">Ячейка</param>
    void Step(Button cell)
    {
        Vector2 differencePosition = currentCell.transform.localPosition - cell.transform.localPosition;
        int x = Mathf.RoundToInt(differencePosition.x);
        int y = Mathf.RoundToInt(differencePosition.y);
        if ((Mathf.Abs(x) == cellSize && y == 0) || (x == 0 && Mathf.Abs(y) == cellSize) && cell.image.sprite.name == "noFill")
        {
            cell.image.sprite = sprites[1];
            currentCell.image.sprite = sprites[0];
            currentCell = cell;
            if (x == -cellSize)
            {
                currentColumn++;
                cell.transform.Rotate(0, 0, 0);
            } else if (x == cellSize)
            {
                currentColumn--;
                cell.transform.Rotate(0, 0, 180);

            } else if (y == -cellSize)
            {
                currentRow--;
                cell.transform.Rotate(0, 0, 90);
                
            } else if (y == cellSize)
            {
                currentRow++;
                cell.transform.Rotate(0, 0, -90);
            }


            if (!HaveStep())
            {
                Vector2 differencePositionFinish = currentCell.transform.localPosition - finishCell.transform.localPosition;
                int finishX = Mathf.RoundToInt(differencePositionFinish.x);
                int finishY = Mathf.RoundToInt(differencePositionFinish.y);
                if ((Mathf.Abs(finishX) == cellSize && finishY == 0) || (finishX == 0 && Mathf.Abs(finishY) == cellSize))
                {
                    bool isFilled = true;
                    foreach(Button oneCell in cells)
                    {
                        if (oneCell.image.sprite.name == "noFill")
                        {
                            isFilled = false;
                            break;
                        }
                    }
                    if (isFilled)
                    {
                        NextLevel();
                    } else
                    {
                        GameOver();
                    }
                } else
                {
                    GameOver();
                }
            }
        }
        
    }

    /// <summary>
    /// Есть ли шаг
    /// </summary>
    /// <returns></returns>
    bool HaveStep()
    {
        Button cell;
        cellsDictionary.TryGetValue((currentColumn + 1, currentRow), out cell);
        if (cell?.image.sprite.name == "noFill") return true;
        cellsDictionary.TryGetValue((currentColumn - 1, currentRow), out cell);
        if (cell?.image.sprite.name == "noFill") return true;
        cellsDictionary.TryGetValue((currentColumn, currentRow + 1), out cell);
        if (cell?.image.sprite.name == "noFill") return true;
        cellsDictionary.TryGetValue((currentColumn, currentRow - 1), out cell);
        if (cell?.image.sprite.name == "noFill") return true;

        return false;
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
