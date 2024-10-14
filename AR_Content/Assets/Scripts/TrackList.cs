using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackList : MonoBehaviour
{
    [SerializeField] private List<Music> musics = new List<Music>();

    //노래 Data 반환
    public Music GetMusic(string name)
    {
        Music music = null;

        for (int i = 0; i < musics.Count; i++)
        {
            if (musics[i].AlbumName == name)
            {
 
                music = musics[i];
 
                return music;
            }
             
        }
         
        return null;
    }

}
