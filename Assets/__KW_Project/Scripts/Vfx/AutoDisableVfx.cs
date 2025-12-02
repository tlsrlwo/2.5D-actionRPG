using UnityEngine;

namespace KW
{
    public class AutoDisableVfx : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 0.5f;
        [SerializeField] private bool useParticleDuration = false;

        private ParticleSystem ps;

        private void Awake()
        {
            ps = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            // 파티클 재생
            if (ps != null)
            {
                ps.Play();
            }

            float duration = lifeTime;

            CancelInvoke(nameof(DisableMe));
            Invoke(nameof(DisableMe), duration);
        }

        private void DisableMe()
        {
            gameObject.SetActive(false);
        }
    }
}
