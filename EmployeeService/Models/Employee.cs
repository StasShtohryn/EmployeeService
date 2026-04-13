

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;


namespace EmployeeService
{
    [DataContract]
    public class Employee
    {
        public Employee()
        {
            Subordinates = new List<Employee>();
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int? ManagerId { get; set; }
        public bool Enabled { get; set; }
        [DataMember]
        public List<Employee> Subordinates { get; set; }
    }
}
