using UnityEngine;

public class ShovelDig : MonoBehaviour
{
    public float digRange = 3f;
    public Camera playerCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, digRange))
            {
                if (hit.collider.CompareTag("Diggable"))
                {
                    Destroy(hit.collider.gameObject); // MVP: remove block
                    Debug.Log("Dug a block!");
                }
            }
        }
    }
}
