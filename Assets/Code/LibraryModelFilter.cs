using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Code
{
    public class LibraryModelFilter : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        private LibraryModel _libraryModel;

        private void Start()
        {
            inputField.onSubmit.AddListener(OnSubmit);
            inputField.onEndEdit.AddListener(OnDeselect);
            inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDestroy()
        {
            inputField.onSubmit.RemoveListener(OnSubmit);
            inputField.onEndEdit.RemoveListener(OnDeselect);
            inputField.onValueChanged.RemoveListener(OnValueChanged);
        }

        [Inject]
        private void Construct(LibraryModel libraryModel)
        {
            _libraryModel = libraryModel;
        }

        private void OnValueChanged(string text)
        {
            _libraryModel.SetFilter(text);
        }

        private void OnDeselect(string text)
        {
            if (inputField.wasCanceled)
            {
                inputField.text = string.Empty;
                _libraryModel.ResetFilter();
            }
        }

        public void OnSubmit(string text)
        {
            if (inputField.wasCanceled)
            {
                inputField.text = string.Empty;
                _libraryModel.ResetFilter();
            }
            else
            {
                _libraryModel.SetFilter(inputField.text);
            }
        }
    }
}