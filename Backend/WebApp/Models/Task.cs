using System;

namespace WebApp.Models
{
    public class TaskModel
    {
        public string Title { get; set; }
        public string Assignee { get; set; }
        public DateTime DueDate { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Assignor { get; set; }
        public int UploadedDocs { get; set; }
    }
}