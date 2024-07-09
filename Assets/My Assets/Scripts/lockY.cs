using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockY : MonoBehaviour
{
    [SerializeField] private GameObject plane;
    [SerializeField] private GameObject _Experiment;
    [SerializeField] private GameObject controller;
    [SerializeField] private GlobalVariables GV;
    [SerializeField] private GameObject _X;
    [SerializeField] private GameObject _Y;
    [SerializeField] private GameObject _Z;

    private bool initialized = false;
    private bool reoriented;
    public bool setOnTable;

    private Vector3 _XAxis;
    private Vector3 _YAxis;
    private Vector3 _ZAxis;

    private void Awake()
    {
        setOnTable = false;
        reoriented = false;
    }

    void Update()
    {
        if (!initialized)
        {
            if (GV != null && GV.r_Controller != null)
            {
                initialized = true;
            }
        }
        else if (!setOnTable)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            this.transform.position = controller.transform.position;

            _YAxis = _Y.transform.position - _X.transform.position;
            _XAxis = _Z.transform.position - _X.transform.position;
        }
        else if (setOnTable && !reoriented)
        {
            reoriented = true;
            Reorient();
        }
    }

    void Reorient()
    {
        _YAxis.Normalize();
        _XAxis.Normalize();
        _ZAxis = Vector3.Cross(_XAxis, _YAxis);
        _ZAxis.Normalize();

        Quaternion newRotation = Quaternion.LookRotation(_YAxis, _ZAxis);
        transform.rotation = newRotation;
    }
}
