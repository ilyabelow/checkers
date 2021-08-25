using System.Collections;
using UnityEngine;

public class Pan : MonoBehaviour
{
    private Vector3 _lookAt;
    private bool _forWhite = true;

    private bool _rotating;
    [SerializeField] private float _rotationSpeed;


    private void Start()
    {
        var position = transform.position;
        var dir = transform.forward;
        var t = -position.z / dir.z;
        _lookAt = position + dir * t;
    }

    private void Update()
    {
        if (!_rotating && Input.GetMouseButtonDown(2))
        {
            SwitchPlayer();
        }
    }

    private void SwitchPlayer()
    {
        _rotating = true;
        StartCoroutine(PerformRotation());
    }

    private IEnumerator PerformRotation()
    {
        Vector3 position = transform.position;
        float phi = 0;
        float r = Mathf.Sqrt(position.x * position.x + position.z * position.z);
        float t = 0;
        
        while (t < 1)
        {
            if (_forWhite)
            {
                phi = Mathf.Lerp(Mathf.PI, Mathf.PI * 2, t * t * (3 - 2 * t));
            }
            else
            {
                phi = Mathf.Lerp(0, Mathf.PI, t * t * (3 - 2 * t));
            }

            transform.position = new Vector3(r * Mathf.Sin(phi), position.y, r * Mathf.Cos(phi));
            transform.LookAt(_lookAt);
            t += _rotationSpeed * Time.deltaTime;
            yield return null;
        }

        if (_forWhite)
        {
            phi = Mathf.PI * 2;
        }
        else
        {
            phi = Mathf.PI;
        }

        transform.position = new Vector3(r * Mathf.Sin(phi), position.y, r * Mathf.Cos(phi));
        transform.LookAt(_lookAt);

        _rotating = false;
        _forWhite = !_forWhite;
    }
}