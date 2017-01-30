using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAudioSource
{
    [TaskCategory("Basic/AudioSource")]
    [TaskDescription("Randomizes the pitch of a an audioclip before playing. Make min,max = 1 for no random pitch.")]
    public class PlayRandomSingleAudio : Action
    {
        public SharedGameObject AudioSourceGO;
        public SharedAudioClip audioClip;
        public SharedFloat volume = 1f;
        public SharedFloat randomPitchMin;
        public SharedFloat randomPitchMax;

        public override TaskStatus OnUpdate()
        {
            if (AudioSourceGO.Value != null)
            {
                GameObject go = AudioSourceGO.Value.gameObject;
               
                AudioSource audio = go.GetComponent<AudioSource>();
                AudioClip daClip = audioClip.Value;
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