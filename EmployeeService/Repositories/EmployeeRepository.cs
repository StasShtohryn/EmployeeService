


using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;



namespace EmployeeService
{
    public class EmployeeRepository: IEmployeeRepository
    {

        private readonly string _connectionString;

        public EmployeeRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString; ;
        }

        public async Task<Employee> GetSubtreeAsync(int rootId)
        {
            var sqlQuery = @"
            with SubTree AS (
                select ID, Name, ManagerId, Enable, 0 as Level
                from Employee
                where ID = @rootId

                UNION ALL

                select e.ID, e.Name, e.Managerid, e.Enable, st.Level + 1 as Level
                from Employee e inner join SubTree st on e.ManagerId = st.ID
            )
            select ID, Name, ManagerId, Enable, Level
            from SubTree
            order by Level";


            var nodesDictionary = new Dictionary<int, Employee>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("rootId", rootId));

                    await connection.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var level = 0;
                        while (await reader.ReadAsync())
                        {
                            var employee = new Employee()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                ManagerId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                                Enabled = reader.GetBoolean(3),
                            };
                            level = reader.GetInt32(4);

                            if (level != 0)
                            {
                                nodesDictionary[employee.ManagerId.Value].Subordinates.Add(employee);
                            }
                            nodesDictionary[employee.Id] = employee;
                        }
                    }
                }
            }

            if (nodesDictionary.Count == 0)
                return null;

            return nodesDictionary[rootId];
        }



        public async Task<bool> SetEnableAsync(int employeeId, int enable)
        {
            var sqlQuery = @"
            update Employee
            set Enable = @enable
            where Id = @Id
        ";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("enable", enable));
                    cmd.Parameters.Add(new SqlParameter("Id", employeeId));

                    await connection.OpenAsync();
                    var rowsAffected = await cmd.ExecuteNonQueryAsync();


                    return rowsAffected > 0;
                }
            }
        }
    }
}
