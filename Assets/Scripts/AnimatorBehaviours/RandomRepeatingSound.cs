using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRepeatingSound : StateMachineBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    private AudioSource audioSource;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioSource = animator.gameObject.GetComponent<AudioSource>();
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!audioSource.isPlaying)
        {
            int chosenClipIndex = Random.Range(0, audioClips.Count);
            AudioClip chosenClip = audioClips[chosenClipIndex];
            audioSource.clip = chosenClip;
            audioSource.Play();
        }
    }
}
