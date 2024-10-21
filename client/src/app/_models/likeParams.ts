import { PagingParams } from "./pagingParams";

export class LikeParams extends PagingParams {
    predicate: string;

    constructor(predicate: string) {
        super(1, 5);
        this.predicate = predicate;
    }
}