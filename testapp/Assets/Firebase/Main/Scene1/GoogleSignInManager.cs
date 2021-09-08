using System;
using System.Collections.Generic;
// using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
// using Firebase.Database;
using Firebase.Extensions;
using Google;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GoogleSignInManager : MonoBehaviour
{
    public Text infoText;
    public string webClientId = "<your client id here>";

    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };

        if (!FB.IsInitialized) FB.Init(InitCallBack, OnHideUnity);
        else FB.ActivateApp();

        CheckFirebaseDependencies();
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                    auth = FirebaseAuth.DefaultInstance;
                else
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

// ################################################### Google Login ####################################################################

    public void SignInWithGoogle()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Google - Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    public void SignOutFromGoogle()
    {
        AddToInformation("Google - Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Google - Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddToInformation("Google - Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Google - Canceled");
        }
        else
        {
            AddToInformation("Google - Welcome: " + task.Result.DisplayName + "!");
            AddToInformation("Google - Email = " + task.Result.Email);
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }

    

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    AddToInformation("\nGoogle - Error code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                // firebaseRTDB();
                AddToInformation("Google - Sign In Successful.");
            }
        });
    }
    
    // private void firebaseRTDB(){
    //     Firebase.Auth.FirebaseUser user = auth.CurrentUser;
    //     var DBref = FirebaseDatabase.DefaultInstance.RootReference;
    //     // Debug.Log(user.UserId);
    //     // Dictionary<string, List<string>> db = new Dictionary<string, List<string>>();
    //     // db["hey"].Add("group1");
    //     // db["hey"].Add("group2");
    //     // List<string> group = new List<string>();
    //     // group.Add("group1");
    //     // group.Add("group2");
    //     // group.Add("group3");
    //     // group.Add("group4");
    //     // group.Add("group5");
    //     // group.Add("group6");
    //     // DBref.Child("Users").Child(user.UserId).SetValueAsync(group);
    //     // Debug.Log(DBref.Child("Users").GetValueAsync().Result.Child(user.UserId).GetRawJsonValue());
    //     Debug.Log(DBref.Child("Users").Child(user.UserId).GetValueAsync().Result.GetRawJsonValue());

    // }

// ################################################### FaceBook Login ####################################################################


    private void InitCallBack()
    {
        if (!FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            // AddToInformation("FaceBook - Failed to initialize");
        }
    }
    private void OnHideUnity(bool isgameshown)
    {
        if (!isgameshown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void Facebook_Login()
    {   
        AddToInformation("FaceBook - Calling SignIn");
        var permission = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(permission, AuthCallBack);
    }

    private async void AuthCallBack(ILoginResult result)
    {   
        if (FB.IsLoggedIn)
        {
            var accessToken = Facebook.Unity.AccessToken.CurrentAccessToken; 
            var temp = await SignInWithFaceBookOnFirebase(accessToken.TokenString);
            loadScene();
        }
        else
        {
          AddToInformation("FaceBook - User Cancelled login");
        }
    }

    public async Task<int> SignInWithFaceBookOnFirebase(string accesstoken)
    {
        // auth = FirebaseAuth.DefaultInstance;
        Credential credential = FacebookAuthProvider.GetCredential(accesstoken);
        await auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
               AddToInformation("FaceBook - singin encountered error" + task.Exception);
            }
            else{
                // firebaseRTDB();
                Firebase.Auth.FirebaseUser newuser = task.Result;
                AddToInformation("FaceBook - Sign In Successful "+ newuser.DisplayName);
            }
            
        });

        return 0;

    }


    private void loadScene(){
        // Debug.Log(StaticGidUid.UserId);
        StaticGidUid.UserId = auth.CurrentUser.UserId;
        Debug.Log(StaticGidUid.UserId);
        SceneManager.LoadScene("MainScene");
    }

    public void Facebook_Logout()
    {
        FB.LogOut();
        AddToInformation("FaceBook - Logging Out");
    }

// ###################################################################################################################################

    List<string> info = new List<string>{}; 
    private void AddToInformation(string str) {
        info.Add(str);
        if(info.Count>5)info.RemoveAt(0);
        infoText.text = string.Join("\n", info);
        // infoText.text += "\n" + str; 
    }
}