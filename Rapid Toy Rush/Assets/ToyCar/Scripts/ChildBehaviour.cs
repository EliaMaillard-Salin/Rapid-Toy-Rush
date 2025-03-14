using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Windows;

public class ChildBehaviour : MonoBehaviour
{
    [SerializeField] Vector2[] _PossiblePositions;
    [SerializeField] GameObject _hand;
    [SerializeField] GameObject _blurPlane;
    [SerializeField] Animator _anim;
    [SerializeField] Animator _handAnimator;
    [SerializeField] float _interval;
    float time;

    int _state; //Idle, Cry, Blink, Stomp
    int _stateAmount = 4; 
    // Start is called before the first frame update
    void Start()
    {
        _state = 0;
        _blurPlane.SetActive(false);
        time = 0.0f;
        _hand.GetComponentInChildren<MeshRenderer>().enabled = false;
        _interval = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Debug.Log(_state);
        if(time >= _interval)
        {
            time = time - _interval;
            _state = UnityEngine.Random.Range(1, _stateAmount);
            Act();
        }
    }

    void Act()
    {
        if (_state == 0)
            return;

        else if (_state == 1)
        {
            StartCoroutine(Cry());
        }

        else if (_state == 2)
        {
            Blink();
        }

        else if (_state == 3)
        {
            StartCoroutine(Stomp());
        }
    }

    IEnumerator Cry()
    {

        _blurPlane.SetActive(true);

        yield return new WaitForSeconds(4);

        _blurPlane.SetActive(false);
        _state = 0;

    }

    void Blink()
    {
         _anim.SetTrigger("Blinking");
    }

    IEnumerator Stomp()
    {
        _hand.GetComponent<MeshRenderer>().enabled = true;
        int index = UnityEngine.Random.Range(0, _PossiblePositions.Length + 1);
        _hand.transform.position.Set(_PossiblePositions[index].x, 0, _PossiblePositions[index].y );
        _handAnimator.SetTrigger("Stomp");
        yield return new WaitForSeconds(0.5f);
        _hand.GetComponent<MeshRenderer>().enabled = false;
    }


}
