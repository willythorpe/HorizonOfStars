using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserNameManager : MonoBehaviour {
    public TMP_InputField userNameInputFild;
    public GameObject notification;

    public void SaveUserName () {

        if (userNameInputFild.text == "" || userNameInputFild.text == null) {
            Notification ("Plese, enter your name.");
            return;
        }

        HomeManager.UserName = userNameInputFild.text;
        SceneManager.LoadScene ("Home", LoadSceneMode.Single);
    }

    void Notification (string text) {
        notification.GetComponentInChildren<TextMeshProUGUI> ().text = text;
        notification.GetComponent<Animator> ().SetTrigger ("show");
    }
}