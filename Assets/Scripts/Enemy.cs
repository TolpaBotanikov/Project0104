using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    private bool _selected;

    public bool Selected 
    {
        get { return _selected; }
        set
        {
            _selected = value;
           if (_selected)
                this.gameObject.GetComponent<Renderer>().
                        material.color = Color.blue;
           else
                this.gameObject.GetComponent<Renderer>().
                        material.color = Color.red;
        }
    }
}
