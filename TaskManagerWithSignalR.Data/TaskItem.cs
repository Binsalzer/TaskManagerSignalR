using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskManagerWithSignalR.Data
{
    public class TaskItem
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [NotMapped]
        public string UserName { get; set; }
    }
}
