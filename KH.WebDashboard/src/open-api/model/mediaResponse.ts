/**
 * Clean Architecture Web API V1
 *
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


export interface MediaResponse { 
    id?: number | null;
    createdDate?: string;
    createdById?: number | null;
    updatedDate?: string | null;
    updatedById?: number | null;
    isDeleted?: boolean;
    deletedDate?: string | null;
    deletedById?: number | null;
    fileContentResult?: Blob | null;
    isValidToDownload?: boolean;
    message?: string | null;
    fileName?: string | null;
    orignalName?: string | null;
    fileExtention?: string | null;
    contentType?: string | null;
    model?: string | null;
    modelId?: number | null;
    filePath?: string | null;
}

