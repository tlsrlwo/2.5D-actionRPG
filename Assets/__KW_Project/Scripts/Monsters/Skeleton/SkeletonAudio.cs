
using UnityEngine;

namespace KW
{
    public class SkeletonAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource sfxSource;

        public AudioClip grawlClip;
        public AudioClip attackClip;

        void Awake()
        {
            if (sfxSource == null) GetComponent<AudioSource>();
        }

        public void PlayAttackSound()
        {
            if (attackClip != null)
            {
                sfxSource.pitch = Random.Range(0.9f, 1.1f);
                sfxSource.PlayOneShot(attackClip);
            }
        }

        


    }
}
