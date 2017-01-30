using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAudioSource
{
    [TaskCategory("Basic/AudioSource")]
    [TaskDescription("Plays a random audioclip from an audioclip list. Also randomizes the pitch. Make min,max = 1 for no random pitch.")]
    public class PlayRandomAudio: Action
    {
        public SharedGameObject AudioSourceGO;
        public SharedAudioClipList audioClips;      
        public SharedFloat volume = 1f;
        public SharedFloat randomPitchMin;
        public SharedFloat randomPitchMax;   

        public override TaskStatus OnUpdate()
        {
            if(AudioSourceGO.Value != null)
            {
                GameObject go = AudioSourceGO.Value.gameObject;
                int max = audioClips.Value.Count;
                AudioSource audio = go.GetComponent<AudioSource>();
                AudioClip daClip = audioClips.Value[Random.Range(0, max)];
                audio.clip = daClip;
                audio.pitch = Random.Range(randomPitchMin.Value, randomPitchMax.Value);
                audio.Play();
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
          
        }

        public override void OnReset()
        {
            gameObject = null;
            randomPitchMin = 0.80f;
            randomPitchMax = 1.20f;           
         
            volume = 1;    
        }
        }
    }