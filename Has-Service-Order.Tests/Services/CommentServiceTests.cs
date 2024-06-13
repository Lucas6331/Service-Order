using AutoMapper;
using Moq;
using OsDsII.api.Dtos.Comments;
using OsDsII.api.Dtos.ServiceOrders;
using OsDsII.api.Exceptions;
using OsDsII.api.Models;
using OsDsII.api.Repository.Comments;
using OsDsII.api.Repository.ServiceOrders;
using OsDsII.api.Services.Comments;

namespace Has_Service_Order.Tests.Services
{
    public class CommentServiceTests
    {
        private readonly Mock<ICommentsRepository> _mockCommentsRepository;
        private readonly Mock<IServiceOrderRepository> _mockSORepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CommentService _service;

        public CommentServiceTests()
        {
            _mockCommentsRepository = new Mock<ICommentsRepository>();
            _mockSORepository = new Mock<IServiceOrderRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new CommentService(_mockCommentsRepository.Object, _mockMapper.Object, _mockSORepository.Object);
        }

        [Fact]
        public async Task Should_Return_ServiceOrder_With_Comments_When_ServiceOrderExists()
        {
            int serviceOrderId = 1;
            ServiceOrder serviceOrder = new ServiceOrder
            {
                Id = serviceOrderId,
                Description = "Test 1",
                Price = 100.0,
                Status = StatusServiceOrder.OPEN,
                OpeningDate = DateTimeOffset.Now,
                FinishDate = DateTimeOffset.Now.AddDays(30),
                Comments = new List<Comment>
                {
                    new Comment { Id = 1, Description = "Comment AAA" },
                    new Comment { Id = 2, Description = "Comment Test" }
                }
            };

            _mockSORepository.Setup(r => r.GetServiceOrderWithCommentsAsync(serviceOrderId)).ReturnsAsync(serviceOrder);

            ServiceOrderDto serviceOrderDto = new ServiceOrderDto(
                serviceOrder.Description,
                serviceOrder.Price,
                serviceOrder.Status,
                serviceOrder.OpeningDate,
                serviceOrder.FinishDate,
                serviceOrder.Comments.Select(c => new CommentDto(c.Description, c.SendDate)).ToList());


            _mockMapper.Setup(mapper => mapper.Map<ServiceOrderDto>(serviceOrder)).Returns(serviceOrderDto);

            var result = await _service.GetByIdAsync(serviceOrderId);

            Assert.Equal(serviceOrderDto, result);
        }

        [Fact]
        public async Task Should_Add_New_Comment_In_A_Existing_ServiceOrder()
        {
            var serviceOrderId = 1;
            CreateCommentDto createCommentDto = new CreateCommentDto("Test Comment");
            ServiceOrder serviceOrder = new ServiceOrder
            {
                Id = serviceOrderId,
                Description = "First ServiceOrder",
                Price = 100.00,
                Status = StatusServiceOrder.OPEN,
                OpeningDate = DateTimeOffset.UtcNow,
                FinishDate = DateTimeOffset.UtcNow.AddDays(90)
            };

            Comment comment = new Comment
            {
                Description = createCommentDto.Description,
                ServiceOrderId = serviceOrderId
            };

            NewCommentDto newCommentDto = new NewCommentDto(comment.Id, comment.Description, comment.SendDate, comment.ServiceOrderId);

            _mockSORepository.Setup(r => r.GetServiceOrderFromCustomerAsync(serviceOrderId)).ReturnsAsync(serviceOrder);
            _mockCommentsRepository.Setup(r => r.AddCommentAsync(It.IsAny<Comment>()));
            _mockMapper.Setup(mapper => mapper.Map<NewCommentDto>(It.IsAny<Comment>())).Returns(newCommentDto);

            var result = await _service.AddAsync(serviceOrderId, createCommentDto);
            Assert.Equal(newCommentDto, result);
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_When_ServiceOrder_Not_Found()
        {
            var serviceOrderId = 1;
            CreateCommentDto createCommentDto = new CreateCommentDto("Test Comment");

            _mockSORepository.Setup(r => r.GetServiceOrderFromCustomerAsync(serviceOrderId)).ReturnsAsync((ServiceOrder)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.AddAsync(serviceOrderId, createCommentDto));
        }
    }
}
