using UnityEngine;

public class MoveAndDestroy : MonoBehaviour
{
    private float speed;
    private float destroyTime;

    public void SetParameters(float moveSpeed, float lifetime)
    {
        speed = moveSpeed;
        destroyTime = lifetime;
        Destroy(gameObject, destroyTime); // Distrugge l'oggetto dopo il tempo impostato
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Movimento lungo Z
    }
}
