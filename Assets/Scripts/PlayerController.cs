using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private List<Level> levels;

    [SerializeField] private int startLevel;
    private Level _currentLevel;
    
    private void Start()
    {
        _currentLevel = levels[startLevel];
    }
    public float speed = 5.0f;

    void Update()
    {
        GetKeyboardInput();

        var playerTransform = transform;
        var playerPosition = playerTransform.position;
        
        playerPosition += Vector3.right * 1 * speed * Time.deltaTime;
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
                _currentLevel = temp;
            }
        }
    }
    public void UpperBoundReached()
    {
        print("Upper bound reached");
    }
    public void LowerBoundReached()
    {
        print("Lower bound reached");
    }

    private void OnDrawGizmos()
    {
        if (levels != null)
        {
            Gizmos.color = Color.yellow;

            foreach (var level in levels)
            {
                Gizmos.DrawLine(new Vector3(0,level.Height,0),new Vector3(1000,level.Height,0));
            }
        }
    }
}

[Serializable]
internal class Level
{
    //public int Order;
    public float Height;
}


