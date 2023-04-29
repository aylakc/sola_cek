using UnityEngine;

public class DraggableController : MonoBehaviour
{
    private Vector3 mousePositionOffset;
    
    public GameObject handTutorial;

    // Start is called before the first frame update
    void Start()
    {
        handTutorial.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }


    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        handTutorial.SetActive(false);
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }
}