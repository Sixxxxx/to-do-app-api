using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using ToDoList.Data.Interfaces;
using ToDoList.Models.Dtos.Requests;
using ToDoList.Models.Entities;
using ToDoList.Services.Interfaces;

namespace ToDoList.Services.Implementation
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;
        private readonly IRepository<ToDoItem> _toDoItemRepo;
        private readonly IMapper _mapper; 
        private readonly UserManager<ApplicationUser> _userManager;
        public ToDoItemService(IServiceFactory serviceFactory, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
            _mapper = _serviceFactory.GetService<IMapper>();
            _userManager = _serviceFactory.GetService<UserManager<ApplicationUser>>();
            _toDoItemRepo = _unitOfWork.GetRepository<ToDoItem>();            
        }

        public async Task<IEnumerable<ToDoItem>> GetAllToDoItems()
        {
            IEnumerable<ToDoItem> toDoItemList = await _toDoItemRepo.GetAllAsync();

            if (!toDoItemList.Any())
                throw new InvalidOperationException("To do list is empty");

            
            return toDoItemList;
        }

        public async Task<ToDoItem> GetToDoItem(int Id)
        {
            ToDoItem toDoItemList = await _toDoItemRepo.GetSingleByAsync(x => x.Id == Id);

            if (toDoItemList == null)
                throw new InvalidOperationException("no item with that Id");

            return toDoItemList;
        }



        public async Task CreateToDoItem(CreateToDoItemRequest request)
        {
            bool toDoItemExists = await _toDoItemRepo.AnyAsync(c =>
                c.IsComplete && c.Description.ToLower() == request.Description.ToLower());

            if (toDoItemExists)
                throw new InvalidOperationException("Item already exists");
            
            ToDoItem newToDoItem = _mapper.Map<ToDoItem>(request);
            
            await _toDoItemRepo.AddAsync(newToDoItem);
        }

        public async Task UpdateToDoItem(int Id, CreateToDoItemRequest request)
        {
            ToDoItem toDoItem = await _toDoItemRepo.GetSingleByAsync(c =>
                c.Id == Id);

            if (toDoItem == null)
                throw new InvalidOperationException("Item doesnt exist");

            ToDoItem newToDoItem = _mapper.Map(request, toDoItem);

            await _toDoItemRepo.UpdateAsync(newToDoItem);
        }

        public async Task DeleteToDoItem(int Id)
        {
            ToDoItem toDoItem = await _toDoItemRepo.GetSingleByAsync(c =>
                c.Id == Id);

            if (toDoItem == null)
                throw new InvalidOperationException("Item doesnt exist");

            await _toDoItemRepo.DeleteAsync(toDoItem);
        }

        public async Task ToggleToDoItem(int Id)
        {
            ToDoItem toDoItem = await _toDoItemRepo.GetSingleByAsync(c =>
                c.Id == Id);

            if (toDoItem == null)
                throw new InvalidOperationException("Item doesnt exist");
            toDoItem.IsComplete = !toDoItem.IsComplete;
            await _toDoItemRepo.UpdateAsync(toDoItem);
        }

        public async Task PatchToDoItem(int Id, JsonPatchDocument<CreateToDoItemRequest> request)
        {
            ToDoItem toDoItem = await GetToDoItem(Id);

            if (toDoItem == null)
                throw new InvalidOperationException("No to do item found");

            CreateToDoItemRequest studentDataToUpdate = _mapper.Map<CreateToDoItemRequest>(toDoItem);

            request.ApplyTo(studentDataToUpdate);

            ToDoItem UpdatedToDoItem = _mapper.Map(studentDataToUpdate, toDoItem);

            await  _toDoItemRepo.UpdateAsync(UpdatedToDoItem);
        }
    }
}
