using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.SceneManagement;

public class PrefabItem : MonoBehaviour
{

    public Text GroupName;
    public Text GroupID;

    // FirebaseFirestore db;

    // public Button yourButton;

    public void setupPrefab(string gName, string gId){
        GroupName.text = gName;
        GroupID.text = gId;

        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            StaticGidUid.GroupId = gId;
            SceneManager.LoadScene("Members");

        });
    }


    private void LoadGroupMemberScreen(string GroupID){
        // db = FirebaseFirestore.DefaultInstance;
        // getDataRealTime(GroupID);
    }

    // private void getDataRealTime(string GroupID){

    //     Query query = db.Collection("Users").WhereArrayContains("Groups", GroupID);

    //     ListenerRegistration listener = query.Listen(snapshot => {
    //         Debug.Log("Callback received query snapshot.");
            
    //         // clearPrefabArea();

    //         foreach (DocumentSnapshot documentSnapshot in snapshot.Documents) {
    //             Dictionary<string, object> User = documentSnapshot.ToDictionary();
    //             Debug.Log(User["name"]);
    //             Debug.Log(User["email"]);

    //             // GameObject newPrefab = GameObject.Instantiate(Prefab, this.gameObject.transform);

    //             // RectTransform rtrans = newPrefab.GetComponent<RectTransform>();
    //             // rtrans.sizeDelta = new Vector2(0, 25);
                
    //             // newPrefab.transform.position = newPrefab.transform.position + new Vector3(0, -PrefabList.Count*60, 0);

    //             // PrefabItem newPrefabItem = newPrefab.GetComponent<PrefabItem>();
    //             // newPrefabItem.setupPrefab((string)group["name"], (string)documentSnapshot.Id);

    //             // PrefabList.Add(newPrefabItem);
    //         }

    //         // RectTransform rt = this.gameObject.GetComponent<RectTransform>();
    //         // rt.sizeDelta = new Vector2(0, PrefabList.Count*75);

    //     });
    // }



    // Start is called before the first frame update
    void Start () {

	}

    void Awake(){

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
