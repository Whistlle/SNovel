﻿using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using SNovel.MessageNotificationCenter;

namespace SNovel
{
    //用于播放文字动画
    public class TextEffect : MonoBehaviour
    {
        public Text _textUI;
        [Range(1, 10)] public float Speed = 6; 
        enum RenderState
        {
            Rendering,
            Waiting
        }

        RenderState _renderState = RenderState.Waiting;

        public bool IsRendering()
        {
            return _renderState == RenderState.Rendering;
        }

        public bool IsWaiting()
        {
            return _renderState == RenderState.Waiting;
        }

        Tweener _animation;
        Sequence _sequense;

        public void StartEffect(string fromText, string toText, Action onFinish)
        {
            _onFinish = onFinish;
            _renderState = RenderState.Rendering;
            _sequense = null;
            _sequense = DOTween.Sequence();
            _textUI.text = fromText;
            _animation = _textUI.DOText(toText, Speed* Mathf.Abs(toText.Length-fromText.Length)/100);
            _sequense.Append(_animation);
            _sequense.AppendCallback(FinishDisplay); 
        }
        public void StartEffect(string text, Action onFinish)
        {
            _renderState = RenderState.Rendering;
            _sequense = null;
            _sequense = DOTween.Sequence();
            _animation = _textUI.DOText(text, Speed * text.Length / 100);
            _sequense.Append(_animation);
            _sequense.AppendCallback(FinishDisplay);              
        }

        private Action _onFinish;
        public void DisplayTextRemain()
        {
            _sequense.Complete();
            FinishDisplay();
        }

        public void DisplayAddText(string text, float duration)
        {
            _renderState = RenderState.Rendering;
            _textUI.text += "\n";
            _animation = _textUI.DOText(text + _textUI.text, duration).ChangeStartValue(_textUI.text);
        }

        
        // Use this for initialization
        void Start()
        {
            _renderState = RenderState.Waiting;
        }

        // Update is called once per frame
        void Update()
        {
            /*
            //check if the animation is done
            if(_renderState == RenderState.Rendering)
            {
                if(_animation.IsComplete())
                {
                    FinishDisplay();
                }
            }*/
        }

        void FinishDisplay()
        {
            _renderState = RenderState.Waiting;
        //    MessageDispatcher.Instance.DispatchMessage(
       //         new Message("EVENT_SCRIPT_COMMAND_FINISH"));
            _onFinish();
        }
    }
}