namespace KH.Dto.Models.lookups
{
    public class RoleDto : BasicEntityDto
    {

        public RoleDto()
        {
        }

        #region Props

        public long? ReportToRoleId { get; set; }
        public RoleDto? ReportToRole { get; set; }
        #endregion

    }


}
