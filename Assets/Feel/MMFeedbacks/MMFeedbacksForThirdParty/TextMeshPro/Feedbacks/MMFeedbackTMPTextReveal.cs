﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MoreMountains.Feedbacks
{
    /// <summary>
    /// This feedback will let you reveal words, lines, or characters in a target TMP, one at a time
    /// </summary>
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback will let you reveal words, lines, or characters in a target TMP, one at a time")]
    [FeedbackPath("TextMesh Pro/TMP Text Reveal")]
    public class MMFeedbackTMPTextReveal : MMFeedback
    {
#if UNITY_EDITOR
        public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.TMPColor; } }
#endif
        /// the duration of this feedback 
        public override float FeedbackDuration
        {
            get
            {
                if (DurationMode == DurationModes.TotalDuration)
                {
                    return RevealDuration;
                }
                else
                {
                    if ((TargetTMPText == null) || (TargetTMPText.textInfo == null))
                    {
                        return 0f;
                    }
                    
                    switch (RevealMode)
                    {
                        case RevealModes.Character:
                            return TargetTMPText.text.Length * IntervalBetweenReveals;
                        case RevealModes.Lines:
                            return TargetTMPText.textInfo.lineCount * IntervalBetweenReveals;
                        case RevealModes.Words:
                            return TargetTMPText.textInfo.wordCount * IntervalBetweenReveals;
                    }
                    return 0f;
                }                
            }
            set
            {
                if (DurationMode == DurationModes.TotalDuration)
                {
                    RevealDuration = value;
                }
                else
                {
                    if (TargetTMPText != null)
                    {
                        switch (RevealMode)
                        {
                            case RevealModes.Character:
                                IntervalBetweenReveals = value / TargetTMPText.text.Length;
                                break;
                            case RevealModes.Lines:
                                IntervalBetweenReveals = value / TargetTMPText.textInfo.lineCount;
                                break;
                            case RevealModes.Words:
                                IntervalBetweenReveals = value / TargetTMPText.textInfo.wordCount;
                                break;
                        }
                    }
                }
            }
        }

        /// the possible ways to reveal the text
        public enum RevealModes { Character, Lines, Words }
        /// whether to define duration by the time interval between two unit reveals, or by the total duration the reveal should take
        public enum DurationModes { Interval, TotalDuration }

        [Header("TextMesh Pro")]
        
        /// the target TMP_Text component we want to change the text on
        [Tooltip("the target TMP_Text component we want to change the text on")]
        public TMP_Text TargetTMPText;

        [Header("Change Text")]

        /// whether or not to replace the current TMP target's text on play
        [Tooltip("whether or not to replace the current TMP target's text on play")]
        public bool ReplaceText = false;
        /// the new text to replace the old one with
        [Tooltip("the new text to replace the old one with")]
        [TextArea]
        public string NewText = "Hello World";

        [Header("Reveal")]
        
        /// the selected way to reveal the text (character by character, word by word, or line by line)
        [Tooltip("the selected way to reveal the text (character by character, word by word, or line by line)")]
        public RevealModes RevealMode = RevealModes.Character;
        /// whether to define duration by the time interval between two unit reveals, or by the total duration the reveal should take
        [Tooltip("whether to define duration by the time interval between two unit reveals, or by the total duration the reveal should take")]
        public DurationModes DurationMode = DurationModes.Interval;
        /// the interval (in seconds) between two reveals
        [Tooltip("the interval (in seconds) between two reveals")]
        [MMFEnumCondition("DurationMode", (int)DurationModes.Interval)]
        public float IntervalBetweenReveals = 0.05f;
        /// the total duration of the text reveal, in seconds
        [Tooltip("the total duration of the text reveal, in seconds")]
        [MMFEnumCondition("DurationMode", (int)DurationModes.TotalDuration)]
        public float RevealDuration = 1f;

        protected float _delay;
        protected Coroutine _coroutine;
        
        /// <summary>
        /// On play we change the text of our target TMPText
        /// </summary>
        /// <param name="position"></param>
        /// <param name="feedbacksIntensity"></param>
        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active)
            {
                return;
            }

            if (TargetTMPText == null)
            {
                return;
            }

            if (ReplaceText)
            {
                TargetTMPText.text = NewText;
                TargetTMPText.ForceMeshUpdate();
            }

            switch (RevealMode)
            {
                case RevealModes.Character:
                    _delay = (DurationMode == DurationModes.Interval) ? IntervalBetweenReveals : RevealDuration / TargetTMPText.text.Length;
                    TargetTMPText.maxVisibleCharacters = 0;
                    _coroutine = StartCoroutine(RevealCharacters());
                    break;
                case RevealModes.Lines:
                    _delay = (DurationMode == DurationModes.Interval) ? IntervalBetweenReveals : RevealDuration / TargetTMPText.textInfo.lineCount;
                    TargetTMPText.maxVisibleLines = 0;
                    _coroutine = StartCoroutine(RevealLines());
                    break;
                case RevealModes.Words:
                    _delay = (DurationMode == DurationModes.Interval) ? IntervalBetweenReveals : RevealDuration / TargetTMPText.textInfo.wordCount;
                    TargetTMPText.maxVisibleWords = 0;
                    _coroutine = StartCoroutine(RevealWords());
                    break;
            }
        }

        /// <summary>
        /// Reveals characters one at a time
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator RevealCharacters()
        {
            int totalCharacters = TargetTMPText.text.Length;
            int visibleCharacters = 0;

            while (visibleCharacters <= totalCharacters)
            {
                TargetTMPText.maxVisibleCharacters = visibleCharacters;
                visibleCharacters++;

                if (Timing.TimescaleMode == TimescaleModes.Scaled)
                {
                    yield return MMFeedbacksCoroutine.WaitFor(_delay);    
                }
                else
                {
                    yield return MMFeedbacksCoroutine.WaitForUnscaled(_delay);
                }
            }
        }

        /// <summary>
        /// Reveals lines one at a time
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator RevealLines()
        {
            int totalLines = TargetTMPText.textInfo.lineCount;
            int visibleLines = 0;

            while (visibleLines <= totalLines)
            {
                TargetTMPText.maxVisibleLines = visibleLines;
                visibleLines++;

                if (Timing.TimescaleMode == TimescaleModes.Scaled)
                {
                    yield return MMFeedbacksCoroutine.WaitFor(_delay);    
                }
                else
                {
                    yield return MMFeedbacksCoroutine.WaitForUnscaled(_delay);
                }
            }
        }

        /// <summary>
        /// Reveals words one at a time
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator RevealWords()
        {
            int totalWords = TargetTMPText.textInfo.wordCount;
            int visibleWords = 0;

            while (visibleWords <= totalWords)
            {
                TargetTMPText.maxVisibleWords = visibleWords;
                visibleWords++;

                if (Timing.TimescaleMode == TimescaleModes.Scaled)
                {
                    yield return MMFeedbacksCoroutine.WaitFor(_delay);    
                }
                else
                {
                    yield return MMFeedbacksCoroutine.WaitForUnscaled(_delay);
                }
            }
        }

        /// <summary>
        /// Stops the animation if needed
        /// </summary>
        /// <param name="position"></param>
        /// <param name="feedbacksIntensity"></param>
        protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            base.CustomStopFeedback(position, feedbacksIntensity);
            if (Active && (_coroutine != null))
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
    }
}
