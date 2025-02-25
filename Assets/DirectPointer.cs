using UnityEngine;

public class DirectPointer : MonoBehaviour
{
    private GameObject pointer;
    private Camera cam;

    private void Awake()
    {
        pointer = Instantiate(Resources.Load<GameObject>("Models/Pointer") as GameObject);
        pointer.name = "Pointer(Instance)";
        cam = Camera.main;
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pointer.SetActive(true);
            GetPosition();
            pointer.transform.LookAt(GetRotation());
        }

        else
        {
            pointer.SetActive(false);
        }
    }

    private Vector3 GetPosition()
    {
        pointer.transform.position = transform.position + (transform.forward * 2.5f);
        return pointer.transform.position;
    }

    private Vector3 GetRotation()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 point = new Vector3();
        if (Physics.Raycast(ray, out hit))
        {
            point = hit.point;
        }
        return point;

    }
}