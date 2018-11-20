using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//対象のtransformをアプリ開始時にロード、終了時にセーブする
public class SavePlayingTransform : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] string prefsKeyString;

    [SerializeField] bool isLoad = true; //ロードの必要が無い時はチェックを外す
    [SerializeField] bool isSave = true; //セーブの必要が無い時はチェックを外す

    // Start is called before the first frame update
    void Start()
    {
        if (!isLoad) { return; }
        if (string.IsNullOrEmpty(prefsKeyString)) { return; }
        if (target == null) { return; }

        string posKey = prefsKeyString + "_position";
        if (PlayerPrefs.HasKey(posKey))
        {
            string posString = PlayerPrefs.GetString(posKey);
            Vector3 pos = JsonUtility.FromJson<Vector3>(posString);
            target.localPosition = pos;
        }

        string rotKey = prefsKeyString + "_rotation";
        if (PlayerPrefs.HasKey(rotKey))
        {
            string rotString = PlayerPrefs.GetString(rotKey);
            Quaternion rot = JsonUtility.FromJson<Quaternion>(rotString);
            target.localRotation = rot;
        }

        string scaleKey = prefsKeyString + "_scale";
        if (PlayerPrefs.HasKey(scaleKey))
        {
            string scaleString = PlayerPrefs.GetString(scaleKey);
            Vector3 scale = JsonUtility.FromJson<Vector3>(scaleString);
            target.localScale = scale;
        }
    }

    private void OnApplicationQuit()
    {
        if (!isSave) {return; }
        if (string.IsNullOrEmpty(prefsKeyString)) { return; }
        if (target == null) { return; }

        string posKey = prefsKeyString + "_position";
        PlayerPrefs.SetString(posKey, JsonUtility.ToJson(target.localPosition));

        string rotKey = prefsKeyString + "_rotation";
        PlayerPrefs.SetString(rotKey, JsonUtility.ToJson(target.localRotation));

        string scaleKey = prefsKeyString + "_scale";
        PlayerPrefs.SetString(scaleKey, JsonUtility.ToJson(target.localScale));
    }
}
