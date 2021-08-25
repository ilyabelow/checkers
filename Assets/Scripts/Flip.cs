using UnityEngine;

public class Flip : MonoBehaviour
{
    private bool _flipped;
    private Rigidbody _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_flipped && Input.GetKeyDown(KeyCode.Space))
        {
            FlipBoard();
        }
    }

    private void FlipBoard()
    {
        _rigidBody.isKinematic = false;
        _rigidBody.AddForceAtPosition(Vector3.up * 200, new Vector3(0, 0, -0.4f));
        _rigidBody.AddForceAtPosition(Vector3.left * 200, new Vector3(0, -0.4f, -0.4f));
        _flipped = true;
    }
}