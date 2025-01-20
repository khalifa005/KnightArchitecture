import { LookupResponse } from "../models/base/response/lookup.model";
type Mappable = {
    id?: number | null;
    nameAr?: string | null;
    nameEn?: string | null;
};
// Generic transformation function
export function transformToLookup<T extends Mappable>(responseArray: T[]): LookupResponse[] {
    return responseArray.map(
        (item) =>
            new LookupResponse(
                item.id || 0,
                item.nameAr || '',
                '',
                item.nameEn || ''
            )
    );
}