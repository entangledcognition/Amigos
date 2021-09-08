using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;

public class MembersPrefabManager : MonoBehaviour
{
    FirebaseFirestore db;
    public GameObject Prefab;
    private string GroupID;
    private string UserID;
    ListenerRegistration dbListener;

    // Start is called before the first frame update
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        GroupID = StaticGidUid.GroupId;
        Debug.Log(GroupID);
        // dbListener = getDataRealTime();
        getDataRealTime();
    }

    private void Awake(){
        
        // if(db==NULL)
        // db = FirebaseFirestore.DefaultInstance;
        // GroupID = StaticGidUid.GroupId;
        // GroupID = "AZpdq80pPmFsCBExNzWq";
        // getDataRealTime();

        // UserID = StaticGidUid.UserId;
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnDestroy(){

    //     dbListener.Stop();
    //     Debug.Log("members Destroyed!");

    // }

    


    List<MembersPrefabItem> PrefabList = new  List<MembersPrefabItem>();

    private void clearPrefabArea(){
        foreach(MembersPrefabItem item in PrefabList)
            {
                Destroy(item.gameObject);
            }
            PrefabList.Clear();
    }
    
    private void getDataRealTime(){
        // Debug.Log("Realtime DATA");

        Query query = db.Collection("Users").WhereArrayContains("Groups", GroupID);

        ListenerRegistration listener = query.Listen(snapshot => {
        // query.GetSnapshotAsync().ContinueWithOnMainThread(task => { 

            // QuerySnapshot snapshot = task.Result;
            Debug.Log("Callback received query snapshot.");
            
            clearPrefabArea();

            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents) {
                Dictionary<string, object> User = documentSnapshot.ToDictionary();
                Debug.Log(User["name"]);
                Debug.Log(User["email"]);

                // if((string)User["id"]==UserID) continue;

                GameObject newPrefab = GameObject.Instantiate(Prefab, this.gameObject.transform);

                RectTransform rtrans = newPrefab.GetComponent<RectTransform>();
                rtrans.sizeDelta = new Vector2(0, 30);
                
                newPrefab.transform.position = newPrefab.transform.position + new Vector3(0, -PrefabList.Count*60, 0);

                MembersPrefabItem newPrefabItem = newPrefab.GetComponent<MembersPrefabItem>();
                newPrefabItem.setupPrefab((string)User["name"], (string)User["email"]);

                PrefabList.Add(newPrefabItem);
            }

            RectTransform rt = this.gameObject.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0, PrefabList.Count*100);

        });

        // return listener;

    }
}
