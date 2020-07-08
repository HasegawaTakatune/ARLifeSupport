using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechRecognizer : MonoBehaviour
{

    [SerializeField] private Text text;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                StartRecognizer();
            }
        }
    }

    private void StartRecognizer()
    {
#if UNITY_ANDROID
        AndroidJavaClass nativeRecognizer = new AndroidJavaClass("com.reryka.speechplugin.NativeSpeechRecognizer");
        text.text = "NativeSpeechRecognizer\n";
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        text.text += "UnityPlayer\n";
        AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        text.text += "currentActivity\n";

        context.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            nativeRecognizer.CallStatic(
            "StartRecognizer",
            context,
            gameObject.name,
            "CallbackMethod"
            );
        }));
#endif 
    }

    private void CallbackMethod(string message)
    {
        string[] messages = message.Split('\n');
        if (messages[0] == "onResults")
        {
            string msg = "";
            for (int i = 1; i < messages.Length; i++)
            {
                msg += messages[i] + "\n";
            }

            text.text = msg;
            Debug.Log(msg);
        }
        else
        {
            text.text = message + "\n";
            Debug.Log(message);
        }
    }
}
