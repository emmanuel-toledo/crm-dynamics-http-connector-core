using Dataverse.Http.Connector.Core.Domains.Annotations;

namespace Dataverse.Domain.Test.Models
{
    [Entity("crmit_employee", "crmit_employees")]
    public class Employees
    {
        [UniqueIdentifier("crmit_EmployeeId", "crmit_employeeid")]
        public Guid Id { get; set; }

        [Text("crmit_Name", "crmit_name")]
        public string? Name { get; set; }

        // ReadOnly column
        [Text("crmit_EmployeeNumber", "crmit_employeenumber", true)]
        public string? EmployeeNumber { get; set; }

        [DateTimeOf("CreatedOn", "createdon")]
        public DateTime CreatedOn { get; set; }

        [DateTimeOf("ModifiedOn", "modifiedon")]
        public DateTime ModifiedOn { get; set; }

        [OptionSet("statuscode", "statuscode")]
        public int StatusCode { get; set; }

        [OptionSet("statecode", "statecode")]
        public int StateCode { get; set; }

        [BoolOptionSet("crmit_IsDeleted", "crmit_isdeleted")]
        public bool IsDeleted { get; set; }

        [Lookup("OwnerId", "ownerid", "systemusers")]
        public Guid OwnerId { get; set; }
    }
}
