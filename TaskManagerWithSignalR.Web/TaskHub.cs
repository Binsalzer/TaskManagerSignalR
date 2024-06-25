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
            var repo = new TaskManagerRepository(_connection);
            Clients.All.SendAsync("taskAdded", repo.GetNewestTask());
        }

        public void Delete(int id)
        {
            Clients.All.SendAsync("taskDeleted", id);
        }

        public void Select()
        {
            var repo = new TaskManagerRepository(_connection);
            var usersRepo = new UsersRepository(_connection);
            var tasks = repo.GetAllTasks();

            foreach (var t in tasks)
            {
                if (t.UserId.HasValue)
                {
                    t.UserName = usersRepo.GetUserForId(t.UserId.Value).Name;
                }
            }

            Clients.All.SendAsync("taskSelected", tasks);
        }
    }
}
