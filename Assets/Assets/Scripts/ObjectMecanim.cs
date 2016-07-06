using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectMecanim : MonoBehaviour {

    TwitterComponentHandler Result = new TwitterComponentHandler();
    public GameObject inputPINField;
    public GameObject TweetResulut;
    public Rigidbody2D Star;
    public Vector2 Origin = new Vector2();
    bool success;
    public float TimeOut;
    private float TimeElapsed;
    Twitter.RequestTokenResponse response;
    private string CONSUMER_KEY = "PUhRhZcpxbcpd2eUWgjdvxb1N";
    private string CONSUMER_SECRET = "qb3JPFWXCfEWSM8EVFHbrZHnaDCEOx84YptoXNhsCrTpNDeVES";
    public string SearchTag;
    private int condition;

    // Use this for initialization
    void Start () {
        condition = 0;
        TimeElapsed = 0.0f;
        StartCoroutine(Twitter.API.GetRequestToken(CONSUMER_KEY, CONSUMER_SECRET,
                new Twitter.RequestTokenCallback(Result.OnRequestTokenCallback)));
        condition = 1;
    }

    public void OnClickAuthPINButon2()
    {
        if (condition == 1)
        {
            string myPIN = inputPINField.GetComponent<InputField>().text;

            StartCoroutine(Twitter.API.GetAccessToken(CONSUMER_KEY, CONSUMER_SECRET, Result.m_RequestTokenResponse.Token, myPIN,
                new Twitter.AccessTokenCallback(Result.OnAccessTokenCallback)));
            condition = 2;
        }
    }

    // Update is called once per frame
    void Update () {
        TimeElapsed += Time.deltaTime;
        if (TimeElapsed >= TimeOut)
        {
            if (condition == 2)
            {
                StartCoroutine(Twitter.API.SearchTweets(SearchTag, CONSUMER_KEY, CONSUMER_SECRET, Result.m_AccessTokenResponse, Result.OnSearchTweetsResponse));

                if (Result.GetSearch())
                {
                    if (Result.GetTweetCount() > 0)
                    {
                        Star.AddForce(new Vector2(-2.0f, -1.0f));
                        print("Gets");
                    }
                    else
                    {
                        Star.MovePosition(Origin);
                    }
                    print(Result.GetTweetCount());
                }
                else
                {
                    Star.MovePosition(Origin);
                }
            }
            TimeElapsed = 0.0f;
        }
	}
}
