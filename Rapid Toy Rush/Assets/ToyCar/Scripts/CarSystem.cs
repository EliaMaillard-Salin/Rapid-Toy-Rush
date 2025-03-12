using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSystem : MonoBehaviour
{
    [SerializeField] TrackSystem _trackSystem;
    int _lapCount;
    public int LapCount {  get { return _lapCount; } set { _lapCount = value; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //win
        if (_lapCount == _trackSystem.LapAmount)
            return;
    }
}
