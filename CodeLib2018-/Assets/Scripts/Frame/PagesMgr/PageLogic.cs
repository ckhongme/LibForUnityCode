using System.Collections.Generic;

namespace K
{
    public class PageLogic
    {
        private int curPageIndex = 1;
        /// <summary>
        /// 当前页面的序号
        /// </summary>
        public int CurPageIndex { get { return curPageIndex; } }

        private int totalPageNum = 1;
        /// <summary>
        /// 页面总数
        /// </summary>
        public int TotalPage { get { return totalPageNum; } }

        /// <summary>
        /// 设置一个页面元素的行数和列数
        /// </summary>
        /// <param name="row">行数</param>
        /// <param name="column">列数</param>
        public void SetOnePageItemInfo(int row, int column)
        {
            onePageItemNum = row * column;
        }

        /// <summary>
        /// 设置元素数量
        /// </summary>
        public void SetItemMaxNum(int num)
        {
            if (num == 0)
            {
                allItemNum = 0;
                totalPageNum = 1;
                return;
            }

            if (allItemNum == num) return;
            allItemNum = num;

            totalPageNum = allItemNum / onePageItemNum;
            totalPageNum = allItemNum % onePageItemNum == 0 ? totalPageNum : totalPageNum + 1;

            if (curPageIndex > totalPageNum) curPageIndex = totalPageNum;
        }

        public void Reset()
        {
            curPageIndex = 1;
        }

        #region 翻页

        public bool HasLastPage { get { return curPageIndex > 1; } }             //是否有上一页
        public bool HasNextPage { get { return curPageIndex < totalPageNum; } }         //是否有下一页

        /// <summary>
        /// 上一页
        /// </summary>
        public void LastPage()
        {
            if (curPageIndex > 1)
            {
                curPageIndex--;
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public void NextPage()
        {
            if (curPageIndex < totalPageNum)
            {
                curPageIndex++;
            }
        }

        /// <summary>
        /// 跳页
        /// </summary>
        public void TurnPage(int pageIndex)
        {
            if (pageIndex >= 1 && pageIndex <= totalPageNum)
            {
                curPageIndex = pageIndex;
            }
        }

        #endregion

        #region Page with Items

        private int onePageItemNum = 1;
        /// <summary>
        /// 单页元素的数量
        /// </summary>
        public int OnePageItemNum { get { return onePageItemNum; } }

        private int allItemNum;           //元素的总量

        /// <summary>
        /// 当前页面 的 元素序号
        /// </summary>
        /// <returns></returns>
        public int[] GetIndexsOfCurrPageItem()
        {
            return GetIndexsOfOnePageItem(curPageIndex);
        }

        /// <summary>
        /// 某一页 的 元素序号
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public int[] GetIndexsOfOnePageItem(int index)
        {
            if (index <= 0 || index > totalPageNum) return null;

            var startIndex = onePageItemNum * (index - 1) + 1;
            var endIndex = index == totalPageNum ? allItemNum : onePageItemNum * index;

            var list = new List<int>();
            for (int i = startIndex; i <= endIndex; i++)
            {
                list.Add(i);
            }
            return list.ToArray();
        }

        #endregion
    }
}