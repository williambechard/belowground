using UnityEngine;

public class lookAt : MonoBehaviour
{
    public Camera targetCamera;
    public Transform targetTransform;
    // Start is called before the first frame update
    void Start()
    {

        targetCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = Input.mousePosition - targetCamera.WorldToScreenPoint(targetTransform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        targetTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
