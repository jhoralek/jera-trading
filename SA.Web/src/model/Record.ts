import { User, File} from '@/model';
import { BidDto, AuctionDto } from '@/poco';

export default interface Record {
    isActive: boolean;
    name: string;
    color?: string;
    mileage: string;
    engineCapacity?: string;
    body?: string; // karoserie
    power?: string;
    fuel: string;
    transmission: string;
    axle: string; // náprava 4x4
    registrationCheck: string;
    contactToAppointment: string;
    user: User; // user who creates this record (can be only dealer)
    userId: number;
    minimumBid: number;
    startingPrice: number;
    validFrom: Date;
    validTo: Date;
    defects?: string; // závady
    moreDescription?: string;
    state?: string; // stav
    equipment?: string; // výbava
    vin?: string;
    numberOfSeets?: number;
    numberOfBids?: number;
    euroNorm?: string;
    doors?: number;
    dateOfFirstRegistration: Date;
    stk: Date;
    dimensions?: string; // celkove rozmery
    operationWeight?: string; // provozni hmotnost
    mostTechnicallyAcceptableWeight?: string; // nejvetsi technicka pripustna hmotnost
    maximumWeight?: string; // maximalni povolena hmotnost
    mostTechnicallyWeightOfRide?: string; // nejvetsi technicky pripustna hmotnost jizdni soupravy
    maximumWeightOfRide?: string; // nejvetsi povolena hmotnost jizdni soupravy
    files?: File[];
    bids?: BidDto[];
    id?: number;
    currentPrice: number;
    auctionId: number;
    auction: AuctionDto;
    withVat: boolean;
}
