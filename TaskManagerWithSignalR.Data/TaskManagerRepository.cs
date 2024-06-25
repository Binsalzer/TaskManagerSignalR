using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerWithSignalR.Data
{
    public class TaskManagerRepository
    {
        private readonly string _connection;

        public TaskManagerRepository(string connection)
        {
            _connection = connection;
        }

        public void AddTask(string description)
        {
            using var context = new TaskManagerDataContext(_connection);
            context.Add(new TaskItem { UserId = null, Description = description });
            context.SaveChanges();
        }

        public List<TaskItem> GetAllTasks()
        {
            using var context = new TaskManagerDataContext(_connection);
            return context.Tasks.ToList();
        }

        public void Delete(int id)
        {
            using var context = new TaskManagerDataContext(_connection);
            context.Database.ExecuteSqlInterpolated($"DELETE FROM Tasks WHERE Id = {id}");
        }

        public void SelectTask(int id, int userId)
        {
            using var context = new TaskManagerDataContext(_connection);
            var task = context.Tasks.FirstOrDefault(t => t.Id == id);
            task.UserId = userId;
            context.SaveChanges();
        }

        public TaskItem GetNewestTask()
        {
            using var context = new TaskManagerDataContext(_connection);
            return context.Tasks.OrderByDescending(t => t.Id).ToList()[0];
        }
    }
}
