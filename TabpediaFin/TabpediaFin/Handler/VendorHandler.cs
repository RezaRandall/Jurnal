//namespace TabpediaFin.Handler
//{
//    public class GetListVendorHandler : IRequestHandler<GetVendorListQuery, List<Contact>>
//    {
//        private readonly IVendorRepository _VendorRepository;
//        public GetListVendorHandler(IVendorRepository VendorRepository)
//        {
//            _VendorRepository = VendorRepository;
//        }

//        public async Task<List<Contact>> Handle(GetVendorListQuery request, CancellationToken cancellationToken)
//        {
//            var result = await _VendorRepository.GetVendorList(request);
//            return result;
//        }
//    }

//    public class GetVendorHandler : IRequestHandler<GetVendorQuery, Contact>
//    {
//        private readonly IVendorRepository _VendorRepository;
//        public GetVendorHandler(IVendorRepository VendorRepository)
//        {
//            _VendorRepository = VendorRepository;
//        }

//        public async Task<Contact> Handle(GetVendorQuery request, CancellationToken cancellationToken)
//        {
//            var result = await _VendorRepository.GetVendor(request);
//            return result;
//        }
//    }

//    public class AddVendorHandler : IRequestHandler<AddVendor, Contact>
//    {
//        private readonly IVendorRepository _VendorRepository;
//        public AddVendorHandler(IVendorRepository VendorRepository)
//        {
//            _VendorRepository = VendorRepository;
//        }

//        async Task<Contact> IRequestHandler<AddVendor, Contact>.Handle(AddVendor request, CancellationToken cancellationToken)
//        {
//            var result = await _VendorRepository.CreateVendor(request);
//            return result;
//        }
//    }
//    public class UpdateVendorHandler : IRequestHandler<UpdateVendor, Contact>
//    {
//        private readonly IVendorRepository _VendorRepository;
//        public UpdateVendorHandler(IVendorRepository VendorRepository)
//        {
//            _VendorRepository = VendorRepository;
//        }

//        async Task<Contact> IRequestHandler<UpdateVendor, Contact>.Handle(UpdateVendor request, CancellationToken cancellationToken)
//        {
//            var result = await _VendorRepository.UpdateVendor(request);
//            return result;
//        }
//    }
//}
