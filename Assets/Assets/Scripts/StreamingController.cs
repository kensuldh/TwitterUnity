using UnityEngine;
using System.Collections;

namespace Streamer {
    public class StreamingController : MonoBehaviour
    {
        TwitterAccess TweetAcc = new TwitterAccess();
        System.Action<string> callback = System.Console.WriteLine;
        
        // Use this for initialization
        void Start()
        {
            callback("http://spiknsk.jp/oauth.php");
            //TweetAcc.BasicAuthUserPassword("kensu_ldh","kensu0129flower");
            TweetAcc.GetOAuthURL(callback);
        }

        // Update is called once per frame
        void Update()
        {

        }
                
    }
}