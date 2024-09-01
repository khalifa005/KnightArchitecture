namespace KH.Dto.lookups.PolicyIssuingSourceDto.Response
{
  public class PolicyIssuingSourceListResponse : BasicEntityDto
  {
    //we used light BasicEntityDto because in pagination list we may
    //not need the tracking info like created by ..etc
    //we can use auto mapper to do mapping or doing our own using ctor

    public PolicyIssuingSourceListResponse()
    {
    }

    public PolicyIssuingSourceListResponse(PolicyIssuingSource e)
    {
      NameAr = e.NameAr;
      NameEn = e.NameEn;
      Description = e.Description;
    }

  }

}
