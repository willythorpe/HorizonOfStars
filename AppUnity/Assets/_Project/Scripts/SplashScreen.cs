using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {
    public float time = 2f;
    // Start is called before the first frame update
    void Start () {
        StartCoroutine (SplashTime ());
    }

    // Update is called once per frame
    void Update () {

    }

    IEnumerator SplashTime () {
        yield return new WaitForSeconds(time);
        SceneManager.LoadSceneAsync("InputUserName",LoadSceneMode.Single);
    }
}