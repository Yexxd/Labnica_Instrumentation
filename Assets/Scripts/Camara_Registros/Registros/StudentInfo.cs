using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentInfo : MonoBehaviour
{
    public TxtController DBRegister;

    public string _studentName;
    public int _studentCode;
    public int _studentAge;
    public int _studentGenre;

    private void Start()
    {
        DBRegister = GameObject.FindGameObjectWithTag("DBRegister").GetComponent<TxtController>();
    }
}
