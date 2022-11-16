namespace TabpediaFin.Handler
{
    public class GetListCustomerHandler : IRequestHandler<GetCustomerListQuery, List<Contact>>
    {
        private readonly ICustomerRepository _customerRepository;
        public GetListCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<Contact>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.GetCustomerList(request);
            return result;
        }
    }

    public class GetCustomerHandler : IRequestHandler<GetCustomerQuery, Contact>
    {
        private readonly ICustomerRepository _customerRepository;
        public GetCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Contact> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.GetCustomer(request);
            return result;
        }
    }

    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomer, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        public DeleteCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> Handle(DeleteCustomer request, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.DeleteCustomer(request);
            return result;
        }
    }
    public class AddCustomerHandler : IRequestHandler<AddCustomer, Contact>
    {
        private readonly ICustomerRepository _customerRepository;
        public AddCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        async Task<Contact> IRequestHandler<AddCustomer, Contact>.Handle(AddCustomer request, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.CreateCustomer(request);
            return result;
        }
    }
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomer, Contact>
    {
        private readonly ICustomerRepository _customerRepository;
        public UpdateCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        async Task<Contact> IRequestHandler<UpdateCustomer, Contact>.Handle(UpdateCustomer request, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.UpdateCustomer(request);
            return result;
        }
    }
}