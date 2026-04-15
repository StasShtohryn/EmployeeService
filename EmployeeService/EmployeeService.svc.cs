using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class EmployeeService : IEmployeeService
    {

        private readonly EmployeeRepository _repository = new EmployeeRepository();

        public async Task<Employee> GetEmployeeById(int id)
        {
            var result = await _repository.GetSubtreeAsync(id);

            if (result == null)
                throw new WebFaultException<string>(
                    string.Format("Employee with ID={0} not found.", id),
                    HttpStatusCode.NotFound);

            return result;
        }

        public async Task<bool> EnableEmployee(int id, int enable)
        {
            bool found = await _repository.SetEnableAsync(id, enable);

            return found;
        }
    }

      
}