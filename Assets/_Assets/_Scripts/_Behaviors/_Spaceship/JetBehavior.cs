using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class JetBehavior : MonoBehaviour 
{
	ParticleSystem psys;
    ParticleSystem.MinMaxCurve onEmission;
    ParticleSystem.MinMaxCurve downEmission;

	Light clarao;
	
	//bool isOn;
	float baseIntensity;
	float intensity = 1.0f;
	float speed;

	AudioSource jetClip;
	float volume;

	// Use this for initialization
	void Awake () 
	{
		psys = gameObject.GetComponent<ParticleSystem>();
        clarao = gameObject.GetComponent<Light>();
		baseIntensity = GetComponent<Light>().intensity;

        onEmission = new ParticleSystem.MinMaxCurve(100);
        downEmission = new ParticleSystem.MinMaxCurve(10);

        jetClip = GetComponent<AudioSource>();
        if (jetClip)
        {
            // TODO change when AudioManager is implemented.
            GameData gd = ServiceLocator.GetGameData();
            AudioManager am = ServiceLocator.GetAudioManager();
            jetClip.clip = am.GetClip(AudioManager.Clips.JETENGINES);
            volume = 0.25f * gd.GetOverallVolume();
            jetClip.volume = 0;
            jetClip.Play();
        }

		StartCoroutine(GetDown());
	}
	
	//Engatando os motores
	public IEnumerator GetOn()
	{		
		if(jetClip && !jetClip.isPlaying)
			jetClip.Play();
		
        //Aumentando a velocidade das particulas
		while(speed < 2.0f || intensity < baseIntensity)
		{
			if(jetClip)
				jetClip.volume += volume * Time.deltaTime;

			if(speed < 2.0f)	speed += Time.deltaTime * 4.0f;
			psys.startSpeed = speed;

            //Aumentando a emissao das particulas
			var emission = psys.emission;
            emission.rate = onEmission;

			
			//Aumentando a intensidade da luz das particulas
			if(intensity < baseIntensity)	intensity += Time.deltaTime * baseIntensity;
	        clarao.intensity = intensity;		
			
			//Espera um frame
			yield return new WaitForEndOfFrame();
		}
	}
	
	//Desligando os motores
	public IEnumerator GetDown()
	{
		//isOn = false;
		//Diminuindo a velocidade das particulas
		while(speed > 0.0f || intensity > 0.0f)
		{
			if(jetClip)
				jetClip.volume -= volume * Time.deltaTime;

			if(speed > 0.0f)	speed -= Time.deltaTime * 4.0f;
			psys.startSpeed = speed;
			
			//Diminuindo a emissao das particulas
			var emission = psys.emission;
            emission.rate = downEmission;
			
			//Diminuindo a intensidade da luz das particulas
			if(intensity > 0.0f)	intensity -= Time.deltaTime * baseIntensity;
	        clarao.intensity = intensity;
			
			//Espera um frame
			yield return new WaitForEndOfFrame();
		}

		if(jetClip)
			jetClip.Stop();
	}
}
