using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class TwitterComponentHandler : MonoBehaviour
{
    public GameObject inputPINField;
    public GameObject inputTweetField;
    public GameObject Text;
    private float TweetCount = 0.0f;
    public string SearchTag;
    public bool IsSearch;

    private string CONSUMER_KEY = "PUhRhZcpxbcpd2eUWgjdvxb1N";
    private string CONSUMER_SECRET = "qb3JPFWXCfEWSM8EVFHbrZHnaDCEOx84YptoXNhsCrTpNDeVES";
    private int condition = 0;

    public Twitter.RequestTokenResponse m_RequestTokenResponse;
    public Twitter.AccessTokenResponse m_AccessTokenResponse;

    const string PLAYER_PREFS_TWITTER_USER_ID = "TwitterUserID";
    const string PLAYER_PREFS_TWITTER_USER_SCREEN_NAME = "TwitterUserScreenName";
    const string PLAYER_PREFS_TWITTER_USER_TOKEN = "TwitterUserToken";
    const string PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET = "TwitterUserTokenSecret";

    const string PLAYER_PREFS_TWITTER_TWEETED_IDS = "TwitterTweetedIDs";

    // Use this for initialization
    void Start()
    {
        IsSearch = false;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnClickSerchByHashTag()
    {
        if (condition == 2)
        {
            // ハッシュタグ "#Unity" で検索（終了するとOnSearchTweetsResponseが実行される）
            StartCoroutine(Twitter.API.SearchTweets(SearchTag, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse, OnSearchTweetsResponse));
        }
    }

    public void OnSearchTweetsResponse(bool success, string response)
    {
        // responseに検索結果のJSON文字列が入ってくるので解析して各ツイートのテキストを得る
        if (success)
        {
            print("OnSearchTweet - succedded.");
            JSONObject json = new JSONObject(response);
            //JSONObject json1 = json[0];
            JSONObject statuses = json.GetField("statuses");
            JSONObject search_metadata = json.GetField("search_metadata");
            JSONObject count = search_metadata.GetField("count");
            TweetCount = count.n;
            print(statuses.Count); //取得ツイート数
                                    /*
                                    for (int i = 0; i < tweets.Length; i++)
                                    {
                                        print(tweets[i].GetField("text").toString()); //ツイート内容
                                        print(tweets[i].GetField("created_at").toString()); //ツイート時間
                                    }
                                    */
            Text.GetComponent<Text>().text = response;
            IsSearch = true;
        }
        else
        {
            print("OnSearchTweet - failed.");
            IsSearch = false;
            
        }
    }

    public bool GetSearch()
    {
        return IsSearch;
    }

    public float GetTweetCount()
    {
        return TweetCount;
    }

    /* OnClick Event */


    public void OnClickGetPINButon()
    {
        print("Get PIN...");
        if (condition == 0)
        {            
            StartCoroutine(Twitter.API.GetRequestToken(CONSUMER_KEY, CONSUMER_SECRET,
                new Twitter.RequestTokenCallback(this.OnRequestTokenCallback)));
            condition = 1;
        }
    }

    public void OnClickAuthPINButon()
    {
        if (condition == 1)
        {
            string myPIN = inputPINField.GetComponent<InputField>().text;

            StartCoroutine(Twitter.API.GetAccessToken(CONSUMER_KEY, CONSUMER_SECRET, m_RequestTokenResponse.Token, myPIN,
                new Twitter.AccessTokenCallback(this.OnAccessTokenCallback)));
            condition = 2;
        }
    }

    public void OnClickTweetButon()
    {
        if (condition == 2)
        {
            string myTweet = inputTweetField.GetComponent<InputField>().text;

            StartCoroutine(Twitter.API.PostTweet(myTweet, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse,
                new Twitter.PostTweetCallback(this.OnPostTweet)));
        }
    }

    /* Callback Event */


    public void OnRequestTokenCallback(bool success, Twitter.RequestTokenResponse response)
    {
        if (success)
        {
            string log = "OnRequestTokenCallback - succeeded";
            log += "\n    Token : " + response.Token;
            log += "\n    TokenSecret : " + response.TokenSecret;
            print(log);

            m_RequestTokenResponse = response;

            print(response.Token);
            print(response.TokenSecret);

            Twitter.API.OpenAuthorizationPage(response.Token);
        }
        else
        {
            print("OnRequestTokenCallback - failed.");
        }
    }

    public void OnAccessTokenCallback(bool success, Twitter.AccessTokenResponse response)
    {
        if (success)
        {
            string log = "OnAccessTokenCallback - succeeded";
            log += "\n    UserId : " + response.UserId;
            log += "\n    ScreenName : " + response.ScreenName;
            log += "\n    Token : " + response.Token;
            log += "\n    TokenSecret : " + response.TokenSecret;
            print(log);

            m_AccessTokenResponse = response;

            PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_ID, response.UserId);
            PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_SCREEN_NAME, response.ScreenName);
            PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_TOKEN, response.Token);
            PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET, response.TokenSecret);

        }
        else
        {
            print("OnAccessTokenCallback - failed.");
        }
    }

    public void OnPostTweet(bool success)
    {
        print("OnPostTweet - " + (success ? "succedded." : "failed."));
    }

}
