using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerWithSignalR.Data;
using TaskManagerWithSignalR.Web.Models;

namespace TaskManagerWithSignalR.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly string _connection;

        public TasksController(IConfiguration config)
        {
            _connection = config.GetConnectionString("ConStr");
        }

        [HttpPost("addtask")]
        public void AddTask(AddTaskVM vm)
        {
            var repo = new TaskManagerRepository(_connection);
            repo.AddTask(vm.Description);
        }

        [HttpGet("getalltasks")]
        public List<TaskItem> GetAllTasks()
        {
            var repo = new TaskManagerRepository(_connection);
            var usersRepo = new UsersRepository(_connection);
            var tasks=repo.GetAllTasks();
            foreach (TaskItem t in tasks)
            {
                if (t.UserId.HasValue)
                {
                    t.UserName = usersRepo.GetUserForId(t.UserId.Value).Name;
                }
            }
            return tasks;
        }

        [HttpPost("delete")]
        public void Delete(DeleteVM vm)
        {
            var repo = new TaskManagerRepository(_connection);
            repo.Delete(vm.Id);
        }

        [HttpPost("Select")]
        public void Select(SelectVM vm)
        {
            var repo = new TaskManagerRepository(_connection);
            var userRepo = new UsersRepository(_connection);

            repo.SelectTask(vm.id, userRepo.GetByEmail(User.Identity.Name).Id);
        }

    }
}
