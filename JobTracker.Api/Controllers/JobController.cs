using JobTracker.Api.Models;
using JobTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Controllers;

// Controllers handle incoming HTTP requests and respond with 
// HTTP responses like 200, 404, 500

[ApiController] // enables opinionated behaviours
[Route("[controller]")]
public class JobController : ControllerBase
{
    private readonly JobDbContext _jobDbContext;

    public JobController(JobDbContext jobDbContext)
    {
        this._jobDbContext = jobDbContext;
    }
    // GET: /jobs
    [HttpGet]
    [Route("GetJob")]
    // public ActionResult<List<Job>> GetAll() => JobService.GetAll();
    public async Task<IEnumerable<Job>> GetJobs()
    {
        return await _jobDbContext.Jobs.ToListAsync();
    }

    // GET: /jobs/2
    [HttpGet]
    [Route("{id}")]
    public ActionResult<Job> Get(int id)
    {
        var job = JobService.Get(id);
        if(job == null) 
            return NotFound();
        return job;
    }

    [HttpPost("AddJob")]
    public async Task<ActionResult<Job>> AddJob([FromBody] Job job)
    {
        _jobDbContext.Jobs.Add(job);
        await _jobDbContext.SaveChangesAsync();
        return Ok(job);
    }
    // public IActionResult Create([FromBody] Job job)
    // {
    //     // 201 status code indicates that a new resource was created
    //     JobService.Add(job);
    //     return CreatedAtAction(nameof(Get), new {id = job.Id}, job);
    // }

    [HttpDelete]
    [Route("DeleteJob/{id}")]
    public bool DeleteJob(int id)
    {
        bool flag = false;
        var job = _jobDbContext.Jobs.Find(id);
        if(job != null)
        {
            flag = true;
            _jobDbContext.Entry(job).State = EntityState.Deleted;
            _jobDbContext.SaveChanges();
        }

        return flag;
    }
    // public IActionResult Delete(int id)
    // {
    //     var job = JobService.Get(id);

    //     if(job is null)
    //         return NotFound();
        
    //     JobService.Delete(id);

    //     return NoContent();
    // }

    [HttpPatch]
    [Route("UpdateJob/{id}")]
    public async Task<Job> UpdateJob(Job job)
    {
        _jobDbContext.Entry(job).State = EntityState.Modified;
        await _jobDbContext.SaveChangesAsync();
        return job;
    }
    // public IActionResult Update(int id, Job job)
    // {
    //     if(id != job.Id)
    //         return BadRequest();
    //     var existingJob = JobService.Get(id);
    //     if(existingJob is null)
    //         return NotFound();
    //     JobService.Update(job);
    //     return NoContent();
    // }

}

