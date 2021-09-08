using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.DynamicLinks;
using Firebase.Extensions;



public class Main : MonoBehaviour
{   


    
    private string GroupID = StaticGidUid.GroupId;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    public InputField shareUrl;
    const string kInvalidDomainUriPrefix = "THIS_IS_AN_INVALID_DOMAIN";
    const string kDomainUriPrefixInvalidError =
      "kDomainUriPrefix is not valid, link shortening will fail.\n" +
      "To resolve this:\n" +
      "* Goto the Firebase console https://firebase.google.com/console/\n" +
      "* Click on the Dynamic Links tab\n" +
      "* Copy the domain e.g x20yz.app.goo.gl\n" +
      "* Replace the value of kDomainUriPrefix with the copied domain.\n";

    public string kDomainUriPrefix = kInvalidDomainUriPrefix;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    public virtual void Start() {
      // FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
      //   dependencyStatus = task.Result;
      //   if (dependencyStatus == DependencyStatus.Available) {
      //     // InitializeFirebase();
      //   } else {
      //     Debug.LogError(
      //       "Could not resolve all Firebase dependencies: " + dependencyStatus);
      //   }
      // });
    }

    // void InitializeFirebase() {
    //   DynamicLinks.DynamicLinkReceived += OnDynamicLink;
    //   firebaseInitialized = true;
    // }

    // void OnDestroy() {
    //   DynamicLinks.DynamicLinkReceived -= OnDynamicLink;
    // }


     DynamicLinkComponents CreateDynamicLinkComponents() {
#if UNITY_5_6_OR_NEWER
      string appIdentifier = Application.identifier;
#else
      string appIdentifier = Application.bundleIdentifier;
#endif

      return new DynamicLinkComponents(
        // The base Link.
        new System.Uri("https://google.com/?GId="+GroupID),
        // The dynamic link domain.
        kDomainUriPrefix) {
        // GoogleAnalyticsParameters = new Firebase.DynamicLinks.GoogleAnalyticsParameters() {
        //   Source = "mysource",
        //   Medium = "mymedium",
        //   Campaign = "mycampaign",
        //   Term = "myterm",
        //   Content = "mycontent"
        // },
        // IOSParameters = new Firebase.DynamicLinks.IOSParameters(appIdentifier) {
        //   FallbackUrl = new System.Uri("https://mysite/fallback"),
        //   CustomScheme = "mycustomscheme",
        //   MinimumVersion = "1.2.3",
        //   IPadBundleId = appIdentifier,
        //   IPadFallbackUrl = new System.Uri("https://mysite/fallbackipad")
        // },
        // ITunesConnectAnalyticsParameters =
        //   new Firebase.DynamicLinks.ITunesConnectAnalyticsParameters() {
        //     AffiliateToken = "abcdefg",
        //     CampaignToken = "hijklmno",
        //     ProviderToken = "pq-rstuv"
        //   },
        AndroidParameters = new Firebase.DynamicLinks.AndroidParameters(appIdentifier) {
          // FallbackUrl = new System.Uri("https://mysite/fallback"),
          // MinimumVersion = 12
        },
        SocialMetaTagParameters = new Firebase.DynamicLinks.SocialMetaTagParameters() {
          Title = "Amigos",
          Description = "Lets join the group!",
          ImageUrl = new System.Uri("https://mysite.com/someimage.jpg")
        },
      };
    }



    //  public Uri CreateAndDisplayLongLink() {
    //   var longLink = CreateDynamicLinkComponents().LongDynamicLink;
    //   DebugLog(String.Format("Long dynamic link {0}", longLink));
    //   return longLink;
    // }

    public void CreateAndDisplayShortLinkAsync() {
      Task<ShortDynamicLink> temp = CreateAndDisplayShortLinkAsync(new DynamicLinkOptions());
    }

    // public Task<ShortDynamicLink> CreateAndDisplayUnguessableShortLinkAsync() {
    //   return CreateAndDisplayShortLinkAsync(new DynamicLinkOptions {
    //     PathLength = DynamicLinkPathLength.Unguessable
    //   });
    // }

    private Task<ShortDynamicLink> CreateAndDisplayShortLinkAsync(DynamicLinkOptions options) {
      if (kDomainUriPrefix == kInvalidDomainUriPrefix) {
        Debug.Log(kDomainUriPrefixInvalidError);
        var source = new TaskCompletionSource<ShortDynamicLink>();
        source.TrySetException(new Exception(kDomainUriPrefixInvalidError));
        return source.Task;
      }

      var components = CreateDynamicLinkComponents();
      return DynamicLinks.GetShortLinkAsync(components, options)
        .ContinueWithOnMainThread((task) => {
          if (task.IsCanceled) {
            Debug.Log("Short link creation canceled");
          } else if (task.IsFaulted) {
            Debug.Log(String.Format("Short link creation failed {0}", task.Exception.ToString()));
          } else {
            ShortDynamicLink link = task.Result;
            Debug.Log(String.Format("Generated short link {0}", link.Url));
            shareUrl.text = link.Url.ToString();
            
            var warnings = new System.Collections.Generic.List<string>(link.Warnings);
            if (warnings.Count > 0) {
              Debug.Log("Warnings:");
              foreach (var warning in warnings) {
                Debug.Log("  " + warning);
              }
            }
          }
          return task.Result;
        });
    }



    public void backToMainScene(){
        SceneManager.LoadScene("MainScene");
    }


}
