using System;
using System.Collections.Generic;

namespace Data.Model
{
    public enum ProjectStatus
    {
        Draft,
        Sent,
        Cancelled,
        Finished
    }

    public class Project
    {
        public int Id { get; set; }
        public ProjectStatus Status { get; set; }
        public string Name { get; set; }
        public string Commentary { get; set; }

        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; }

        public Automobile Automobile { get; set; }
        public Color Color { get; set; }

        public List<Modification> Modifications { get; set; }
    }

    public class ProjectDto
    {
        public int AutomobileId { get; set; }
        public int ColorId { get; set; }
        public string Name { get; set; }
        public string Commentary { get; set; }

        public List<Modification> Modifications { get; set; }
    }
}
