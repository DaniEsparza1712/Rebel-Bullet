using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private string exitName;

    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("LastExit") == exitName)
        {
            _player = GameObject.Find("Sam");
            if (_player.GetComponent<CharacterController>().enabled)
            {
                _player.GetComponent<CharacterController>().enabled = false;
                _player.transform.position = transform.position;
                _player.transform.forward = transform.forward;
                _player.GetComponent<CharacterController>().enabled = true;
                
            }
            else
            {
                _player.transform.position = transform.position;
                _player.transform.forward = transform.forward;
            }
            
        }
    }
}
