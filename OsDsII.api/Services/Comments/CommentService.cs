using AutoMapper;
using OsDsII.api.Dtos.Comments;
using OsDsII.api.Dtos.ServiceOrders;
using OsDsII.api.Exceptions;
using OsDsII.api.Models;
using OsDsII.api.Repository.Comments;
using OsDsII.api.Repository.ServiceOrders;

namespace OsDsII.api.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentsRepository commentsRepository, IMapper mapper, IServiceOrderRepository serviceOrderRepository)
        {
            _commentsRepository = commentsRepository;
            _mapper = mapper;
            _serviceOrderRepository = serviceOrderRepository;
        }

        public async Task<ServiceOrderDto> GetByIdAsync(int serviceOrderId)
        {
            var serviceOrderWithComments = await _serviceOrderRepository.GetServiceOrderWithCommentsAsync(serviceOrderId);

            return _mapper.Map<ServiceOrderDto>(serviceOrderWithComments);
        }

        public async Task<NewCommentDto> AddAsync(int serviceOrderId, CreateCommentDto createCommentDto)
        {
            var serviceOrder = await _serviceOrderRepository.GetServiceOrderFromCustomerAsync(serviceOrderId);

            if (serviceOrder == null)
            {
                throw new NotFoundException("ServiceOrder not found.");
            }

            var comment = new Comment
            {
                Description = createCommentDto.Description,
                ServiceOrderId = serviceOrderId
            };

            await _commentsRepository.AddCommentAsync(comment);

            return _mapper.Map<NewCommentDto>(comment);
        }
    }
}