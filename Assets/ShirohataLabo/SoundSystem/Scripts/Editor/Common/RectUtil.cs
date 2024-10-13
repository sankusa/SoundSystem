using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoundSystem {
    public static class RectUtil {
        /// <summary>
        /// レイアウトタイプ
        /// </summary>
        public enum LayoutType {
            Expand = 0, // 拡大
            Fixed = 1, // 固定
        }
        
        /// <summary>
        /// 長さ指定情報
        /// </summary>
        public struct LayoutLength {
            public float Length { get; }
            public LayoutType LayoutType { get; }
            
            public LayoutLength(float length, LayoutType layoutType = LayoutType.Expand) {
                Length = length;
                LayoutType = layoutType;
            }
        }

        /// <summary>
        /// マージンをとる
        /// </summary>
        public static Rect Margin(
            Rect rect,
            float leftMargin = 0,
            float rightMargin = 0,
            float topMargin = 0,
            float bottomMargin = 0
        ) {
            return new Rect(
                rect.x + leftMargin,
                rect.y + topMargin,
                rect.width - leftMargin - rightMargin,
                rect.height - topMargin - bottomMargin
            );
        }

        /// <summary>
        /// Rectを分割(横方向)
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="widthList"></param>
        /// <param name="notExpandIndexList"></param>
        /// <param name="leftMargin"></param>
        /// <param name="rightMargin"></param>
        /// <returns></returns>
        public static Rect[] DivideRectHorizontal(
            Rect rect,
            IList<LayoutLength> widthList,
            float leftMargin = 0,
            float rightMargin = 0
        ) {
            // 固定幅/非固定幅それぞれの幅の合計
            float notExpandWidthTotal = widthList
                .Where(x => x.LayoutType == LayoutType.Fixed)
                .Select(x => x.Length)
                .Sum();
            float expandWidthTotal = widthList
                .Where(x => x.LayoutType == LayoutType.Expand)
                .Select(x => x.Length)
                .Sum();
            float expandSpace = rect.width - notExpandWidthTotal;

            // Rect生成
            float currentRectXMin = rect.x;
            Rect[] rects = new Rect[widthList.Count];
            for (int i = 0; i < widthList.Count; i++) {
                if (widthList[i].LayoutType == LayoutType.Fixed) {
                    rects[i] = new Rect(currentRectXMin + leftMargin, rect.y, widthList[i].Length - leftMargin - rightMargin, rect.height);
                    currentRectXMin += widthList[i].Length;
                }
                else if (widthList[i].LayoutType == LayoutType.Expand) {
                    float fixedWidth = expandSpace * widthList[i].Length / expandWidthTotal;
                    rects[i] = new Rect(currentRectXMin + leftMargin, rect.y, fixedWidth - leftMargin - rightMargin, rect.height);
                    currentRectXMin += fixedWidth;
                }
            }
            return rects;
        }
        
        /// <summary>
        /// Rectを分割(縦方向)
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="heightList"></param>
        /// <param name="topMargin"></param>
        /// <param name="bottomMargin"></param>
        /// <returns></returns>
        public static Rect[] DivideRectVertical(
            Rect rect,
            IList<LayoutLength> heightList,
            float topMargin = 0,
            float bottomMargin = 0
        ) {
            // 固定幅/非固定幅それぞれの幅の合計
            float notExpandHeightTotal = heightList
                .Where(x => x.LayoutType == LayoutType.Fixed)
                .Select(x => x.Length)
                .Sum();
            float expandHeightTotal = heightList
                .Where(x => x.LayoutType == LayoutType.Expand)
                .Select(x => x.Length)
                .Sum();
            float expandSpace = rect.height - notExpandHeightTotal;

            // Rect生成
            float currentRectYMin = rect.y;
            Rect[] rects = new Rect[heightList.Count];
            for (int i = 0; i < heightList.Count; i++) {
                if (heightList[i].LayoutType == LayoutType.Fixed) {
                    rects[i] = new Rect(rect.x, currentRectYMin + topMargin, rect.width, heightList[i].Length - topMargin - bottomMargin);
                    currentRectYMin += heightList[i].Length;
                }
                else if (heightList[i].LayoutType == LayoutType.Expand) {
                    float fixedHeight = expandSpace * heightList[i].Length / expandHeightTotal;
                    rects[i] = new Rect(rect.x, currentRectYMin + topMargin, rect.width, fixedHeight - topMargin - bottomMargin);
                    currentRectYMin += fixedHeight;
                }
            }
            return rects;
        }
    }
}