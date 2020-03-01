using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class HomeManager : MonoBehaviour {
    public TMP_Dropdown shipDB;
    public TMP_Dropdown planetDB;
    public TextMeshProUGUI shipInformations;
    public TextMeshProUGUI wellcomeUser;
    //---------------------------------
    public static string UserName = "Luke";
    public Ship[] ships02 = new Ship[10];
    List<Ship> ships = new List<Ship> ();
    public Planet[] planets = new Planet[10];
    string[] shipsName = new string[10];
    float[] shipCapacity;
    string[] planetName = new string[10];
    public GameObject loadingPanel;
    public GameObject notification;
    public GameObject estimateButton;

    // Start is called before the first frame update
    void Start () {
        wellcomeUser.text = "Welcome " + UserName + "!";
        print ("Start");

        GetPlanetAndStarShipsInformations();
    }

     void GetPlanetAndStarShipsInformations(){
         StartCoroutine (checkInternetConnection ((isConnected) => {
            // handle connection status here
            if(isConnected){
                StartCoroutine (WaitForGetShips ());
            }else{
                Notification ("No internet connection.");
                loadingPanel.SetActive (false);
                StartCoroutine (RetryConection ());
            }
        }));
    }

    void RetryGetPlanetAndStarShipsInformations(){
         StartCoroutine (checkInternetConnection ((isConnected) => {
            // handle connection status here
            if(isConnected){
                loadingPanel.SetActive (true);
                StartCoroutine (WaitForGetShips ());
            }else{
                StartCoroutine (RetryConection ());
            }
        }));
    }

    IEnumerator RetryConection(){
        yield return new WaitForSeconds(5f);
        RetryGetPlanetAndStarShipsInformations();
    }

    IEnumerator checkInternetConnection (Action<bool> action) {
        WWW www = new WWW ("https://google.com");
        yield return www;
        if (www.error != null) {
            action (false);
        } else {
            action (true);
        }
    }

   

    IEnumerator WaitForGetShips () {
        string url = "https://api-horizonofstars.azurewebsites.net/API-HorizonOfStars.asmx/GetAllStarships"; //endPoint
        using (UnityWebRequest www = UnityWebRequest.Get (url)) {
            yield return www.SendWebRequest ();
            if (www.isNetworkError || www.isHttpError) {
                Debug.Log (www.error);
                Notification ("Sorry :( Error ocurred:" + www.error);
                loadingPanel.SetActive (false);
            } else {
                Debug.Log ("Complete!");
                string responseText = www.downloadHandler.text;
                XmlDocument doc = new XmlDocument ();
                doc.LoadXml (responseText);
                Debug.Log (doc.InnerText.ToString ());
                FillShips (doc.InnerText.ToString ());
            }
        }
    }

    void FillShips (string shipData) {
        string[] list;
        list = shipData.Split ("&" [0]);

        for (int i = 0; i < list.Length; i++) {

            if (list[i] != " ") {
                Ship _ship = new Ship ();

                string[] newlist;

                newlist = list[i].Split ("|" [0]);

                _ship._name = newlist[0].ToString ().Trim ();
                _ship.capacity = float.Parse (newlist[1].ToString ().Trim ());

                ships.Add (_ship);
                ships02[i] = _ship;
            }
        }

        for (int i = 0; i < shipsName.Length; i++) {
            shipsName[i] = ships02[i]._name;
        }

        foreach (string name in shipsName) {
            shipDB.options.Add (new TMP_Dropdown.OptionData () { text = name });
        }

        StartCoroutine (WaitForGetPlanets ());
    }

    IEnumerator WaitForGetPlanets () {
        string url = "https://api-horizonofstars.azurewebsites.net/API-HorizonOfStars.asmx/GetAllPlanets"; //endPoint
        using (UnityWebRequest www = UnityWebRequest.Get (url)) {
            yield return www.SendWebRequest ();
            if (www.isNetworkError || www.isHttpError) {
                Debug.Log (www.error);
                Notification ("Sorry :( Error ocurred:" + www.error);
                loadingPanel.SetActive (false);
            } else {
                Debug.Log ("Complete!");
                string responseText = www.downloadHandler.text;
                XmlDocument doc = new XmlDocument ();
                doc.LoadXml (responseText);
                Debug.Log (doc.InnerText.ToString ());
                FillPlanets (doc.InnerText.ToString ());
            }
        }
    }

    void FillPlanets (string planetData) {
        string[] list;
        list = planetData.Split ("&" [0]);

        for (int i = 0; i < list.Length; i++) {

            if (list[i] != " ") {
                Planet _planets = new Planet ();

                string[] newlist;

                newlist = list[i].Split ("|" [0]);

                _planets._name = newlist[0].ToString ().Trim ();
                _planets.distance = float.Parse (newlist[1].ToString ().Trim ());

                planets[i] = _planets;
            }
        }

        for (int i = 0; i < planetName.Length; i++) {
            planetName[i] = planets[i]._name;
        }

        //planetDB.options.Clear ();
        foreach (string name in planetName) {
            planetDB.options.Add (new TMP_Dropdown.OptionData () { text = name });
        }

        loadingPanel.SetActive (false);
    }

    public void UpdateShipInformations (string shipname, string capcity, string resupply) {
        shipInformations.text = "<b>Starship:</b> " + shipname + "\n<b>Capacity:</b> " + capcity + "\n<b>Resupply:</b> " + resupply;
        loadingPanel.SetActive (false);
        StartCoroutine (WaitForRequestInsertBI (shipname, capcity, resupply));
    }

    public void Resupply () {
        print ("Resupply Start");
        string _shipName = shipDB.options[shipDB.value].text;
        string _planetName = planetDB.options[planetDB.value].text;

        bool ship = false;
        if (_shipName == "Starship") {
            ship = true;
        }

        bool planet = false;
        if (_planetName == "Planet") {
            planet = true;
        }

        if (ship && planet) {
            Notification ("Please select a starship and planet.");
            return;
        } else if (ship) {
            Notification ("Please select a starship.");
            return;
        } else if (planet) {
            Notification ("Please select a planet.");
            return;
        }

        loadingPanel.SetActive (true);

        Ship _ship = new Ship ();

        for (int i = 0; i < ships02.Length; i++) {
            if (ships[i]._name == _shipName) {
                _ship = ships[i];
                break;
            }
        }

        Planet _planet = new Planet ();

        for (int i = 0; i < planets.Length; i++) {
            if (planets[i]._name == _planetName) {
                _planet = planets[i];
                break;
            }
        }
        StartCoroutine (checkInternetConnection ((isConnected) => {
            // handle connection status here
            if(isConnected){
                StartCoroutine (WaitForRequestGetResupply (_ship, _planet));
            }else{
                Notification ("No internet connection.");
                loadingPanel.SetActive (false);
            }
        }));
    }

    IEnumerator WaitForRequestGetResupply (Ship ship, Planet planet) {

        WWWForm form = new WWWForm ();

        form.AddField ("nuOPPlanet", planet.distance.ToString ()); //distância do planeta
        form.AddField ("nuChargeFullSpaceShip", ship.capacity.ToString ()); //capacidade da nave

        string url = "https://api-horizonofstars.azurewebsites.net/API-HorizonOfStars.asmx/calcStopResupply"; //endPoint

        using (UnityWebRequest www = UnityWebRequest.Post (url, form)) {
            yield return www.SendWebRequest ();
            if (www.isNetworkError || www.isHttpError) {
                Debug.Log (www.error);
                Notification ("Sorry :( Error ocurred:" + www.error);
                loadingPanel.SetActive (false);
            } else {

                Debug.Log ("Complete!");
                string responseText = www.downloadHandler.text;
                XmlDocument doc = new XmlDocument ();
                doc.LoadXml (responseText);
                Debug.Log (doc.InnerText.ToString ());
                UpdateShipInformations (ship._name, ship.capacity.ToString (), doc.InnerText.ToString ());
            }
        }
    }

    public void SelectionStarship (TMP_InputField iF) {
        estimateButton.SetActive (true);
        iF.text = null;

    }
    public void Search (TMP_InputField iF) {

        if (iF.text == null || iF.text == "") {
            print ("Empty fild");
            Notification ("Please enter a starship.");
            return;
        }

        string _planetName = planetDB.options[planetDB.value].text;

        if (_planetName == "Planet") {
            print ("Planet Empty");
            Notification ("The planet is missing.");
            return;
        }

        Planet _planet = new Planet ();

        for (int i = 0; i < planets.Length; i++) {
            if (planets[i]._name == _planetName) {
                _planet = planets[i];
                break; //pode ser removido
            }
        }

        estimateButton.SetActive (false);
        loadingPanel.SetActive (true);
        StartCoroutine (checkInternetConnection ((isConnected) => {
            // handle connection status here
            if(isConnected){
                 StartCoroutine (WaitForRequestGetfinderStarShip (iF.text, _planet.distance.ToString ()));
            }else{
                Notification ("No internet connection.");
                loadingPanel.SetActive (false);
            }
        }));
    }
    IEnumerator WaitForRequestGetfinderStarShip (string ship, string planet) {

        WWWForm form = new WWWForm ();

        form.AddField ("nuOPPlanet", planet); 
        form.AddField ("nmNameSpaceship", ship); 

        string url = "https://api-horizonofstars.azurewebsites.net/API-HorizonOfStars.asmx/GetFilterStarship"; //endPoint

        using (UnityWebRequest www = UnityWebRequest.Post (url, form)) {
            yield return www.SendWebRequest ();
            if (www.isNetworkError || www.isHttpError) {
                Debug.Log (www.error);
                Notification ("Sorry :( Error ocurred:" + www.error);
                loadingPanel.SetActive (false);
            } else {

                Debug.Log ("Complete!");
                string responseText = www.downloadHandler.text;
                XmlDocument doc = new XmlDocument ();
                doc.LoadXml (responseText);
                Debug.Log (doc.InnerText.ToString ());
                if (doc.InnerText.ToString () == "Not is spaceship with this especification") {
                    print ("Not is spaceship with this especification");
                    Notification ("We didn't find this starship");
                    loadingPanel.SetActive (false);
                } else {

                    string data = doc.InnerText.ToString ();
                    string[] list;
                    list = data.Split ("|" [0]);

                    for (int i = 0; i < list.Length; i++) {

                        list[i] = list[i].ToString ().Trim ();
                    }

                    UpdateShipInformations (list[0], list[1], list[2]);
                }
            }
        }
    }

    IEnumerator WaitForRequestInsertBI (string shipName, string shipCapacity, string resu) {

        WWWForm form = new WWWForm ();

        form.AddField ("nmName", UserName);
        form.AddField ("nmStarship", shipName);
        form.AddField ("nmCapacity", shipCapacity);
        form.AddField ("nmResupply", resu);

        string url = "https://api-horizonofstars.azurewebsites.net/API-HorizonOfStars.asmx/insertBI"; //endPoint

        using (UnityWebRequest www = UnityWebRequest.Post (url, form)) {
            yield return www.SendWebRequest ();
            if (www.isNetworkError || www.isHttpError) {
                Debug.Log (www.error);
            } else {
                Debug.Log ("Insert BI complete!");
            }
        }
    }

    void Notification (string text) {
        notification.GetComponentInChildren<TextMeshProUGUI> ().text = text;
        notification.GetComponent<Animator> ().SetTrigger ("show");
    }
}