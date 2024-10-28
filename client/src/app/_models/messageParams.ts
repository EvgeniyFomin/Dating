import { Container } from "../_enums/container";
import { PagingParams } from "./pagingParams";

export class MessageParams extends PagingParams {
    container: Container;

    constructor(container: Container) {
        super(1, 5);
        this.container = container;
    }
}