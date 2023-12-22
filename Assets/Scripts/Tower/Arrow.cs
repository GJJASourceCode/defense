using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;

    [SerializeField]
    private Transform targetTransform;

    [HideInInspector]
    public float damage;

    public float MoveSpeed => moveSpeed;

    private void Update()
    {
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position +=
            (targetTransform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
        ;
        Vector2 newPos = targetTransform.transform.position - transform.position;
        float rotZ = Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (Vector3.Distance(targetTransform.position, transform.position) < 1f)
        {
            targetTransform.GetComponent<Mob>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void MoveTo(Transform target)
    {
        targetTransform = target;
    }
}
