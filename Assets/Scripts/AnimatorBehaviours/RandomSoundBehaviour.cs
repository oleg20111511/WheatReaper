using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundBehaviour : StateMachineBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioSource audioSource = animator.gameObject.GetComponent<AudioSource>();
        int chosenClipIndex = Random.Range(0, audioClips.Count);
        AudioClip chosenClip = audioClips[chosenClipIndex];
        audioSource.clip = chosenClip;
        audioSource.Play();
    }
}
