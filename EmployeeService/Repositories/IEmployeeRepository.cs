using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EmployeeService
{
    public interface IEmployeeRepository
    {

        Task<Employee> GetSubtreeAsync(int rootId);



        Task<bool> SetEnableAsync(int employeeId, int enable);
    }
}