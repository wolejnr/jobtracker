using JobTracker.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace JobTracker.Api.Services;
// Services are where we place the business logic
public static class JobService
{
    static List<Job> Jobs { get; }
    static int nextId = 5;

    static JobService()
    {
        Jobs = [
            new Job {Id = 1, Company = "Creative Trnd", DateApplied = new DateTime(2026, 1, 28), Role = "Web Developer", Location="Ottawa", Status = "Applied"},
            new Job {Id = 2, Company = "Eperformance Inc.", DateApplied = new DateTime(2026, 2, 28), Role = "Customer Service Specialist", Location="Ottawa", Status = "Interviewing"},
            new Job {Id = 3, Company = "Algonquin College", DateApplied = new DateTime(2025, 7, 22), Role = "Professor of ICT", Location="Ottawa", Status = "Offer", Notes = "Might accept"},
            new Job {Id = 4, Company = "Heritage College", DateApplied = new DateTime(2024, 7, 22), Role = "Professor of Computer Science", Location="Ottawa", Status = "Rejected"},
        ];
    }

    public static List<Job> GetAll() => Jobs;

    public static Job? Get(int id) => Jobs.FirstOrDefault(j => j.Id == id);

    // Method to add a new job to the list
    public static void Add(Job job)
    {
        job.Id = nextId++;
        Jobs.Add(job);
    } 

    // Method to delete
    public static void Delete(int id)
    {
        var job = Get(id);
        if(job is null)
            return;

        Jobs.Remove(job);
    }

    public static void Update(Job job)
    {
        var index = Jobs.FindIndex(p => p.Id == job.Id);
        if (index == -1)
            return;
        
        Jobs[index] = job;
    }
}