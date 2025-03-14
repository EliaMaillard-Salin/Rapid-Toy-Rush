using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSystem : MonoBehaviour
{
    [SerializeField] TrackSystem _trackSystem;
    [SerializeField] int _damages;
    [SerializeField] int _life;
    int _lapCount;
    float _carLife;

    public float CarLife { get { return _carLife; } set { _carLife = value; } }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "car")
            return;

        Vector3 carVelocity = GetComponent<CarController>().CurrentLocalVelocity;
        Vector3 otherVelocity = collision.gameObject.GetComponent<CarController>().CurrentLocalVelocity;
        collision.gameObject.GetComponent<CarSystem>().CarLife -= Vector3.Dot(otherVelocity, carVelocity) * 5;
    }
}
