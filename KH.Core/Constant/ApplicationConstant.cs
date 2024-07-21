using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.Helper.Constant
{
  public static class ApplicationConstant
  {
    public const string MOBILE_NUMBER_PREFIX = "966";
    public const string SUPER_ADMIN_PERMISSION = "SuperAdmin";
    public const string CRM_CLIENT_SYSTEM_PERMISSION = "Customer";

    #region [PERMISSIONS] According To System Functions Name

    //--Ticket Category
    public const string TICKET_CATEGORY_PERMISSION = "ticket-categories";
    public const string ADD_TICKET_CATEGORY_PERMISSION = "add-ticket-category";
    public const string EDIT_TICKET_CATEGORY_PERMISSION = "edit-ticket-category";
    public const string DELETE_TICKET_CATEGORY_PERMISSION = "delete-ticket-category";

    //Ticket Status
    public const string TICKET_STATUS_PERMISSION = "ticket-status";
    public const string EDIT_TICKET_STATUS_PERMISSION = "edit-ticket-status";
    public const string DELETE_TICKET_STATUS_PERMISSION = "delete-ticket-status";


    #endregion

  }
}
