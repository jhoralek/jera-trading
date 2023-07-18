import Vue from 'vue';
import Vuex, { StoreOptions } from 'vuex';
import VuexPersistance from 'vuex-persist';

import { RootState } from '@/store/types';
import {
  profile,
  auth,
  settings,
  record,
  message,
  auction,
} from '@/store/modules';

Vue.use(Vuex);

const vuexLocal = new VuexPersistance({
  storage: window.localStorage,
  supportCircular: true,
  modules: ['auth', 'settings', 'record', 'auction'],
  key: 'simple_auction',
});

// create store with RootState
const store: StoreOptions<RootState> = {
  state: {
    version: '1.0.0.23',
    settings: undefined,
    auth: undefined,
    profile: undefined,
    record: undefined,
    message: undefined,
    auction: undefined,
  },
  modules: {
    profile, // my own Store state hook up as a module
    auth,
    settings,
    record,
    message,
    auction,
  },
  plugins: [vuexLocal.plugin], // vuex in localStorage as a plugin
};

// initialize store
export default new Vuex.Store<RootState>(store);
