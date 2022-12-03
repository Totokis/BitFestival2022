using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private List<Level> levels;
    [SerializeField] private int startLevel;
    private Level _currentLevel;


    public UnityEvent<int> levelChanged;
    public UnityEvent<int> upperBoundReached;
    public UnityEvent<int> lowerBoundReached;

    private void Start()
    {
        Time.timeScale = 1f;
        _currentLevel = levels[startLevel];
        StartCoroutine(DifficultyIncreaser());
    }

    public float speed = 5.0f;
    private IEnumerator DifficultyIncreaser()
    {
        yield return new WaitForSeconds(1f);
        speed += 0.057f;
        StartCoroutine(DifficultyIncreaser());
    }

    void Update()
    {
        GetKeyboardInput();

        var playerTransform = transform;
        var playerPosition = playerTransform.position;

        playerPosition += Vector3.right * 1 * speed * Time.deltaTime;
        if (playerTransform.position.y != _currentLevel.Height)
        {
            playerPosition = Vector3.Slerp(new Vector3(playerPosition.x, playerPosition.y) ,new Vector3(playerPosition.x, _currentLevel.Height), 0.2f);
        }
        else
            playerPosition = new Vector3(playerPosition.x, _currentLevel.Height, playerPosition.z);

        playerTransform.position = playerPosition;
    }
    private void GetKeyboardInput()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            var temp = levels.ElementAtOrDefault(levels.IndexOf(_currentLevel) - 1);
            if (temp == null)
            {
                UpperBoundReached();
            }
            else
            {
                LevelChanged();
                _currentLevel = temp;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            var temp = levels.ElementAtOrDefault(levels.IndexOf(_currentLevel) + 1);
            if (temp == null)
            {
                LowerBoundReached();
            }
            else
            {
                LevelChanged();
                _currentLevel = temp;
            }
        }
    }
    public void LevelChanged()
    {
        //print("Level changed");
        levelChanged.Invoke(levels.IndexOf(_currentLevel));
    }
    public void UpperBoundReached()
    {
        //print("Upper bound reached");
        upperBoundReached.Invoke(levels.IndexOf(_currentLevel));
    }
    public void LowerBoundReached()
    {
        //print("Lower bound reached");
        lowerBoundReached.Invoke(levels.IndexOf(_currentLevel));
    }

    //private void OnDrawGizmos()
    //{
    //    if (levels != null)
    //    {
    //        Gizmos.color = Color.yellow;

    //        foreach (var level in levels)
    //        {
    //            Gizmos.DrawLine(new Vector3(0,level.Height,0),new Vector3(1000,level.Height,0));
    //        }
    //    }
    //}
}

[Serializable]
internal class Level
{
    //public int Order;
    public float Height;
}


