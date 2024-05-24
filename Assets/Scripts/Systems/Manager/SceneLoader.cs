using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private Manager _manager;
    [SerializeField] private Slider slider;
    [SerializeField] private RectTransform loadScreen;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("Manager"))
            _manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    public void LoadNewScene(string scene)
    {
        StartCoroutine(SceneEnumerator(scene));
    }

    public void LoadDestroy(string scene, float waitTime, List<GameObject> destroyManually)
    {
        StartCoroutine(TitleEnumerator(scene, waitTime, destroyManually));
    }

    private IEnumerator TitleEnumerator(string scene, float wait, List<GameObject> destroyManually)
    {
        PlayerPrefs.DeleteAll();
        yield return new WaitForSeconds(wait);
        foreach (var obj in destroyManually)
        {
            Destroy(obj);
        }
        SceneManager.LoadScene(scene);
    }

    private IEnumerator SceneEnumerator(string scene)
    {
        if(_manager)
            _manager.SetControl(false);
        loadScreen.gameObject.SetActive(true);
        slider.value = 0;
        animator.ResetTrigger("In");
        animator.ResetTrigger("Out");
        animator.SetTrigger("In");
        
        yield return new WaitForSeconds(0.5f);
        
        AsyncOperation load = SceneManager.LoadSceneAsync(scene);
        while (!load.isDone)
        {
             slider.value = Mathf.Clamp01(load.progress / 0.9f);
             yield return null;
        }
        if (!_manager && GameObject.Find("Manager"))
        {
            _manager = GameObject.Find("Manager").GetComponent<Manager>();
            _manager.SetControl(false);
        }
        yield return new WaitForSeconds(1);
        
        animator.SetTrigger("Out");
        
        yield return new WaitForSeconds(0.5f);
        loadScreen.gameObject.SetActive(false);
        
        if(_manager)
            _manager.SetControl(true);
        Time.timeScale = 1;
    }
}
