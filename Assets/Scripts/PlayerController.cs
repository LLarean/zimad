using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float _cameraSpeed = 1;
    public float _moveSpeed   = 1;
    
    private Camera   _camera;
    private Animator _animator;
    private Vector3  _lastPosition;
    private Vector3  _velosity;
    private Queue<GameObject> _fxToDestory;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        _camera   = Camera.main;
        if (_camera == null)
        {
            Debug.LogError("Main camera not found. Add the camera with MainCamera tag to the scene");
            enabled = false;
        }
    }

    private void Update()
    {
        var newx = Mathf.Lerp(_camera.transform.position.x, this.transform.position.x, Time.deltaTime * _cameraSpeed);
        var newv = new Vector3(newx, _camera.transform.position.y, _camera.transform.position.z);
        _camera.transform.position = newv;

        transform.position += _velosity * _moveSpeed * Time.deltaTime;
    }

    private void LateUpdate()
    {
        _animator.SetBool("Run", _lastPosition != transform.position);
        _lastPosition = transform.position;
    }

    public void MoveBack()
    {
        _velosity = new Vector3(1, 0, 0);
    }

    public void MoveForward()
    {
        _velosity = new Vector3(-1, 0, 0);
    }

    public void StopMoving()
    {
        _velosity = Vector3.zero;
    }

    public void CreateFX(string prefabName)
    {
        var fxPrefab = Resources.Load<GameObject>(prefabName);
        if(fxPrefab == null) Debug.LogErrorFormat("The prefab with name {0} not found in the Resources folder.", prefabName);

        Instantiate(fxPrefab, this.transform.position, Quaternion.identity);
    }
}