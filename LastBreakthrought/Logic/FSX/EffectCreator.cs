using LastBreakthrought.Infrustructure.AssetManagment;
using LastBreakthrought.Util;
using System.Collections;
using UnityEngine;

namespace LastBreakthrought.Logic.FSX
{
    public class EffectCreator
    {
        private ICoroutineRunner _coroutineRunner;

        public EffectCreator(ICoroutineRunner coroutineRunner) => 
            _coroutineRunner = coroutineRunner;


        public void CreateLightningEffect(Transform at) => 
            CreateEffectAt(AssetPath.LightningEffectPath, at);

        public void CreateFireEffect(Transform at) => 
            CreateEffectAt(AssetPath.FireEffectPath, at);
        public void CreateExplosionEffect(Transform at) => 
            CreateEffectAt(AssetPath.FireExplosionPath, at);

        public void CreateEffectAt(string effectPath, Transform at)
        {
            var effectPrefab = CreateEffect(effectPath, at);

            DestroyEffect(effectPrefab);
        }

        private ParticleSystem CreateEffect(string effectPath, Transform at)
        {
            var effect = Resources.Load<ParticleSystem>(effectPath);
            var effectPrefab = Object.Instantiate(effect, at);
            return effectPrefab;
        }

        private IEnumerator DestroyEffect(ParticleSystem particle)
        {
            float waitTime = particle.main.duration + particle.main.startLifetime.constantMax;

            yield return new WaitForSeconds(waitTime);

            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            Object.Destroy(particle);
        }
    }
}
