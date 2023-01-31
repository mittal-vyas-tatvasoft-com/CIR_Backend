namespace CIR.Core.ViewModel.Usersvm
{
    public class RoleViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool AllPermissions { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastEditedOn { get; set; }
        public int TotalCount { get; set; }

    }
    public class RoleModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool AllPermissions { get; set; }

    }
    public class RolePrivilegesMModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public long Value { get; set; }
    }
}
