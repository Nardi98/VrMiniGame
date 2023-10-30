using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ResourcesUI : MonoBehaviour
{
    public TMP_Text _stone;
    public TMP_Text _wood;
    public TMP_Text _food;

    [Range(1f, 10f)]
    public float _updateTime = 2f;
    private float _passedTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _stone.text = Globals.GAME_RESOURCE["stone"].Amout.ToString();
        _wood.text = Globals.GAME_RESOURCE["wood"].Amout.ToString();
        _food.text = Globals.GAME_RESOURCE["food"].Amout.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _passedTime += Time.deltaTime;

        if(_passedTime > _updateTime)
        {
            _stone.text =  Globals.GAME_RESOURCE["stone"].Amout.ToString();
            _wood.text = Globals.GAME_RESOURCE["wood"].Amout.ToString();
            _food.text = Globals.GAME_RESOURCE["food"].Amout.ToString();
            _passedTime = 0f;
        }
    }
}
