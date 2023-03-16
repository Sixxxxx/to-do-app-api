using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDoList.Models.Dtos.Requests;
using ToDoList.Models.Dtos.Responses;
using ToDoList.Models.Entities;
using ToDoList.Services.Infrastructure;
using ToDoList.Services.Interfaces;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Authorization")]
    public class ToDoListController : ControllerBase
    {
        private readonly IToDoItemService _toDoService;
        public ToDoListController(IToDoItemService toDoService)
        {
            _toDoService = toDoService;
        }


        [HttpGet("get-all-to-do-items")]
        [SwaggerOperation(Summary = "Gets all items in the to do list")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Gets all items in the to do list", Type = typeof(SuccessResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetAllToDoItems()
        {
            IEnumerable<ToDoItem> response =await _toDoService.GetAllToDoItems();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("get-to-do-item")]
        [SwaggerOperation(Summary = "Gets item with id")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Gets item with id", Type = typeof(SuccessResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDoItem(int Id)
        {
            ToDoItem response = await _toDoService.GetToDoItem(Id);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("create-to-do-item")]
        [SwaggerOperation(Summary = "Creates a to do item")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Creates a to do item", Type = typeof(SuccessResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<ToDoItemResponse>> CreateToDoItem(CreateToDoItemRequest request)
        {
            await _toDoService.CreateToDoItem(request);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("update-to-do-item")]
        [SwaggerOperation(Summary = "Update a to do item")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Updates a to do item", Type = typeof(SuccessResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<ToDoItemResponse>> UpdateToDoItem(int Id, CreateToDoItemRequest request)
        {
            await _toDoService.UpdateToDoItem(Id,request);
            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("delete-to-do-item")]
        [SwaggerOperation(Summary = "Delete a to do item")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Deletes a to do item", Type = typeof(SuccessResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<ToDoItemResponse>> DeleteToDoItem(int Id)
        {
            await _toDoService.DeleteToDoItem(Id);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("toggle-to-do-item")]
        [SwaggerOperation(Summary = "toggles a to do item")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "toggles a to do item", Type = typeof(SuccessResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<ToDoItemResponse>> ToggleToDoItem(int Id)
        {
            await _toDoService.ToggleToDoItem(Id);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPatch("patch-to-do-list")]
        [SwaggerOperation(Summary = "Patches to do item")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Profile patched successfully", Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "No profile found for user", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to update profile", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateStudentProfile(int Id, JsonPatchDocument<CreateToDoItemRequest> request)
        {
            await _toDoService.PatchToDoItem(Id, request);
            return Ok();
        }

    }
}
