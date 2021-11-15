import { MutationTree } from 'vuex';
import { RecordState } from '@/store/types';
import { Record } from '@/model';
import { FileSimpleDto, RecordTableDto, BidDto, RecordMinimumDto } from '@/poco';

const DATE_FORMAT = 'DD.MM.YYYY HH:mm';

const mutations: MutationTree<RecordState> = {
    /**
     * Initialization of the state
     * @param state Record state
     */
    RECORD_INITIAL_STATE(state) {
        state.current = undefined;
        state.currentWinnerId = -1;
        state.records = [];
        state.error = false;
    },
    RECORD_INITIAL_CURRENT(state) {
        state.error = false;
        state.current = {
            isActive: false,
            files: [],
        } as Record;
        state.currentWinnerId = -1;
    },
    /**
     * Change current record state
     * @param state - Record state
     * @param record - value of record
     */
    RECORD_CHANGE_CURRENT_STATE(state, record: Record) {
        state.error = false;
        state.current = record;

        if (record !== null && record.validFrom !== null) {
            state.current.validFrom = record.validFrom;
        }
        if (record !== null && record.validTo !== null) {
            state.current.validTo = record.validTo;
        }
        if (record !== null && record.dateOfFirstRegistration !== null) {
            state.current.dateOfFirstRegistration = new Date(record.dateOfFirstRegistration);
        }
        if (record !== null && record.stk !== null) {
            state.current.stk = new Date(record.stk);
        }
    },
    /**
     * Change list of records
     * @param state - Record state
     * @param records - list of records
     */
    RECORD_CHANGE_LIST_STATE(state, records: RecordTableDto[]) {
        state.error = false;
        state.records = records;
    },
    RECORD_DELETE_RECORD_FROM_LIST(state, record: Record) {
        state.error = false;
        state.records = state.records
            .filter((item) => item.id !== record.id);
    },
    RECORD_SET_CURRENT_FILES(state, files: FileSimpleDto[]) {
        state.error = false;
        if (state.current !== undefined) {
            state.current.files = files;
        }
    },
    RECORD_APPEND_CURRENT_FILES(state, files: FileSimpleDto[]) {
        state.error = false;
        files.forEach((x) => state.current.files.push(x));
    },
    RECORD_SET_CURRENT_USER_ID(state, userId: number) {
        state.error = false;
        state.current.userId = userId;
    },
    RECORD_SET_CURRENT_AUCTION_ID(state, auctionId: number) {
        state.error = false;
        state.current.auctionId = auctionId;
    },
    RECORD_CHANGE_BIDS_TO_CURRENT(state, bids: BidDto[]) {
        state.error = false;
        state.current.bids = bids as BidDto[];
    },
    RECORD_SET_VALID_DATES(state, dates: any) {
        const { validFrom, validTo } = dates;

        state.error = false;

        const formatedFrom = this._vm.$moment(new Date(validFrom), DATE_FORMAT).utc(true);
        const formatedTo = this._vm.$moment(new Date(validTo), DATE_FORMAT).utc(true);

        state.current.validFrom = formatedFrom;
        state.current.validTo = formatedTo;
    },
    RECORD_CHANGE_NUMBER_OF_BIDS_TO_CURRENT(state, numberOfBids: number) {
        state.error = false;

        if (state.current !== undefined) {
            state.current.numberOfBids = numberOfBids;
        }
    },
    RECORD_CHANGE_WINNING_USER_ID(state, bid: BidDto) {
        const { userId, price, recordValidTo } = bid;

        state.error = false;

        state.currentWinnerId = userId;
        state.current.currentPrice = price;
        state.current.validTo = recordValidTo;

        const isIn = state.current.bids
            .filter((item) => item.id === bid.id).length > 0;

        if (!isIn) {
            state.current.bids.unshift(bid);
            state.current.numberOfBids++;
        }
    },
    RECORD_CHANGE_CURRENT_PRICE_TO_START_PRICE(state) {
        state.error = false;

        if (state.current !== undefined) {
            state.current.currentPrice = state.current.startingPrice;
        }
    },
    RECORD_UPDATE_LIST_STATE(state, records: RecordMinimumDto[]) {
        state.error = false;

        state.records.forEach((item) => {
            const updItem = records.find((f) => f.id === item.id);

            if (updItem !== undefined) {
                item.validTo = updItem.validTo;
                item.currentPrice = updItem.currentPrice;
                item.winningUserId = updItem.winningUserId;
            }
        });
    },
    RECORD_SET_CURRENT_PRICE(state, price: number) {
        state.error = false;

        state.current.currentPrice = price;
    },
    RECORD_EXTEND_VALID_TO(state, validTo: Date) {
        state.error = false;

        state.current.validTo = validTo;
    },
};

export default mutations;
