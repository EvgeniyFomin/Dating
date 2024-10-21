import { PagingParams } from "./pagingParams";
import { User } from "./user";

export class UserParams extends PagingParams {
    gender: string;
    minAge = 18;
    maxAge = 99;

    orderBy = 'lastActive';

    constructor(user: User | null) {
        super(1, 5);
        console.log('user in constructor of params: ' + JSON.stringify(user));
        this.gender = this.getGenderFilter(user?.gender)
    }

    private getGenderFilter(gender: any) {
        switch (gender) {
            case 1:
                return '2';
            case 2:
                return '1';
            default:
                return '3';
        }
    }
}