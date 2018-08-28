import { ActionTree } from 'vuex';
import axios from 'axios';

import {
    RootState,
    AuctionState,
} from '@/store/types';

import {
    AuctionDto,
    AuctionTableDto,
 } from '@/poco';

import { MessageStatusEnum, Auction } from '@/model';

import {
    AUCTION_INITIAL_STATE,
    AUCTION_CHANGE_CURRENT_STATE,
    AUCTION_INITIAL_CURRENT,
    AUCTION_CHANGE_TABLE_STATE,
    AUCTION_DELETE_RECORD_FROM_LIST,
} from '@/store/mutation-types';
import { resolveSrv } from 'dns';

const actions: ActionTree<AuctionState, RootState> = {
    initialState({ commit }): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            commit(AUCTION_INITIAL_STATE);
            return resolve(true);
        });
    },
    initialCurrent({commit}): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            commit(AUCTION_INITIAL_CURRENT);
            return resolve(true);
        });
    },
    getDetail({commit, rootState, dispatch}, id: number): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/auctions/getForEdit?id=${id}`,
                { headers: { authorization: rootState.auth.token } })
                .then((response) => {
                    const auction: Auction = response.data as Auction;
                    commit(AUCTION_CHANGE_CURRENT_STATE, auction);
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Auction',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                    { root: true});
                    return resolve(false);
                });
        });
    },
    create({commit, rootState, dispatch }, auction: Auction): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.post(`${rootState.settings.apiUrl}/auctions/create`, auction,
                { headers: { authorization: rootState.auth.token } })
                .then((response) => {
                    const auct: Auction = response.data as Auction;
                    commit(AUCTION_CHANGE_CURRENT_STATE, auct);
                    dispatch('message/change', {
                        mod: 'Auction',
                        message: {
                            state: MessageStatusEnum.Success,
                            message: 'createdSuccessfully',
                        },
                    },
                    { root: true });
                    dispatch('auction/getAllForAdmin', {}, { root: true });
                    dispatch('auction/initialCurrent', {}, { root: true });
                    return resolve(true);
                })
                .catch((error) => {
                    dispatch('message/change', {
                        mod: 'Auction',
                        message: {
                            state: MessageStatusEnum.Error,
                            message: error.message,
                        },
                    },
                    { root: true});
                    return resolve(false);
                });
        });
    },
    getAllForAdmin({commit, rootState, dispatch}): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            return axios.get(`${rootState.settings.apiUrl}/auctions/getAllForAdmin`,
                { headers: { authorization: rootState.auth.token } })
            .then((response) => {
                commit(AUCTION_CHANGE_TABLE_STATE, response.data as AuctionTableDto[]);
                return resolve(true);
            })
            .catch((error) => {
                dispatch('message/change', {
                    mod: 'Auction',
                    message: {
                        state: MessageStatusEnum.Error,
                        message: error.message,
                    },
                },
                { root: true});
            });
        });
    },
    update({commit, rootState, dispatch}, auction: Auction): Promise<boolean> {
        return new Promise<boolean> ((resolve) => {
            return axios.put(`${rootState.settings.apiUrl}/auctions`, auction,
                { headers: { authorization: rootState.auth.token } })
            .then((response) => {
                commit(AUCTION_CHANGE_CURRENT_STATE, response.data as Auction);
                dispatch('message/change', {
                    mod: 'Auction',
                    message: {
                        state: MessageStatusEnum.Success,
                        message: 'updatedSuccessfully',
                    },
                },
                { root: true });
                dispatch('auction/getAllForAdmin', {}, { root: true });
                dispatch('auction/initialCurrent', {}, { root: true });
                return resolve(true);
            })
            .catch((error) => {
                dispatch('message/change', {
                    mod: 'Auction',
                    message: {
                        state: MessageStatusEnum.Error,
                        message: error.message,
                    },
                },
                { root: true});
                return resolve(undefined);
            });
        });
    },
    delete({commit, rootState, dispatch}, auctionId: number): Promise<boolean> {
        return new Promise<boolean> ((resolve) => {
            return axios.delete(`${rootState.settings.apiUrl}/auctions/delete`,
                {
                    params: {
                        id: auctionId,
                    },
                    headers: {
                        authorization: rootState.auth.token,
                    },
                })
            .then((response) => {
                commit(AUCTION_INITIAL_CURRENT);
                commit(AUCTION_DELETE_RECORD_FROM_LIST, response.data as Auction);
                dispatch('message/change', {
                    mod: 'Auction',
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
                    mod: 'Auction',
                    message: {
                        state: MessageStatusEnum.Error,
                        message: error.message,
                    },
                },
                { root: true});
                return resolve(false);
            });
        });
    },
};

export default actions;
