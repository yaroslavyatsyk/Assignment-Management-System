using ToDo_Web_App.Models;

namespace ToDo_Web_App.Repositories
{
    public interface IAssignmentRepository
    {
        public Task<List<Assignment>> GetAssignmentsAsync(string userName, string? title, string? sortOrder, string? category, int? pageNumber);
        public Task<Assignment?> GetAssignmentByIdAsync(int id);

        public Task AddAssignmentAsync(Assignment assignment);

        public Task DeleteAssignmentAsync(int id);

        public Task UpdateAssignmentAsync(Assignment assignment);


    }
}
