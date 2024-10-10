using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamController : MonoBehaviour
{

    [SerializeField] private Transform target;

    private Vector3 offset;

    private float followSpeed;

    private void Awake()
    {
        offset = new Vector3(40, 9, -65);
    }
  
    private void Update()
    {
        followSpeed = 5f * Time.deltaTime; 

        Vector3 targetPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed);

    }


}
