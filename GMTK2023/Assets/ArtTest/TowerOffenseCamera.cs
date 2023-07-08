using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerOffenseCamera : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 1.0f;
    [SerializeField] private float pointRounding = 1.0f;
    [SerializeField] private bool roundToInt = true;

    [SerializeField] private bool allowZoom = true;
    [SerializeField] private float scrollThreshold = 0.5f;
    [SerializeField] private int zoomLevels = 10;
    [SerializeField] private int startingZoom = 10;
    [SerializeField] private float minZoom = 4;
    [SerializeField] private float maxZoom = 32;
    private int _currentZoom;
    
    private Camera _cameraComponent;
    private Vector3 _internalPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _cameraComponent = GetComponent<Camera>();
        
        _internalPosition = transform.position;
        _currentZoom = startingZoom;

        RoundCameraPosition();
        UpdateZoom();
    }

    // Update is called once per frame
    void Update()
    {
        HandleZoom();
        HandleMovement();
        
        RoundCameraPosition();
    }

    private void HandleMovement()
    {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            input += Vector3.forward * cameraSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            input += Vector3.right * -cameraSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input += Vector3.forward * -cameraSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            input += Vector3.right * cameraSpeed;
        }

        Vector3 position = _internalPosition;
        
        Vector3 inputTransform = transform.rotation * input;
        Vector3 inputProjection = Vector3.ProjectOnPlane(inputTransform, Vector3.up).normalized;
        Vector3 movement = inputProjection * ((_currentZoom + 1) * cameraSpeed * Time.deltaTime);
        
        _internalPosition = position + movement;

    }

    private void HandleZoom()
    {
        if (!allowZoom)
            return;
        
        float MouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");
        if (Mathf.Abs(MouseWheel) < scrollThreshold)
            return;

        if (MouseWheel > 0.0f)
            _currentZoom--;
        else if (MouseWheel < 0.0f)
            _currentZoom++;
        
        UpdateZoom();
    }

    private void UpdateZoom()
    {
        if (!allowZoom)
            return;
        
        _currentZoom = Mathf.Clamp(_currentZoom, 0, zoomLevels);

        float step = (maxZoom - minZoom) / zoomLevels;
        
        float size = minZoom + _currentZoom * step;
        size = Mathf.Clamp(size, minZoom, maxZoom);
        
        _cameraComponent.orthographicSize = size;
    }

    private void RoundCameraPosition()
    {
        pointRounding = Mathf.Abs(pointRounding);
        if (pointRounding <= 0.0f)
            return;
        
        Vector3 position = _internalPosition;
        position.x = pointRounding * Mathf.Round(position.x / pointRounding);
        position.y = pointRounding * Mathf.Round(position.y / pointRounding);
        position.z = pointRounding * Mathf.Round(position.z / pointRounding);

        if (roundToInt)
        {
            position.x = Mathf.RoundToInt(position.x);
            position.y = Mathf.RoundToInt(position.y);
            position.z = Mathf.RoundToInt(position.z);
        }

        transform.position = position;
    }
}
