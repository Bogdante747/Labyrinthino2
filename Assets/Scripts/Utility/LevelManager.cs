using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] levels;
    void Start()
    {
        levels[GameManager.level - 1].SetActive(true);
    }

}
