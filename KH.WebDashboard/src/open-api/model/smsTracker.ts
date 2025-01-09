/**
 * Clean Architecture Web API V1
 *
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


export interface SmsTracker { 
    id?: number;
    createdDate?: string;
    createdById?: number | null;
    updatedDate?: string | null;
    updatedById?: number | null;
    isDeleted?: boolean;
    deletedDate?: string | null;
    deletedById?: number | null;
    mobileNumber?: string | null;
    message?: string | null;
    isSent?: boolean | null;
    failureReasons?: string | null;
    scheduleSendDate?: string | null;
    status?: string | null;
    model?: string | null;
    modelId?: number;
}

