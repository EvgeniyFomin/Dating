export class PagingParams {
    pageNumber: number;
    pageSize: number;

    constructor(pageNumber: number, pageSize: number) {
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
    }
}