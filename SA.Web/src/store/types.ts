import { User, Country, Record, Auction, Customer } from '@/model';
import {
  MessageDto,
  RecordTableDto,
  AuctionTableDto,
  AuctionDto,
  AuctionLookupDto,
} from '@/poco';

/**
 * This is root state of app store
 */
export interface RootState {
  version: string;
  settings: SettingsState;
  auth: AuthState;
  profile: ProfileState;
  record: RecordState;
  message: MessageState;
  auction: AuctionState;
}
/**
 * User state
 */
export interface ProfileState {
  user?: User;
  usersCurrent: RecordTableDto[];
  customerCurrent: Customer;
  usersEnded: RecordTableDto[];
  list: User[];
  error: boolean;
  errorMessage: string;
}
/**
 * Used for authentication
 */
export interface AuthState {
  userId: number;
  userName: string;
  token: string;
  language: string;
  isAuthenticated: boolean;
  isDealer: boolean;
  isFeePayed: boolean;
  error: boolean;
  errorMessage: string;
  secondsToLogout: number;
  timer: any;
}
/**
 * Default settings off webpage
 */
export interface SettingsState {
  language: string;
  resource: Dictionary<string>;
  countries: Dictionary<Country[]>;
  apiUrl: string;
  tableView: boolean;
  currentForm: string;
  version: string;
  isMobile: boolean;
}
/**
 * Record state
 */
export interface RecordState {
  current?: Record;
  currentWinnerId: number;
  records: RecordTableDto[];
  endedRecords: RecordTableDto[];
  error: boolean;
}

/**
 * Error state
 */
export interface MessageState {
  isOn: boolean;
  module: string;
  created: Date;
  message: MessageDto;
  timeout: number;
  fromResources: boolean;
  buttonText: string;
  callbackFnc: any;
}

/**
 * Auction state
 */
export interface AuctionState {
  auctionsTable: AuctionTableDto[];
  lookupList: AuctionLookupDto[];
  auctions: AuctionDto[];
  current: Auction;
}

export interface Dictionary<T> {
  [key: string]: T;
}
