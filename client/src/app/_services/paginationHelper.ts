import { HttpParams, HttpResponse } from "@angular/common/http";
import { PagingParams } from "../_models/pagingParams";
import { signal } from "@angular/core";
import { PaginatedResult } from "../_models/pagination";

export function setPaginationHeaders(pagingParams: PagingParams): HttpParams {
    let params = new HttpParams();

    if (pagingParams.pageNumber && pagingParams.pageSize) {
        params = params.append('pageNumber', pagingParams.pageNumber);
        params = params.append('pageSize', pagingParams.pageSize);
    }

    return params;
}

export function setPaginatedResponse<T>(
    response: HttpResponse<T>,
    paginatedResult: ReturnType<typeof signal<PaginatedResult<T> | null>>) {
    if ((response.body as []).length <= 0) {
        paginatedResult.set(null);
        return;
    }

    paginatedResult.set({
        items: response.body as T,
        pagination: JSON.parse(response.headers.get('Pagination')!)
    })
};