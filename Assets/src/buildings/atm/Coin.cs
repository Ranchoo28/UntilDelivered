using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 360f;
    public float movementHeight = 5f;   
    public float movementTime = 2f;     

    private Vector3 startPosition;
    private float elapsedTime = 0f;

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(destroyer());
    }

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        elapsedTime += Time.deltaTime;
        float verticalMovement = Mathf.PingPong(elapsedTime / movementTime, 1f) * movementHeight;
        transform.position = new Vector3(startPosition.x, startPosition.y + verticalMovement, startPosition.z);
    }

    private IEnumerator destroyer()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

}
