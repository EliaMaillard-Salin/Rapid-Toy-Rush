using System;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject target;
    
    [Header("Settings")]
    [SerializeField] private float speed = 1;
    [SerializeField] private float distance = 1;
    
    private Vector3 _dir;

    private void Start()
    {
        _dir = (target.transform.position - transform.position).normalized;
    }

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position - _dir * distance, speed * Time.deltaTime);
    }
}
