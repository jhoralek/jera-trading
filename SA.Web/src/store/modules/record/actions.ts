import axios from 'axios';
import moment from 'moment-timezone';
import { ActionTree } from 'vuex';
import {
    RootState,
    RecordState,
} from '@/store/types';

import { Record, MessageStatusEnum, Bid, Auction } from '@/model';

import {
    RECORD_INITIAL_STATE,
    RECORD_CHANGE_LIST_STATE,
    RECORD_CHANGE_CURRENT_STATE,
    RECORD_INITIAL_CURRENT,
    RECORD_DELETE_RECORD_FROM_LIST,
    RECORD_SET_CURRENT_FILES,
    RECORD_APPEND_CURRENT_FILES,
    RECORD_SET_CURRENT_USER_ID,
    RECORD_SET_CURRENT_AUCTION_ID,
    RECORD_SET_VALID_DATES,
    RECORD_CHANGE_BIDS_TO_CURRENT,
    RECORD_CHANGE_WINNING_USER_ID,
    RECORD_UPDATE_LIST_STATE,
    RECORD_CHANGE_NUMBER_OF_BIDS_TO_CURRENT,
    RECORD_CHANGE_CURRENT_PRICE_TO_START_PRICE,
    RECORD_SET_CURRENT_PRICE,
    RECORD_EXTEND_VALID_TO,
} from '@/store/mutation-types';
import {
    RecordTableDto,
    FileSimpleDto,
    ResponseMessage,
    BidDto,
    RecordMinimumDto,
} from '@/poco';
import { record } from '.';

const actions: ActionTree<RecordState, RootState> = {
    /**
     * Initial state of Record object
     * @param param - commit
     */
    initialState({ commit }): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            commit(RECORD_INITIAL_STATE);
            return resolve(true);
        });
    },
    initialCurrent({ commit }): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            commit(RECORD_INITIAL_CURRENT);
            return resolve(true);
        });
    },
    /**
     * Load all active records
     */
    loadAllPublished({ commit, rootState, dispatch }): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(
                `${rootState.settings.apiUrl}/records/getAllCurrentAuctionItems`)
                .then((response) => {
                    const records: RecordTableDto[] = response.data as RecordTableDto[];
                    commit(RECORD_CHANGE_LIST_STATE, records);

                    response.data.forEach((item) => {
                        axios.get(`${rootState.settings.apiUrl}/files/getFilesByRecordId?id=${item.id}`)
                            .then((responseFiles) => {
                                const r: FileSimpleDto[] = responseFiles.data as FileSimpleDto[];

                                item.files = r;
                                commit(RECORD_CHANGE_LIST_STATE, records);
                            });

                        return axios.get(`${rootState.settings.apiUrl}/bids/getBidsByRecordId?id=${item.id}`)
                            .then((responseBids) => {
                                const b: BidDto[] = responseBids.data as BidDto[];

                                const bidIds = b.map((x) => x.userId);
                                // add unique ids into the array
                                item.biddingUserIds = [...new Set(bidIds)];
                                item.winningUserId = bidIds.length > 0 ? bidIds[0] : 0;
                                item.currentPrice = b.length === 0 ? item.startingPrice : b[0].price;

                                commit(RECORD_CHANGE_LIST_STATE, records);
                            });
                    });

                    dispatch('auction/getFutureAutions', {
                        mod: 'Auction',
                    },
                        { root: true });

                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(false);
                });
        });
    },
    loadUpdateAllPublished({ commit, rootState, dispatch }): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(
                `${rootState.settings.apiUrl}/records/getAllCurrentAuctionItemsMinimum`)
                .then((response) => {
                    const records: RecordMinimumDto[] = response.data as RecordMinimumDto[];
                    commit(RECORD_UPDATE_LIST_STATE, records);
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(false);
                });
        });
    },
    /**
     * Get record detail by id
     * @param id - record id
     */
    getDetail({ commit, rootState, dispatch }, id: number): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/records/getById?id=${id}`)
                .then((response) => {
                    const r: Record = response.data as Record;

                    commit(RECORD_CHANGE_CURRENT_STATE, r);
                    dispatch('record/getRecordsLastBid', id, { root: true });

                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(false);
                });
        });
    },
    getRecordFiles({ commit, rootState, dispatch }, id: number): Promise<FileSimpleDto[]> {
        return new Promise<FileSimpleDto[]>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/files/getFilesByRecordId?id=${id}`)
                .then((response) => {
                    const r: FileSimpleDto[] = response.data as FileSimpleDto[];

                    commit(RECORD_SET_CURRENT_FILES, r);

                    return resolve(r);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(null);
                });
        });
    },

    getActualRandom({ commit, rootState, dispatch }): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/records/actualRandom`)
                .then((response) => {
                    const r: Record = response.data as Record;
                    commit(RECORD_CHANGE_CURRENT_STATE, r);
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(false);
                });
        });
    },
    getBids({ commit, rootState, dispatch }, recordId: string): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/bids/getBidsByRecordId?id=${recordId}`)
                .then((response) => {
                    commit(RECORD_CHANGE_BIDS_TO_CURRENT, response.data as BidDto[]);
                    commit(RECORD_CHANGE_NUMBER_OF_BIDS_TO_CURRENT, response.data.length);

                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(false);
                });
        });
    },
    /**
     * Get all users active auctions where bidding
     * @param param0 Commit, root and dispatch object to manipulate and call other actions
     * @param id User id
     */
    getAllActiveWithUsersBids({ commit, rootState, dispatch }, id: number): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(
                `${rootState.settings.apiUrl}/records/allActiveWithUsersBids?id=${id}`,
                { headers: { authorization: rootState.auth.token } })
                .then((response) => {
                    const recods: Record[] = response.data as Record[];
                    commit(RECORD_CHANGE_LIST_STATE, recods);
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                });
        });
    },
    /**
     * Get actual price for record
     * @param param0 States
     * @param id idRecord
     */
    getActualPrice({ rootState, dispatch }, id: number): Promise<number> {
        return new Promise<number>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/bids/getActualPrice?id=${id}`,
                { headers: { authorization: rootState.auth.token } })
                .then((response) => {
                    return resolve(response.data as number);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                });
        });
    },
    getAllForAdmin({ commit, rootState, dispatch }): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/records/getAllForAdmin`,
                { headers: { authorization: rootState.auth.token } })
                .then((response) => {
                    commit(RECORD_CHANGE_LIST_STATE, response.data as RecordTableDto[]);
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                });
        });
    },
    getAuctionRecordsForAdmin({ commit, rootState, dispatch }, auctionId: number): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/records/getAuctionRecordsForAdmin?id=${auctionId}`,
                { headers: { authorization: rootState.auth.token } })
                .then((response) => {
                    commit(RECORD_CHANGE_LIST_STATE, response.data as RecordTableDto[]);
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                });
        });
    },
    createRecord({ commit, rootState, dispatch }, r: Record): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.post(`${rootState.settings.apiUrl}/records/create`, r,
                { headers: { authorization: rootState.auth.token } })
                .then((response) => {
                    commit(RECORD_CHANGE_CURRENT_STATE, response.data as Record);
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Success,
                            message: 'createdSuccessfully',
                        },
                    },
                        { root: true });
                    dispatch('record/getAuctionRecordsForAdmin', r.auctionId, { root: true });
                    dispatch('record/initialCurrent', {}, { root: true });
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(false);
                });
        });
    },
    uploadFiles({ commit, rootState, dispatch }, formData: any): Promise<File[]> {
        return new Promise<File[]>((resolve) => {
            return axios.post(`${rootState.settings.apiUrl}/files/uploadFiles`, formData,
                { headers: { authorization: rootState.auth.token } })
                .then((response) => {
                    return resolve(response.data as File[]);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(undefined);
                });
        });
    },
    setCurrent({ commit }, r: Record): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            commit(RECORD_CHANGE_CURRENT_STATE, r);
            return resolve(true);
        });
    },
    setCurrentUserId({ commit }, userId: number): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            commit(RECORD_SET_CURRENT_USER_ID, userId);
            return resolve(true);
        });
    },
    setCurrentRecordDatesFromAuction({ commit }, auction: Auction): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            const { validFrom, validTo } = auction;

            commit(RECORD_SET_VALID_DATES, { validFrom, validTo });
            return resolve(true);
        });
    },
    setCurrentRecordDates({ commit }, r: Record): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            const { validFrom, validTo } = r;

            commit(RECORD_SET_VALID_DATES, { validFrom, validTo });
            return resolve(true);
        });
    },
    setCurrentAuctionId({ commit }, auctionId: number): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            commit(RECORD_SET_CURRENT_AUCTION_ID, auctionId);
            return resolve(true);
        });
    },
    setFiles({ commit }, files: FileSimpleDto[]): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            commit(RECORD_SET_CURRENT_FILES, files);
            return resolve(true);
        });
    },
    addFiles({ commit }, files: FileSimpleDto[]): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            commit(RECORD_APPEND_CURRENT_FILES, files);
            return resolve(true);
        });
    },
    updateRecord({ rootState, dispatch }, r: Record): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.put(`${rootState.settings.apiUrl}/records`, r,
                { headers: { authorization: rootState.auth.token } })
                .then((response) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Success,
                            message: 'updatedSuccessfully',
                        },
                    },
                        { root: true });
                    dispatch('record/getAllForAdmin', {}, { root: true });
                    dispatch('record/initialCurrent', {}, { root: true });
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(undefined);
                });
        });
    },
    deleteRecord({ commit, rootState, dispatch }, recordId: number): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.delete(`${rootState.settings.apiUrl}/records/delete`,
                {
                    params: {
                        id: recordId,
                    },
                    headers: {
                        authorization: rootState.auth.token,
                    },
                })
                .then((response) => {
                    commit(RECORD_DELETE_RECORD_FROM_LIST, response.data as Record);
                    commit(RECORD_INITIAL_CURRENT);
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Success,
                            message: 'deleteSuccessfully',
                        },
                    },
                        { root: true });
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(false);
                });
        });
    },
    addBid({ commit, rootState, dispatch }, bid: Bid): Promise<number> {
        return new Promise<number>((resolve) => {
            return axios.post(`${rootState.settings.apiUrl}/bids/create`, bid,
                { headers: { authorization: rootState.auth.token } })
                .then((resp1) => {
                    const response = resp1.data as ResponseMessage<BidDto>;

                    commit(RECORD_SET_CURRENT_PRICE, response.entity.price);
                    commit(RECORD_EXTEND_VALID_TO, response.entity.recordValidTo);

                    dispatch('record/getBids', bid.recordId, { root: true });

                    if (response.code === 'createdSuccessfully') {
                        dispatch('record/sendEmailToOverbideduser', bid.recordId, { root: true });
                    }

                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: response.status,
                            message: response.code,
                        },
                    }, { root: true });

                    return resolve(response.entity.price);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(null);
                });
        });
    },
    sendEmailToOverbideduser({ rootState }, recordId: number): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/bids/sendEmailToOverbided?recordId=${recordId}`,
                { headers: { authorization: rootState.auth.token } })
                .then(() => {
                    return resolve(true);
                })
                .catch(() => {
                    return resolve(false);
                });
        });
    },
    getAllEndedRecords({ commit, rootState, dispatch }): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/records/getAllEndedRecords`,
                { headers: { authorization: rootState.auth.token } })
                .then((response) => {
                    commit(RECORD_CHANGE_LIST_STATE, response.data as RecordTableDto[]);
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(false);
                });
        });
    },
    getLatestEndedRecords({ commit, rootState, dispatch }, take: number): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/records/getLatestEndedRecords?take=${take}`)
                .then((response) => {
                    commit(RECORD_CHANGE_LIST_STATE, response.data as RecordTableDto[]);
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(false);
                });
        });
    },
    updateRecordDatesByAuctionId({ commit, rootState, dispatch }, auctionId: number): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/records/updateRecordDatesByAuctionId?id=${auctionId}`,
                { headers: { authorization: rootState.auth.token } })
                .then((resp1) => {
                    commit(RECORD_CHANGE_LIST_STATE, resp1.data as RecordTableDto[]);
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Success,
                            message: 'itemsUpdatedSuccessfully',
                        },
                    },
                        { root: true });
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(false);
                });
        });
    },
    getRecordsBidForAdmin({ commit, rootState, dispatch }, recordId: number): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/bids/getRecordsBidForAdmin?id=${recordId}`,
                { headers: { authorization: rootState.auth.token } })
                .then((resp1) => {
                    return axios.get(`${rootState.settings.apiUrl}/records/getById?id=${recordId}`)
                        .then((resp2) => {
                            commit(RECORD_CHANGE_CURRENT_STATE, resp2.data as RecordTableDto);
                            commit(RECORD_CHANGE_BIDS_TO_CURRENT, resp1.data as BidDto[]);
                            return resolve(true);
                        });
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(false);
                });
        });
    },
    getRecordsLastBid({ commit, rootState, dispatch }, recordId: string): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/bids/getRecordsLastBid?id=${recordId}`)
                .then((resp1) => {
                    const data = resp1.data as BidDto;

                    if (data !== null) {
                        commit(RECORD_CHANGE_WINNING_USER_ID, data);
                    } else {
                        commit(RECORD_CHANGE_CURRENT_PRICE_TO_START_PRICE);
                    }

                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Record',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                        { root: true });
                    return resolve(false);
                });
        });
    },
};

export default actions;
