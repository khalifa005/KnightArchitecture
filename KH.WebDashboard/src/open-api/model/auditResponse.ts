/**
 * Clean Architecture Web API V1
 *
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


export interface AuditResponse { 
    Id?: number;
    UserId?: string | null;
    Type?: string | null;
    TableName?: string | null;
    DateTime?: string | null;
    OldValues?: string | null;
    NewValues?: string | null;
    AffectedColumns?: string | null;
    PrimaryKey?: string | null;
}

