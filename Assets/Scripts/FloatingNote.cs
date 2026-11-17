
using UnityEngine;

public class FloatingNote : MonoBehaviour
{
    public float FloatHeight = 0.08f;
    public float FloatSpeed = 2f;

    private Vector3 StartPosition;

    void Start()
    {
        StartPosition = transform.position;
    }

    void Update()
    {
        float movement =
            Mathf.Sin(Time.time * FloatSpeed) * FloatHeight;

        transform.position = StartPosition +
            Vector3.up * movement;
    }
}
