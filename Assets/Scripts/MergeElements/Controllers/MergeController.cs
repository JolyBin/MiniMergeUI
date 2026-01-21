using System.Linq;
using System.Xml.Linq;
using Core.MergeElements.Models;
using Core.MergeElements.Views;
using DG.Tweening;
using UnityEngine;

namespace Core.MergeElements.Controllers
{
    /// <summary>
    /// Контроллер перемещения, спавна и мерджа элементов
    /// </summary>
    public class MergeController
    {
        private MergeElementSettings _firstElement;
        private MergeCell[] _elementsGrid;

        //UI
        private MergeWindowUI _mergeWindowUI;
        private MergeElementUI _mergewElementUIPrefab;
        private MergeElementUI _tempElementUI;

        private Vector2 _activeElementPointerOffset;
        private MergeCell _selectbleCalle;


        public MergeController(int maxElementsCount, MergeElementSettings firstElement, MergeElementUI mergewElementUIPrefab, MergeWindowUI mergeWindowUI)
        {
            _firstElement = firstElement;
            _elementsGrid = new MergeCell[maxElementsCount];

            _mergeWindowUI = mergeWindowUI;
            _mergewElementUIPrefab = mergewElementUIPrefab;

            Init();
        }

        /// <summary>
        /// Создание моделей и вьюх
        /// </summary>
        public void Init()
        {
            for (int i = 0; i < _elementsGrid.Length; i++)
            {
                MergeCell cell = new(GameObject.Instantiate(_mergewElementUIPrefab));

                _mergeWindowUI.AddElement(cell.View.RectTransform, i);
                _elementsGrid[i] = cell;
            }

            MergeElementUI lastElem = _elementsGrid[_elementsGrid.Length - 1].View;
            _tempElementUI = GameObject.Instantiate(_mergewElementUIPrefab);
            _tempElementUI.RectTransform.SetParent(lastElem.RectTransform.parent);
            _tempElementUI.RectTransform.offsetMin = Vector2.zero;
            _tempElementUI.RectTransform.offsetMax = Vector2.zero;
            _tempElementUI.gameObject.SetActive(false);
        }

        /// <summary>
        /// Отображение окна и подписка на UI события
        /// </summary>
        public void ShowWindow()
        {
            _mergeWindowUI.Show();
            Reset();
            _mergeWindowUI.OnClickSpawnButton += TryAddElement;
            _mergeWindowUI.OnClickResetButton += Reset;
            _mergeWindowUI.OnPointerDown += HandlePointerDown;
            _mergeWindowUI.OnPointerDrag += HandlePointerDrag;
            _mergeWindowUI.OnPointerUp += HandlePointerUp;
        }

        /// <summary>
        /// Скрытие окна и очистка всех событий
        /// </summary>
        public void HideWindo()
        {
            _mergeWindowUI.Hide();
            Reset();
        }

        /// <summary>
        /// Очистка поля
        /// </summary>
        private void Reset()
        {
            ResetSelectbleElement();
            foreach (MergeCell cell in _elementsGrid)
            {
                cell.Model = null;
                cell.View.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Пробуем добавить новый элемент на сетку
        /// </summary>
        private void TryAddElement()
        {            
            var emptyElements = _elementsGrid
                .Select((value, index) => new { value, index })
                .Where(x => x.value.Model == null)
                .Select(x => x.index).ToArray();
            
            if (emptyElements.Length == 0)
                return;

            int randomindex = emptyElements[Random.Range(0, emptyElements.Length)];
            MergeElement newElement = _firstElement.GetMergeElement();

            var cell = _elementsGrid[randomindex];
            cell.Model = newElement;
            cell.View.SetVisualParams(newElement.Icon);
            cell.View.gameObject.SetActive(true);
        }

        /// <summary>
        /// Ищем на какой элемент попала мышка
        /// </summary>
        /// <param name="pointerPos">позиция мыши</param>
        /// <param name="mergeCell">возвращаем найденную ячейку</param>
        /// <returns></returns>
        private bool TryGetCellOnPosition(Vector2 pointerPos, out MergeCell mergeCell)
        {
            mergeCell = null;
            foreach (var element in _elementsGrid)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(element.View.RectTransform, Input.mousePosition, _mergeWindowUI.WorldCamera))
                {
                    mergeCell = element;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Получаем координаты нажатия мыши и запоминаем выбранный элемент. Визуально отображаться будет временный UI элемент
        /// </summary>
        /// <param name="pointerPos">Координата клика</param>
        private void HandlePointerDown(Vector2 pointerPos)
        {
            ResetSelectbleElement();

            if (TryGetCellOnPosition(pointerPos, out MergeCell cell) && cell.Model != null)
            {
                _selectbleCalle = cell;
                _tempElementUI.transform.position = _selectbleCalle.View.transform.position;
                _tempElementUI.SetVisualParams(_selectbleCalle.Model.Icon);
                _selectbleCalle.View.gameObject.SetActive(false);
                _tempElementUI.gameObject.SetActive(true);


                _activeElementPointerOffset = _tempElementUI.RectTransform.anchoredPosition - pointerPos;
            }
        }

        /// <summary>
        /// Перемещаем временный UI элемент
        /// </summary>
        /// <param name="pointerPos">Координата текущего положения мыши</param>
        private void HandlePointerDrag(Vector2 pointerPos)
        {
            if (_selectbleCalle == null) return;
            _tempElementUI.RectTransform.anchoredPosition = pointerPos + _activeElementPointerOffset;
        }

        /// <summary>
        /// Получаем координаты мыши при отпускании UI элемента. Проверяем мёрджи и прочее, затем выключаем временный UI элемент
        /// </summary>
        /// <param name="pointerPos">Координата клика</param>
        private void HandlePointerUp(Vector2 pointerPos)
        {
            if (_selectbleCalle == null) return;

            if(TryGetCellOnPosition(pointerPos, out MergeCell cell))
            {
                if(cell.Model == null)
                {
                    cell.Model = _selectbleCalle.Model;
                    _selectbleCalle.Model = null;
                    _selectbleCalle = null;
                    _tempElementUI.gameObject.SetActive(false);
                    return;
                }
                if (cell != _selectbleCalle && cell.Model.ID == _selectbleCalle.Model.ID && cell.Model.NextMergeElement != null)
                {
                    _selectbleCalle.Model = null;
                    _selectbleCalle = null;
                    _tempElementUI.gameObject.SetActive(false);

                    cell.Model = cell.Model.NextMergeElement;
                    cell.View.RectTransform.DOScale(1.2f, 0.1f)
                        .SetLoops(2, LoopType.Yoyo);
                }
            }
            ResetSelectbleElement();
        }

        /// <summary>
        /// Сброс выбранного элемента дял исключения багов с быстрыми кликами
        /// </summary>
        private void ResetSelectbleElement()
        {
            _tempElementUI.gameObject.SetActive(false);
            if (_selectbleCalle != null)
            {
                _selectbleCalle.View.gameObject.SetActive(true);
                _selectbleCalle = null;
            }
            
        }
    }
}
