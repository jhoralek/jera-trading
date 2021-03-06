import Country from '@/model/Country';

export default interface Address {
    street: string;
    city: string;
    postCode: string;
    country: Country;
    countryId: number;
    id?: number;
    created: Date;
}
