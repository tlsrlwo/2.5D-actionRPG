using UnityEngine;

namespace KW
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerAudio : MonoBehaviour
    {
        [Header("오디오 소스")]
        [SerializeField] private AudioSource footstepSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("오디오 클립")]
        [SerializeField] private AudioClip footstepClip;

        [SerializeField] private AudioClip attackClip;
        [SerializeField] private AudioClip hitClip;
        [SerializeField] private AudioClip dashClip;

        void Awake()
        {
            if (footstepSource == null) footstepSource = GetComponent<AudioSource>();
            if (sfxSource == null) sfxSource = GetComponent<AudioSource>();
        }

        // 걷는 소리
        public void PlayFootstep()
        {
            if (footstepClip != null)
            {
                // 피치 조절로 덜 지루하게
                footstepSource.pitch = Random.Range(0.9f, 1.1f);
                //소리재생
                footstepSource.PlayOneShot(footstepClip);
            }
        }

        // 공격 소리
        public void PlayAttackSound()
        {
            if (attackClip != null)
            {
                sfxSource.pitch = Random.Range(0.9f, 1.1f);
                sfxSource.PlayOneShot(attackClip);
            }
        }
        // 피격 소리
        public void PlayHitSound()
        {
            if (hitClip != null) sfxSource.PlayOneShot(hitClip);
        }
        
        // 대쉬 소리
        public void PlayDashSound()
        {
            if (dashClip != null) sfxSource.PlayOneShot(dashClip);
        }



    }
}
