using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform playerCamera = null;

    [SerializeField] public GameObject placeableObjectPrefab = null;
    [SerializeField] private KeyCode buildModeHotkey = KeyCode.B;
    [SerializeField] private KeyCode rotateKeyCode = KeyCode.R;

    private bool buildMode = false;
    private float mouseWheelRotation;
    private bool canPlace = false;


    private Color tempDefaultObjectColor;
    GameObject currentPlaceableObject;


    void Start()
    {
        // placeHolder = Instantiate(placeableObjectPrefab, new Vector3(5, 0, 5), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        HandleNewObjectHotkey();
        if (currentPlaceableObject != null)
        {
            MoveCurrentPlaceableObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
            CanPlaceObject();
        }
        //Ray targetRay = new Ray(playerCamera.position,playerCamera.rotation * Vector3.forward);
        //Vector3 pt = targetRay.GetPoint(5.0f);

        // placeHolder.transform.position = pt;
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.gameObject.name);
    }
    
        private void CanPlaceObject()
    {
        Renderer placeableObjectRenderer = currentPlaceableObject.GetComponent<Renderer>();
        if (Vector3.Distance(playerCamera.position, currentPlaceableObject.transform.position) > 15.0f)
        {
            canPlace = false;
            placeableObjectRenderer.material.SetColor("_Color", Color.red);
        } else
        {
            canPlace = true;
            placeableObjectRenderer.material.SetColor("_Color", Color.green);
        }


    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            Renderer currentPlaceableObjectRenderer = currentPlaceableObject.GetComponent<Renderer>();
            currentPlaceableObjectRenderer.material.SetColor("_Color", tempDefaultObjectColor);

            currentPlaceableObject.layer = 0;
            
            
            //Rigidbody rb = currentPlaceableObject.AddComponent<Rigidbody>() as Rigidbody;

            currentPlaceableObject = null;
            currentPlaceableObject = Instantiate(placeableObjectPrefab);

            Renderer newPlaceableObjectRenderer = currentPlaceableObject.GetComponent<Renderer>();
            newPlaceableObjectRenderer.material.SetColor("_Color", tempDefaultObjectColor);

        }
    }

    private void RotateFromMouseWheel()
    {
        if (Input.GetKeyDown(rotateKeyCode)) {
            currentPlaceableObject.transform.Rotate(Vector3.up, 90.0f);
        }
    }

    private void MoveCurrentPlaceableObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {

            Vector3 targetPos = hitInfo.point;
            targetPos.y += currentPlaceableObject.transform.localScale.y / 2;

            currentPlaceableObject.transform.position = targetPos;
        }
    }

    private void GetNearbyObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(currentPlaceableObject.transform.position, 5.0f);
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log(hitCollider.name);
        }
    }



    private void HandleNewObjectHotkey()
    {
        if (Input.GetKeyDown(buildModeHotkey))
        {
            if (buildMode == false) {
                currentPlaceableObject = Instantiate(placeableObjectPrefab);

                Renderer placeableObjectRenderer = currentPlaceableObject.GetComponent<Renderer>();
                tempDefaultObjectColor = placeableObjectRenderer.material.GetColor("_Color");
            }

            else
            {
                Destroy(currentPlaceableObject);
            }
            buildMode = !buildMode;
        }
    }

}
