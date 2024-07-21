namespace KH.Dto.Models.lookups
{
    public class DepartmentDto : BasicEntityDto
    {

        public DepartmentDto()
        {
        }

        public DepartmentDto(Department e)
        {
            Id = e.Id;
            Description = e.Description;
            NameEn = e.NameEn;
            NameAr = e.NameAr;
        }

        public Department ToEntity()
        {
            var e = new Department()
            {
                NameAr = NameAr,
                NameEn = NameEn,
                Description = Description,

            };

            if (Id.HasValue)
            {
                //update mode
                IsUpdateMode = true;
                e.Id = Id.Value;
            }
            else
            {
                //creation mode
            }

            return e;
        }

        #region Props

        public bool IsUpdateMode { get; set; }

        #endregion

        #region FK

        #endregion
    }

    public class PolicyInsuranceIssuingSourceDto : BasicEntityDto
    {

        public PolicyInsuranceIssuingSourceDto()
        {
        }


        #region Props

        public bool IsUpdateMode { get; set; }

        #endregion

        #region FK

        #endregion
    }
}
