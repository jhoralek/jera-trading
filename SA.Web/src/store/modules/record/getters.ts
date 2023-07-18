import { GetterTree } from 'vuex';
import {
    RootState,
    RecordState,
} from '@/store/types';

import { Record } from '@/model';
import { RecordTableDto, BidDto } from '@/poco';

const getters: GetterTree<RecordState, RootState> = {
    /**
     * Check if the record state is not empty
     * @param state - Record's state
     */
    isRecordLoaded(state): boolean {
        return state.current !== undefined;
    },
    /**
     * Get current record in state
     * @param state - Record state
     */
    getCurrent(state): Record {
        const { current } = !state.error && state;
        return current;
    },
    /**
     *
     * @param state
     */
    getRecords(state): RecordTableDto[] {
        const { records } = !state.error && state;
        return records;
    },

    getEndedRecors(state): RecordTableDto[] {
        const { endedRecords } = !state.error && state;
        return endedRecords;
    },

    getBids(state): BidDto[] {
        const { bids } = !state.error && state.current;
        return bids;
    },

    getCurrentWinnerId(state): number {
        const { currentWinnerId } = !state.error && state;
        return currentWinnerId;
    },
};

export default getters;
