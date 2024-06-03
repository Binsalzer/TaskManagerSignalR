using Microsoft.AspNetCore.SignalR;
using TaskManagerWithSignalR.Data;
using TaskManagerWithSignalR.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagerWithSignalR.Web
{
    public class TaskHub : Hub
    {
        private readonly string _connection;

        public TaskHub(IConfiguration config)
        {
            _connection = config.GetConnectionString("ConStr");
        }

        public void AddTask(AddTaskVM vm)
        {
            Clients.All.SendAsync("taskAdded", new TaskItem { UserId = null, Description = vm.Description });
        }

        public void Delete(int id)
        {
            Clients.All.SendAsync("taskDeleted", id);
        }

        public void Select()
        {
            var repo=new TaskManagerRepository(_connection);
            Clients.All.SendAsync("taskSelected", repo.GetAllTasks());
        }
    }
}
