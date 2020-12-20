using System;
using System.Linq;
using Actor;
using GameWorkflow;
using TMPro;
using UnityEngine;

namespace Root
{
    public class MakeStatistic : MonoBehaviour
    {
        [SerializeField] private ActorColor statisticUnitsColor;
        [SerializeField] private GameObject templateLine;
        [SerializeField] private GameObject content;
        private GameController _controller;
        private float lineHeight;

        private void OnEnable()
        {
            _controller = FindObjectOfType<GameController>();
            var coloredPlayerModels = _controller.playerModels.Where(x => x.Color == statisticUnitsColor).ToList();
            lineHeight = templateLine.GetComponent<RectTransform>().rect.height;
            templateLine.gameObject.SetActive(false);
            content.GetComponent<RectTransform>().sizeDelta =
                new Vector2(0, (coloredPlayerModels.Count + 1) * lineHeight);
            for (int i = 0; i < coloredPlayerModels.Count; i++)
            {
                var a = Instantiate(templateLine, content.transform);
                var rectTrans = a.GetComponent<RectTransform>();
                rectTrans.anchoredPosition = new Vector2(0, -lineHeight * (i + 1));
                a.gameObject.SetActive(true);
                a.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = coloredPlayerModels[i].Name;
                var status = coloredPlayerModels[i].Health == 0 ? "Dead" : "Hurted";
                if (Math.Abs(coloredPlayerModels[i].Health - 100) < 0.1)
                    status = "Best";
                a.transform.Find("Status").GetComponent<TextMeshProUGUI>().text =
                    $"{status}: {coloredPlayerModels[i].Health}";
            }
        }
    }
}