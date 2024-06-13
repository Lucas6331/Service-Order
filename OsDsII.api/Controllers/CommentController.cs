using Microsoft.AspNetCore.Mvc;
using OsDsII.api.Dtos.Comments;
using OsDsII.api.Exceptions;
using OsDsII.api.Services.Comments;

namespace OsDsII.api.Controllers
{
    [ApiController]
    [Route("ServiceOrders/{id}/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCommentsAsync(int id)
        {
            try
            {
                var serviceOrderDto = await _commentService.GetByIdAsync(id);

                return Ok(serviceOrderDto);
            }
            catch (BaseException ex)
            {
                return ex.GetResponse();
            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComment(int id, CreateCommentDto createCommentDto)
        {
            try
            {
                var commentDto = await _commentService.AddAsync(id, createCommentDto);

                return Created(nameof(CommentController), commentDto);
            }
            catch (BaseException ex)
            {
                return ex.GetResponse();
            }
        }
    }
}