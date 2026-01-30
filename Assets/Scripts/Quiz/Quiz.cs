using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Quiz : MonoBehaviour
{
    /// <summary>
    /// Слово
    /// </summary>
    /// /// <param name="word">Правильное слово</param>
    /// /// <param name="letters">Буквы</param>
    /// /// <param name="answer">Предложение после ответа</param>
    [System.Serializable]
    public class Word
    {
        public string word;
        public string[] letters;
        public string answer;
    }

    [SerializeField] private InputField field;
    [SerializeField] private GameObject[] letters;
    [SerializeField] private Image[] fragments;
    [SerializeField] private GameObject prefabLetter;
    [SerializeField] private Button eraseButton, sendButton;
    [SerializeField] private Word word;
    [SerializeField] private Timer scriptTimer;
    [SerializeField] private GameObject nextLevel;
    private List<Button> pressedLetters = new();
    private int attempts = 3;
    void Start()
    {
        scriptTimer.SetTimer(40f);
        Utility.ShuffleArray(word.letters);
        for (int i = 0; i < word.letters.Length; i++)
        {
            GameObject letter;
            if (i < 9)
            {
                letter = Instantiate(prefabLetter, letters[0].transform);
            } else
            {
                letter = Instantiate(prefabLetter, letters[1].transform);
            }
            letter.GetComponentInChildren<Text>().text = word.letters[i];
            letter.GetComponent<Button>().onClick.AddListener(() => WriteField(letter.GetComponent<Button>()));
        }
        sendButton.onClick.AddListener(() => Send());
        eraseButton.onClick.AddListener(() => Erase());
    }

    void Update()
    {
        if (scriptTimer.TimeExpired)
        {
            StartCoroutine(GameOver());
        }
    }


    /// <summary>
    /// Написать букву в поле
    /// </summary>
    /// <param name="letter">Кнопка с буквой</param>
    void WriteField(Button letter)
    {
        field.text += letter.GetComponentInChildren<Text>().text;
        letter.interactable = false;
        pressedLetters.Add(letter);
    }

    /// <summary>
    /// Проверить ответ
    /// </summary>
    void Send()
    {
        if (field.text.ToLower() == word.word.ToLower())
        {
            StartCoroutine(NextLevel());
        } else
        {
            attempts--;
            StartCoroutine(Utility.ChangeTransparency(fragments[attempts], 0f, 0.5f));
            if (attempts == 0)
            {
                StartCoroutine(GameOver());
            }
        }
    }

    /// <summary>
    /// Стереть букву
    /// </summary>
    void Erase()
    {
        Button letter = pressedLetters[pressedLetters.Count - 1];
        pressedLetters.RemoveAt(pressedLetters.Count - 1);
        field.text = field.text.Remove(field.text.Length - 1);
        letter.interactable = true;
    }

    /// <summary>
    /// Победа
    /// </summary>
    /// <returns></returns>
    IEnumerator NextLevel()
    {
        eraseButton.interactable = false;
        sendButton.interactable = false;
        foreach(Image fragment in fragments)
        {
            StartCoroutine(Utility.ChangeTransparency(fragment, 0f, 0.5f));
        }
        field.text = word.answer;
        scriptTimer.SetTimer(4f);
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
        nextLevel.SetActive(true);
    }

    /// <summary>
    /// Проигрыш
    /// </summary>
    /// <returns></returns>
    IEnumerator GameOver()
    {
        eraseButton.interactable = false;
        sendButton.interactable = false;
        foreach (Image fragment in fragments)
        {
            StartCoroutine(Utility.ChangeTransparency(fragment, 0f, 0.5f));
        }
        field.text = word.answer;
        scriptTimer.SetTimer(4f);
        yield return new WaitForSeconds(4f);
        if (gameObject.name == "Level3") 
        { 
            SceneManager.LoadScene("MainMenu");
        } else
        {
            gameObject.SetActive(false);
            nextLevel.SetActive(true);
        }
    }
}
