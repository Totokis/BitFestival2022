using UnityEngine;

public class PathController : MonoBehaviour
{
    public Transform[] positions;
    
    public float speed = 1.0f;
    
    private int _currentIndex;
    
    private Vector3 _currentPosition;
    private bool _endReached;

    void Start()
    {
        _currentPosition = positions[0].position;
    }

    void Update()
    {
        if(_endReached) return;
        
        transform.position = Vector3.MoveTowards(transform.position, _currentPosition, speed * Time.deltaTime);
        
        if (transform.position == _currentPosition)
        {
            _currentIndex++;
            
            if (_currentIndex == positions.Length)
            {
                End();
                return;
            }
            
            _currentPosition = positions[_currentIndex].position;
        }
    }
    private void End()
    {
        _endReached = true;
        Destroy(this,1f);
    }
}

