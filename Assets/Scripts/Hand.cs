using System.Collections;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _liftSpeed;
    [SerializeField] private float _movingHeight;

    private Rigidbody _selectedChecker;
    private bool _grabbed;
    private bool _isLifting;
    private Vector3 _offset;

    private Coroutine _checkerRotation;
    private Coroutine _checkerLifting;

    private Camera _camera;


    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!_grabbed)
        {
            Raycast();
            if (_selectedChecker != null && Input.GetMouseButtonDown(0))
            {
                GrabChecker();
            }
        }
        else
        {
            if (!_isLifting)
            {
                MoveChecker();
            }

            if (!Input.GetMouseButton(0))
            {
                ReleaseChecker();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                RotateChecker();
            }
        }
    }

    private void RotateChecker()
    {
        _checkerRotation = StartCoroutine(PerformRotation(_selectedChecker.transform));
    }

    private void GrabChecker()
    {
        _grabbed = true;
        _isLifting = true;
        _selectedChecker.isKinematic = true;
        _checkerLifting = StartCoroutine(PerformLift(_selectedChecker.transform));
    }

    private void ReleaseChecker()
    {
        _grabbed = false;
        _selectedChecker.isKinematic = false;
        _isLifting = false;
        if (_checkerRotation != null)
        {
            StopCoroutine(_checkerRotation);
        }

        if (_checkerLifting != null)
        {
            StopCoroutine(_checkerLifting);
        }
    }

    private IEnumerator PerformRotation(Transform checker)
    {
        Quaternion startRotation = checker.rotation;
        Quaternion finalRotation = startRotation * Quaternion.Euler(0, 0, 180);
        float t = 0;
        while (t <= 1)
        {
            checker.rotation = Quaternion.Lerp(startRotation, finalRotation, t);
            t += _rotationSpeed * Time.deltaTime;
            yield return null;
        }

        checker.rotation = finalRotation;
    }

    private IEnumerator PerformLift(Transform checker)
    {        
        var pos = checker.position;
        _offset = GetCursorPosition() - pos;
        _offset.y = 0;
        
        float y = pos.y;
        while (y <= _movingHeight)
        {
            checker.position = new Vector3(pos.x, y, pos.z);
            y += _liftSpeed;
            yield return null;
        }
        checker.position = new Vector3(pos.x, _movingHeight, pos.z);
        _isLifting = false;
    }


    private Vector3 GetCursorPosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        float t = (_movingHeight - ray.origin.y) / ray.direction.y;
        return ray.GetPoint(t);
    }

    private void MoveChecker()
    {
        _selectedChecker.transform.position = GetCursorPosition() - _offset;
    }


    private void Raycast()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Checker")))
        {
            Rigidbody selectedGameObject = hit.rigidbody;
            if (_selectedChecker == null || !ReferenceEquals(selectedGameObject, _selectedChecker))
            {
                if (_selectedChecker != null)
                {
                    _selectedChecker.GetComponent<Outline>().enabled = false;
                }

                _selectedChecker = selectedGameObject;
                _selectedChecker.GetComponent<Outline>().enabled = true;
            }
        }
        else if (_selectedChecker != null)
        {
            _selectedChecker.GetComponent<Outline>().enabled = false;
            _selectedChecker = null;
        }
    }
}