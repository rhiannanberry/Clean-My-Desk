using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twity.DataModels.Core;

public class TweetHandler : MonoBehaviour {
    private bool post = false;
    private string message = "";
    private byte[] imgBinary;
    private string imgEncoded;
    // Use this for initialization
    void Start () {
        Twity.Oauth.consumerKey = SECRETS.consumerKey;
        Twity.Oauth.consumerSecret = SECRETS.consumerSecretKey;
        Twity.Oauth.accessToken = SECRETS.accessToken;
        Twity.Oauth.accessTokenSecret = SECRETS.accessTokenSecret;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate() {
        if (post) {
            //Debug.Log(TakeScreenshot().Length);
            Post();
            post = false;
        }
    }

    private void MediaCallback(bool success, string response) {
        if (success) {
            UploadMedia media = JsonUtility.FromJson<UploadMedia>(response);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["media_ids"] = media.media_id.ToString();
            parameters["status"] = message;
            StartCoroutine(Twity.Client.Post("statuses/update", parameters, Callback));
        } else {
            Debug.Log(response);
        }
    }

    private void Callback(bool success, string response) {
        if (success) {
            Tweet tweet = JsonUtility.FromJson<Tweet>(response);
        } else {
            Debug.Log(response);
        }
    }

    public void TriggerPost(string message) {
        post = true;
        this.message = message;
    }

    private void Post(byte[] imgBinary = null) {
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters["status"] = message;
        if (imgBinary != null) {
            this.imgBinary = imgBinary;
            imgEncoded = System.Convert.ToBase64String(imgBinary);
            PostMedia();
            
            
            return;
        }
        StartCoroutine(Twity.Client.Post("statuses/update", parameters, Callback));
    }

    public void Get() {
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters["q"] = "search word";
        parameters["count"] = 30.ToString(); ;
        StartCoroutine(Twity.Client.Get("search/tweets", parameters, Callback));
    }

    private byte[] TakeScreenshot() {
        //ScreenCapture.CaptureScreenshot("Assets/Resources/YEET.png");
        //return System.IO.File.ReadAllBytes("Assets/Resources/YEET.png");
        //return (Resources.Load("YEET") as Texture2D).EncodeToPNG();
        Vector2 resDown = ReduceResolution(Screen.width, Screen.height, 800);
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 16);
        Camera.main.targetTexture = rt;
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Camera.main.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        System.IO.File.WriteAllBytes("Assets/test.jpg", screenshot.EncodeToJPG(50));
        return screenshot.EncodeToJPG(50);
    }

    private Vector2 ReduceResolution(int width, int height, int maxDimension, bool matchWidth = true) {
        if (!(maxDimension > width && maxDimension > height)) {
            float modifier = matchWidth ? width / maxDimension : height / maxDimension;
            height = matchWidth ? (int)(height / modifier) : maxDimension;
            width = matchWidth ? maxDimension : (int)(width / modifier);
        }
        return new Vector2(width, height);
    }

    private void PostMedia() {
        InitPostMedia();
    }

    private void InitPostMedia() {
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters["command"] = "INIT";
        parameters["total_bytes"] = (imgBinary.Length).ToString();
        parameters["media_type"] = "image/jpeg";
        parameters["media_category"] = "tweet_image";

        StartCoroutine(Twity.Client.Post("media/upload", parameters, InitCallback));
    }

    private void InitCallback(bool success, string response) {
        Debug.Log(response);
    }
}
