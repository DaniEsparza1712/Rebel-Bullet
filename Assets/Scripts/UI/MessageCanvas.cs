using System.Collections;
using UnityEngine;

public class MessageCanvas : MonoBehaviour
{
    public float lifeTime;

    private void OnEnable()
    {
        StartCoroutine(DisappearAfterTime());
    }

    private IEnumerator DisappearAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}
