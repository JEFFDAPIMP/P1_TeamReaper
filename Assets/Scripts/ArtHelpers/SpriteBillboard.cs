using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] private bool freezeXZAxis = true;
    [SerializeField] private float x = 1.0f;
    [SerializeField] private float y = 1.0f;
    [SerializeField] private float z = 1.0f;
    private Quaternion offset = new Quaternion();

    [SerializeField] private bool useParentObjectRotationZ = false;
    private GameObject parentObject = null;

    public bool test0 = false;
    public bool test1 = false;
    public bool test2 = false;
    public bool test3 = false;
    public bool test4 = false;
    public bool test5 = false;
    public bool test6 = false;
    public bool test7 = false;

    private void Start()
    {
        parentObject = GetComponentInParent<Transform>().gameObject;
    }

    private void Update()
    {
        if (freezeXZAxis)
        {
            transform.rotation = Quaternion.Euler(90f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        }
        else
        {
            if (useParentObjectRotationZ)
            {
                offset.x = x; offset.y = y; offset.z = z;
                //transform.rotation = (Camera.main.transform.rotation * offset);
                //transform.Rotate()
                transform.rotation = Quaternion.Euler(parentObject.transform.rotation.z, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z);
                //transform.Rotate(parentObject.transform.rotation.z, 0f, 0f);
            }
            else if (test0)
            {
                transform.rotation = Camera.main.transform.rotation;
            }
            else if (test1)
            {
                offset.x = x; offset.y = y; offset.z = z;
                transform.rotation = offset;
            }
            else if (test2)
            {
                offset.x = x; offset.y = y; offset.z = z;
                transform.rotation = Camera.main.transform.rotation * parentObject.transform.rotation;
            }
            else
            {
                offset.x = x; offset.y = y; offset.z = z;
                transform.rotation = Camera.main.transform.rotation * offset;
            }
        }
    }
}
