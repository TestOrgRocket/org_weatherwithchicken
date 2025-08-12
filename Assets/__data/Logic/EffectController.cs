using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(Grow());
    }
    IEnumerator Grow()
    {
        transform.localScale = Vector3.zero;
        while (transform.localScale.x < 1f)
        {
            transform.localScale += Vector3.one * Time.deltaTime * 3f;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
