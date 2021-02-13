﻿using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A script to make an object draggable with the mouse.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Draggable : MonoBehaviour
{
    private Camera _mainCamera;
    private Rigidbody2D _rigidbody2D;
    private bool _dragging;
    public bool IsDragging => _dragging;

    private void Start()
    {
        _mainCamera = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown((int) MouseButton.LeftMouse))
        {
            _dragging = true;
            _rigidbody2D.isKinematic = true;
            transform.rotation = Quaternion.identity;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnMouseUp()
    {
        if (_dragging && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
        {
            _dragging = false;
            _rigidbody2D.isKinematic = false;
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
        }
    }

    private void Update()
    {
        if (_dragging)
        {
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
    }
}