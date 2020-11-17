using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace AIForAgedClient.ViewModel
{
    public class PaginatedListVM<T>:ViewModelBase
    {
        private int _pageIndex;
        public int PageIndex 
        {
            get => _pageIndex;
            set => Set(()=>PageIndex,ref _pageIndex,value);
        }

        private int _totalPages;
        public int TotalPages 
        {
            get => _totalPages;
            set => Set(()=>TotalPages,ref _totalPages,value);
        }

        private List<T> _items;
        public List<T> Items
        {
            get => _items;
            set => Set(()=>Items,ref _items,value);
        }

        private bool _hasPreviousPage;
        public bool HasPreviousPage
        {
            get => _hasPreviousPage;
            set => Set(()=>HasPreviousPage,ref _hasPreviousPage,value);
        }

        private bool _hasNextPage;
        public bool HasNextPage
        {
            get => _hasNextPage;
            set => Set(() => HasNextPage, ref _hasNextPage, value);
        }
    }
}
