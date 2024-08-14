using UnityEngine;

public class BillBoardTree : MonoBehaviour
{
    private Camera _camera;
    private GameObject parentObject;
    [SerializeField] private bool useStaticBillboard;

    public float x = 90f;
    public float y = 0f;
    public float z = 0f;

    private void Awake()
    {
        _camera = Camera.main;
        parentObject = GetComponentInParent<Transform>().gameObject;
    }

    private void Update()
    {
        if (useStaticBillboard)
        {
            transform.rotation = _camera.transform.rotation;
        }
        else
        {
            transform.LookAt(_camera.transform);
        }

        transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, 0f);


    }
}
