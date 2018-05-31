using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using AnonyIsland.HTTP;
using AnonyIsland.Models;

namespace AnonyIsland.Data
{
    class Cn48TopViewList : ObservableCollection<CnBlog>, ISupportIncrementalLoading
    {
        private bool _busy;
        private bool _hasMoreItems;
        private int _currentPage = 1;
        public event DataLoadingEventHandler DataLoading;
        public event DataLoadedEventHandler DataLoaded;

        public int TotalCount
        {
            get; set;
        }

        public bool HasMoreItems
        {
            get
            {
                if (_busy)
                {
                    return false;
                }

                return _hasMoreItems;
            }
            private set => _hasMoreItems = value;
        }

        public Cn48TopViewList()
        {
            HasMoreItems = true;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return InnerLoadMoreItemsAsync(count).AsAsyncOperation();
        }

        private async Task<LoadMoreItemsResult> InnerLoadMoreItemsAsync(uint expectedCount)
        {
            _busy = true;
            var actualCount = 0;
            List<CnBlog> list = null;
            try
            {
                DataLoading?.Invoke();
                list = await BlogService.Get48TopViewsAysnc(_currentPage * 20);
            }
            catch (Exception)
            {
                HasMoreItems = false;
            }

            if (list != null && list.Any() && list.Count > TotalCount)
            {
                int index = Count;
                if (index >= 0)
                {
                    actualCount = list.Count - index;
                    for (int i = index; i < list.Count; ++i)
                    {
                        Add(list[i]);
                    }
                    TotalCount += actualCount;
                    _currentPage++;
                    HasMoreItems = true;
                }
            }
            else
            {
                HasMoreItems = false;
            }
            DataLoaded?.Invoke();
            _busy = false;
            return new LoadMoreItemsResult
            {
                Count = (uint)actualCount
            };
        }
    }
}
