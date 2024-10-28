import { PagingParams } from "./pagingParams";

export class MessageParams extends PagingParams {
    container: string = '0'

    constructor(container: string) {
        super(1, 5);
        this.container = container;
    }
}