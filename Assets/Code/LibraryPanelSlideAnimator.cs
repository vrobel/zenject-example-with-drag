using System;
using DG.Tweening;
using UnityEngine;
using Zenject;
using R3;

namespace Code
{
    public class LibraryPanelSlideAnimator : MonoBehaviour, IInitializable, IDisposable
    {
        DisposableBag _disposable;

        [SerializeField] private RectTransform panel;
        [Range(0f, 1f), SerializeField] private float duration = 0.5f;

        private bool _isHidden;

        private bool IsHidden
        {
            get => _isHidden;
            set
            {
                if (_isHidden == value)
                {
                    return;
                }

                _isHidden = value;

                if (_isHidden)
                {
                    panel.DOAnchorPosX(_panelAnchoredPosition.x - panel.sizeDelta.x, duration,
                        true);
                }
                else
                {
                    panel.DOAnchorPosX(_panelAnchoredPosition.x, duration, true);
                }
            }
        }

        private IObservablePlacementSystem _observablePlacementSystem;
        private Vector2 _panelAnchoredPosition;

        [Inject]
        private void Construct(IObservablePlacementSystem observablePlacementSystem)
        {
            _observablePlacementSystem = observablePlacementSystem;
        }

        public void Initialize()
        {
            _panelAnchoredPosition = panel.anchoredPosition;
            _observablePlacementSystem.DraggedObject.Subscribe(OnDragging)
                .AddTo(ref _disposable);
        }

        private void OnDragging(SceneItem obj)
        {
            IsHidden = obj != null;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}