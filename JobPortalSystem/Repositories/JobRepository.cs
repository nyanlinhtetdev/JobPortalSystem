using Dapper;
using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.DTOs.Job;
using JobPortalSystem.Api.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Api.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public JobRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task CreateJob(Job request)
        {
            await _context.Jobs.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<JobDto>> SearchJobs(JobSearchRequestDto request)
        {
            var connectionString = _configuration.GetConnectionString("DbConnection")!;
            await using var db = new SqlConnection(connectionString);

            var sql = @"SELECT * FROM Jobs WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                sql += " AND Title LIKE @Title";
                parameters.Add("Title", $"%{request.Title}%");
            }

            if (!string.IsNullOrWhiteSpace(request.Location))
            {
                sql += " AND Location LIKE @Location";
                parameters.Add("Location", $"%{request.Location}%");
            }

            if (request.MinimumSalary >= 0)
            {
                sql += " AND Salary >= @MinSalary";
                parameters.Add("MinSalary", request.MinimumSalary);
            }

            sql += " AND IsDeleted = 0";

            sql += " ORDER BY CreatedAt DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            parameters.Add("Offset", (request.Page - 1) * request.PageSize);
            parameters.Add("PageSize", request.PageSize);

            var jobs = await db.QueryAsync<JobDto>(sql, parameters);
            return jobs.ToList();
        }

        public async Task<Job?> GetJobPostById(Guid id)
        {
            return await _context.Jobs.FirstOrDefaultAsync(job => job.Id == id && job.IsDeleted == false);
        }

        public async Task<List<JobDto>> GetJobs(Guid id)
        {
            return await _context.Jobs
                .Where(job => job.EmployerId == id && job.IsDeleted == false)
                .AsNoTracking()
                .Select(job => new JobDto
                {
                    Id = job.Id,
                    Title = job.Title,
                    Description = job.Description,
                    Salary = job.Salary,
                    Location = job.Location
                })
                .ToListAsync();
        }

        public async Task<int?> UpdateJob(Guid id, JobRequestDto request)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(job => job.Id == id && job.IsDeleted == false);
            if (job is null)
            {
                return null;
            }

            job.Title = request.Title;
            job.Description = request.Description;
            job.Location = request.Location;
            job.Salary = request.Salary;

            return await _context.SaveChangesAsync();
        }

        public async Task<int?> DeleteJob(Guid id)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(job => job.Id == id && job.IsDeleted == false);
            if (job is null)
            {
                return null;
            }

            job.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
    }
}