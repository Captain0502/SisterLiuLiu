using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 在动作的Event中添加 SoundEvent.
/// 把这个类绑到动画人物上.
/// 设置声音列表.
/// </summary>
public class PlaySound : MonoBehaviour
{
    public AudioClip [] SoundList;
    public bool PrintSoundId;
    
	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
    /// <summary>
    /// 事件参数,声音列表中的声音索引,从1开始.
    /// </summary>
    /// <param name="soundId"></param>
    public void SoundEvent(int soundId)
    {
//        soundId--;
//        if (soundId < 0) return;
//        if (PrintSoundId) Debug.Log("SoundEvent: " + soundId.ToString());
//        if (SoundList[soundId] != null)
//        {
//            AudioSource.PlayClipAtPoint(SoundList[soundId], transform.position);
//        }
    }
}
