/**
 * Clean Architecture Web API V1
 *
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


export interface SmsTrackerResponse { 
    Id?: number | null;
    CreatedDate?: string;
    CreatedById?: number | null;
    UpdatedDate?: string | null;
    UpdatedById?: number | null;
    IsDeleted?: boolean;
    DeletedDate?: string | null;
    DeletedById?: number | null;
    MobileNumber?: string | null;
    Message?: string | null;
    IsSent?: boolean | null;
    FailureReasons?: string | null;
    Status?: string | null;
    Model?: string | null;
    ModelId?: number;
}

