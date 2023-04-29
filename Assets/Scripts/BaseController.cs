using UnityEngine;

public class BaseController : MonoBehaviour
{
    // flag to prevent multiple collisions
    private bool _hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    public float radius = 0.5f;
    public LayerMask layerMask;

    private void Update()
    {
        if (!_hasCollided)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("stamp"))
                {
                    Debug.Log("Collision detected with " + collider.gameObject.name);
                    // Do something when a collision is detected, such as play a sound or spawn a particle effect
                    _hasCollided = true;
                }
            }
        }
    }

    // // collision detection
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log("on trigger 2D");
    //     // check if the collision is with the basketball
    //     if (other.CompareTag("stamp") && !_hasCollided)
    //     {
    //         Debug.Log("collided with stamp");
    //         _hasCollided = true;
    //     }
    // }
}