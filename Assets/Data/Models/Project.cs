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

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Automobile Automobile { get; set; }
        public Color Color { get; set; }

        public List<Modification> Modifications { get; set; }
    }

    public class ProjectDto
    {
        public int Id { get; set; }
        public int AutomobileId { get; set; }
        public int ColorId { get; set; }
        public string Name { get; set; }
        public string Commentary { get; set; }

        public List<Modification> Modifications { get; set; }
    }

    public class ProjectListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Commentary { get; set; }

        public Automobile Automobile { get; set; }
        public string Color { get; set; }
        public ProjectStatus Status { get; set; }
        public int ModificationsCount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
