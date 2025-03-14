using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomBlinking : MonoBehaviour
{
    public Animator anim;
    public float minBlinkTime, maxBlinkTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(blinking());
    }

    IEnumerator blinking()
    {
        while (true)
        {
            anim.SetTrigger("Blinking");
            yield return new WaitForSeconds(1);
        }
    }
}
