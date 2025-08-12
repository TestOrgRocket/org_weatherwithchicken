using System.Collections;
using System.Net;
using Unity.Advertisement.IosSupport;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoading : MonoBehaviour
{
    IEnumerator Start()
    {
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(1);
    }
}