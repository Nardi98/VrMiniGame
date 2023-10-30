using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningBuildingBehaviour : BasicBehaviour
{
    private GameObject _winningCanvas;

    // Start is called before the first frame update
    void Start()
    {
        _winningCanvas = GameObject.FindWithTag("winningCanvas");
       
        
    }

    // Update is called once per frame
   

    protected  override void UpdateBehaviour()
    {
        if(_winningCanvas != null)
        {
            _winningCanvas.GetComponent<Canvas>().enabled = true;

        }
    }


}
