using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private bool _isPlayerAlive = true;

    void Update()
    {
        
        if (_isPlayerAlive)
        {
            Move();
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * _speed * Time.deltaTime;
        transform.position += movement;
    }

    public void SetPlayerAlive(bool isAlive)
    {
        _isPlayerAlive = isAlive;
    }
}
