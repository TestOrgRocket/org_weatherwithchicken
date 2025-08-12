using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class LoadingIconRotation : MonoBehaviour
{
    public float speed = 100f;
    void OnEnable()
    {
        StartCoroutine(Rotation());
    }
    IEnumerator Rotation()
    {
        while (true)
        {
            transform.Rotate(0f, 0f, speed * Time.deltaTime);
            yield return null;
        }
    }
}