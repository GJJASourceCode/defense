using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] //코드 상시실행
public class IsometricZ : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.y / 100
        );
    }
}
