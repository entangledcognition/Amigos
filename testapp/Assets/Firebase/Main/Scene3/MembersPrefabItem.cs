using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MembersPrefabItem : MonoBehaviour
{

    public Text UserName;
    public Text UserEmail;

    public void setupPrefab(string Name, string Email){
        UserName.text = Name;
        UserEmail.text = Email;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
