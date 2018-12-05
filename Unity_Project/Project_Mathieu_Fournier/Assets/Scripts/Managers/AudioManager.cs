using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager m_Instance;
    public static AudioManager Instance
    {
        get { return m_Instance; }
    }

    [SerializeField]
    private AudioSource m_MusicSource;
    [SerializeField]
    private List<AudioClip> m_Musics = new List<AudioClip>();
    private float m_MusicVolume;

    [SerializeField]
    private List<SFXAudio> m_SFXAudios = new List<SFXAudio>();
    [SerializeField]
    private List<AudioClip> m_SFXClips = new List<AudioClip>();
    [Tooltip("Only one instance of these SFXClips in the scene at once.")]
    [SerializeField]
    private List<string> m_UniqueClipNames = new List<string>();
    private Dictionary<string, SFXAudio> m_UniqueSFXRef = new Dictionary<string, SFXAudio>();

    private float m_SFXVolume;

    private void Awake()
    {
        if(AudioManager.Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(m_MusicSource);

        for(int i = 0; i < m_UniqueClipNames.Count; i++)
        {
            m_UniqueSFXRef.Add(m_UniqueClipNames[i], null);
        }
    }

    //FadeOutFadeInMusic
    private IEnumerator SwitchMusicRoutine(float a_Duration, AudioClip a_NextClip)
    {
        //Error Example.
        if (a_Duration <= 0)
        {
            Debug.LogError("Duration is <= 0, so you should never use this function, tell Mathieu F is this happens.");
            yield break;
        }

        while (m_MusicSource.volume > 0f)
        {
            m_MusicSource.volume -= Time.deltaTime / a_Duration;
            yield return null;
        }

        m_MusicSource.clip = a_NextClip;
        m_MusicSource.Play();

        while (m_MusicSource.volume < 1f)
        {
            m_MusicSource.volume += Time.deltaTime / a_Duration;
            yield return null;
        }
    }

    public void SwitchMusic(string a_MusicName, float a_Duration)
    {
        for(int i = 0; i < m_Musics.Capacity; i++)
        {
            if(m_Musics[i].name == a_MusicName)
            {
                AudioClip nextClip = m_Musics[i];
                StartCoroutine(SwitchMusicRoutine(a_Duration, nextClip));
                return;
            }
        }

        Debug.LogWarning("Wrong Music Name, AudioManager Unable to change music. Mathieu F");
    }

    public void PlaySFX(int a_SFXIndex, string a_ClipName, Vector3 i_Position)
    {
        for(int i = 0; i < m_SFXClips.Capacity; i++)
        {
            if(m_SFXClips[i].name == a_ClipName)
            {
                //If the unique sfx already exist in the scene destroy the old one and keep the new one
                for (int x = 0; x < m_UniqueClipNames.Count; x++)
                {
                    if(m_UniqueClipNames[x] == a_ClipName)
                    {
                        if(m_UniqueSFXRef[a_ClipName] != null)
                        {
                            Destroy(m_UniqueSFXRef[a_ClipName].gameObject);
                        }
                        SFXAudio sfx = Instantiate(m_SFXAudios[a_SFXIndex], i_Position, Quaternion.identity);
                        sfx.SetUp(m_SFXClips[i]);
                        sfx.Play();
                        m_UniqueSFXRef[a_ClipName] = sfx;
                        return;
                    }
                }

                SFXAudio audio = Instantiate(m_SFXAudios[a_SFXIndex], i_Position, Quaternion.identity);
                audio.SetUp(m_SFXClips[i]);
                audio.Play();
                return;
            }
        }

        Debug.LogWarning("Wrong Clip Name, AudioManager Unable to play SFX. Mathieu F");
    }

    public void SetMusicVolume(float a_Volume)
    {
        m_MusicSource.volume = a_Volume;
    }

    public void SetSFXVolume(float a_Volume)
    {
        for(int i = 0; i < m_SFXAudios.Capacity; i++)
        {
            m_SFXAudios[i].SetSFXVolume(a_Volume);
        }
    }
}
