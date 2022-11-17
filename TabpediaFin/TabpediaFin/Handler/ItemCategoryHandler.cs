namespace TabpediaFin.Handler
{
    public class GetListItemCategoryHandler : IRequestHandler<GetItemCategoryListQuery, List<ItemCategory>>
    {
        private readonly IItemCategoryRepository _itemCategoryRepository;
        public GetListItemCategoryHandler(IItemCategoryRepository itemCategoryRepository)
        {
            _itemCategoryRepository = itemCategoryRepository;
        }

        public async Task<List<ItemCategory>> Handle(GetItemCategoryListQuery request, CancellationToken cancellationToken)
        {
            var result = await _itemCategoryRepository.GetItemCategoryList(request);
            return result;
        }
    }

    public class GetItemCategoryHandler : IRequestHandler<GetItemCategoryQuery, ItemCategory>
    {
        private readonly IItemCategoryRepository _itemCategoryRepository;
        public GetItemCategoryHandler(IItemCategoryRepository itemCategoryRepository)
        {
            _itemCategoryRepository = itemCategoryRepository;
        }

        public async Task<ItemCategory> Handle(GetItemCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = await _itemCategoryRepository.GetItemCategory(request);
            return result;
        }
    }

    public class DeleteItemCategoryHandler : IRequestHandler<DeleteItemCategory, bool>
    {
        private readonly IItemCategoryRepository _itemCategoryRepository;
        public DeleteItemCategoryHandler(IItemCategoryRepository itemCategoryRepository)
        {
            _itemCategoryRepository = itemCategoryRepository;
        }

        public async Task<bool> Handle(DeleteItemCategory request, CancellationToken cancellationToken)
        {
            var result = await _itemCategoryRepository.DeleteItemCategory(request);
            return result;
        }
    }
    public class AddItemCategoryHandler : IRequestHandler<AddItemCategory, ItemCategory>
    {
        private readonly IItemCategoryRepository _itemCategoryRepository;
        public AddItemCategoryHandler(IItemCategoryRepository itemCategoryRepository)
        {
            _itemCategoryRepository = itemCategoryRepository;
        }

        async Task<ItemCategory> IRequestHandler<AddItemCategory, ItemCategory>.Handle(AddItemCategory request, CancellationToken cancellationToken)
        {
            var result = await _itemCategoryRepository.CreateItemCategory(request);
            return result;
        }
    }
    public class UpdateItemCategoryHandler : IRequestHandler<UpdateItemCategory, ItemCategory>
    {
        private readonly IItemCategoryRepository _itemCategoryRepository;
        public UpdateItemCategoryHandler(IItemCategoryRepository itemCategoryRepository)
        {
            _itemCategoryRepository = itemCategoryRepository;
        }

        async Task<ItemCategory> IRequestHandler<UpdateItemCategory, ItemCategory>.Handle(UpdateItemCategory request, CancellationToken cancellationToken)
        {
            var result = await _itemCategoryRepository.UpdateItemCategory(request);
            return result;
        }
    }
}
