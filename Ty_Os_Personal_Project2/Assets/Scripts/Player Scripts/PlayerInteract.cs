using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public bool holdingBattery;
    public bool canInteract = true;
    private Camera cam;
    private InteractText iT;
    [SerializeField] private GameObject interactBox;
    [SerializeField] private KeyCode interactBtn;
    [SerializeField] private Transform objectGripPos;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        iT = GetComponent<InteractText>();
    }

    // Update is called once per frame
    void Update()
    {
        // find a object to interact with
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;
        bool isHit = Physics.Raycast(ray, out hitInfo, distance, mask);

        // if there is an object
        if (isHit && canInteract) {
            // get the interact script & object
            Interactable objectInteract = hitInfo.collider.GetComponent<Interactable>();
            if (objectInteract != null) {
                // display prompt message
                interactBox.SetActive(true);
                iT.updateText(objectInteract.promptMessage);
                // interact with object
                if (Input.GetKeyDown(interactBtn)) {
                    objectInteract.BaseInteract();
                    if (hitInfo.collider.gameObject.tag == "battery") holdingBattery = true;
                }
            }
        }
        else {
            iT.updateText("");
            interactBox.SetActive(false);
        }
    }
}
