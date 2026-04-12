

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;


namespace EmployeeService
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ManagerId { get; set; }
        public bool Enabled { get; set; }
    }

    public class EmployeeNode
    {
        public EmployeeNode()
        {
            Subordinates = new List<EmployeeNode>();
        }

        public Employee Employee { get; set; }
        public List<EmployeeNode> Subordinates { get; set; }
    }
}
