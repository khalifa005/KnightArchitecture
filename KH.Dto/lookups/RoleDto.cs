namespace KH.Dto.Models.lookups
{
    public class RoleDto : BasicEntityDto
    {

        public RoleDto()
        {
        }

        #region Props

        public int? ReportToRoleId { get; set; }
        public RoleDto? ReportToRole { get; set; }
        #endregion

    }


}
