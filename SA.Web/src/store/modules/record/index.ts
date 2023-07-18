import { Module } from 'vuex';
import { RootState, RecordState } from '@/store/types';

import actions from '@/store/modules/record/actions';
import getters from '@/store/modules/record/getters';
import mutations from '@/store/modules/record/mutations';

export const state: RecordState = {
    current: undefined,
    currentWinnerId: -1,
    records: [],
    endedRecords: [],
    error: false,
};

const namespaced: boolean = true;

export const record: Module<RecordState, RootState> = {
    namespaced,
    state,
    actions,
    getters,
    mutations,
};
