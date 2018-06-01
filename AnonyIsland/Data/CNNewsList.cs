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
    class CnNewsList : ObservableCollection<CnNews>, ISupportIncrementalLoading
    {
        private bool _busy;
        private bool _hasMoreItems;
        private int _currentPage = 1;
        private readonly int _pageSize;
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
        public CnNewsList()
        {
            _pageSize = App.NewsCountOneTime;
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
            List<CnNews> list = null;
            try
            {
                DataLoading?.Invoke();
                list = await NewsService.GetRecentNewsAsync(_currentPage, _pageSize);
            }
            catch (Exception)
            {
                HasMoreItems = false;
            }

            if (list != null && list.Any())
            {
                actualCount = list.Count;
                TotalCount += actualCount;
                _currentPage++;
                HasMoreItems = true;
                list.ForEach(c => { Add(c); });
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