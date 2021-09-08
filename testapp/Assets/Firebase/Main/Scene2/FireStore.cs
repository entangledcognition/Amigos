using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;



public class FireStore : MonoBehaviour
{



    // Start is called before the first frame update
    // FirebaseFirestore db;

    public InputField GroupName;
    public InputField GroupID;

    // public GameObject Prefab;

    void Awake(){

        

    }

    //Add a listener for dynamic link

    // IEnumerator addMembersCoroutine(){
    //     // string name = GroupName.text;
    //     // string uid = StaticGidUid.UserId;
    //     byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
    //     using (UnityWebRequest req = UnityWebRequest.Put("https://us-central1-ec-app-amigos.cloudfunctions.net/addMember/?gid=" + GroupID + "&uid=" + UserId, myData))
    //       {
    //           yield return req.SendWebRequest();

    //         if (req.result != UnityWebRequest.Result.Success)
    //         {
    //             Debug.Log(req.error);
    //         }
    //         else
    //         {
    //             Debug.Log("Form upload complete!");
    //         }
    //       }
    // }

    void Start()
    {
        // db = FirebaseFirestore.DefaultInstance;
        // getDataRealTime();
        
    }

    public void createGroup(){
        StartCoroutine(createGroupCoroutine());
    }

    IEnumerator createGroupCoroutine(){
        string name = GroupName.text;
        string uid = StaticGidUid.UserId;
        byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
        using (UnityWebRequest req = UnityWebRequest.Put("https://us-central1-ec-app-amigos.cloudfunctions.net/createGroup/?name=" + name + "&uid=" + uid, myData))
          {
              yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
          }
    }

    public void leaveGroup(){
        StartCoroutine(leaveGroupCoroutine());
    }


    IEnumerator leaveGroupCoroutine(){
        string gid = GroupID.text;
        string uid = StaticGidUid.UserId;
        byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
        using (UnityWebRequest req = UnityWebRequest.Put("https://us-central1-ec-app-amigos.cloudfunctions.net/leaveGroup/?gid=" + gid + "&uid=" + uid, myData))
          {
              yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
          }
    }

    // List<PrefabDes> PrefabList = new  List<PrefabDes>();

    // private void clearPrefabArea(){
    //     foreach(PrefabDes item in PrefabList)
    //         {
    //             Destroy(item.gameObject);
    //         }
    //         PrefabList.Clear();
    // }
    
    // private void getDataRealTime(){

    //     Query query = db.Collection("Groups").WhereArrayContains("members", "shqav4SFZOgh79GTdZO3cxjJnIl1");

    //     ListenerRegistration listener = query.Listen(snapshot => {
    //     Debug.Log("Callback received query snapshot.");
        
    //     clearPrefabArea();
    //     int i=0;
    //     foreach (DocumentSnapshot documentSnapshot in snapshot.Documents) {
    //         Dictionary<string, object> group = documentSnapshot.ToDictionary();
    //         Debug.Log(group["name"]);
    //         Debug.Log(documentSnapshot.Id);

    //         GameObject newPrefab = GameObject.Instantiate(Prefab, this.gameObject.transform);
    //         newPrefab.transform.position = newPrefab.transform.position + new Vector3(0, -i*100, 0);
    //         // GameObject newPrefab = Instantiate(Prefab, transform.position, transform.rotation) as GameObject;

    //         // GameObject newPrefab = Instantiate(Prefab, new Vector2(0, i*2.0F), Quaternion.identity) as GameObject;
            
    //         // newPrefab.transform.Translate(0, 10, 0)
    //         PrefabDes newPrefabDes = newPrefab.GetComponent<PrefabDes>();

    //         newPrefabDes.setupPrefab((string)group["name"], (string)documentSnapshot.Id);

    //         PrefabList.Add(newPrefabDes);
    //         i++;
    //     }

    //     RectTransform rt = this.gameObject.GetComponent<RectTransform>();
    //     rt.sizeDelta = new Vector2(0, PrefabList.Count*100);

    //     });

    // }
    // Update is called once per frame
    void Update()
    {
        
    }
}
