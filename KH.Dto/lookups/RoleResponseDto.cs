namespace KH.Dto.Models.lookups
{
    public class RoleResponseDto : BasicEntityDto
    {

        public RoleResponseDto()
        {
        }

        #region Props

        public long? ReportToRoleId { get; set; }
        public RoleResponseDto? ReportToRole { get; set; }
        #endregion

    }


}
