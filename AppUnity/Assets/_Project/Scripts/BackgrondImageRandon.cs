using UnityEngine;
using UnityEngine.UI;

public class BackgrondImageRandon : MonoBehaviour {
    public Sprite[] images;
    void Start () {
        int number = Random.Range (0, images.Length);
        print("randon "+number);
        GetComponent<Image> ().sprite = images[number];
    }
}