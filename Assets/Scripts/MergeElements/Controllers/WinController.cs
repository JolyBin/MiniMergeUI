using System;
using System.Linq;
using System.Xml.Linq;
using Core.MergeElements.Models;
using Core.MergeElements.Views;
using DG.Tweening;
using UnityEngine;

namespace Core.MergeElements.Controllers
{
    /// <summary>
    /// Контроллер окна победы
    /// </summary>
    public class WinController
    {
        public event Action OnCloseWindow;
        //UI
        private WinWindowUI _winWindowUI;


        public WinController(WinWindowUI winWindowUI)
        {
            _winWindowUI = winWindowUI;
        }


        /// <summary>
        /// Отображение окна и подписка на UI события
        /// </summary>
        public void ShowWindow()
        {
            _winWindowUI.Show();
            _winWindowUI.OnClickRestartButton += () => OnCloseWindow?.Invoke();
        }

        /// <summary>
        /// Скрытие окна и очистка всех событий
        /// </summary>
        public void HideWindow()
        {
            _winWindowUI.Hide();
            OnCloseWindow = null;
        }
    }
}
