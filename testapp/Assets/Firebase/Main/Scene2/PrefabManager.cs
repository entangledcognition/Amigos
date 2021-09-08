using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;

public class PrefabManager : MonoBehaviour
{

    FirebaseFirestore db;
    public GameObject Prefab;
    private string Uid;

    // Start is called before the first frame update
    void Start(){

        db = FirebaseFirestore.DefaultInstance;
        Uid = StaticGidUid.UserId;   
        getDataRealTime();                
    }

    private void Awake(){
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    List<PrefabItem> PrefabList = new  List<PrefabItem>();

    private void clearPrefabArea(){
        foreach(PrefabItem item in PrefabList)
            {
                Destroy(item.gameObject);
            }
            PrefabList.Clear();
    }
    
    private void getDataRealTime(){

        Query query = db.Collection("Groups").WhereArrayContains("members", Uid);

        ListenerRegistration listener = query.Listen(snapshot => {
        Debug.Log("Callback received query snapshot.");
        
        clearPrefabArea();

        foreach (DocumentSnapshot documentSnapshot in snapshot.Documents) {

            Dictionary<string, object> group = documentSnapshot.ToDictionary();
            Debug.Log(group["name"]);
            Debug.Log(documentSnapshot.Id);

            GameObject newPrefab = GameObject.Instantiate(Prefab, this.gameObject.transform);

            RectTransform rtrans = newPrefab.GetComponent<RectTransform>();
            rtrans.sizeDelta = new Vector2(0, 30);
            
            newPrefab.transform.position = newPrefab.transform.position + new Vector3(0, -PrefabList.Count*60, 0);

            PrefabItem newPrefabItem = newPrefab.GetComponent<PrefabItem>();
            newPrefabItem.setupPrefab((string)group["name"], (string)documentSnapshot.Id);

            PrefabList.Add(newPrefabItem);
        }

        RectTransform rt = this.gameObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0, PrefabList.Count*100);

        });

    }

    public void OnClick(){
        Debug.Log("Clicked");
    }
}
