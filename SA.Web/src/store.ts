import Vue from 'vue';
import Vuex, { StoreOptions } from 'vuex';
import VuexPersistance from 'vuex-persist';

import { RootState } from '@/store/types';
import {
  profile,
  auth,
  settings,
  record,
} from '@/store/modules';

Vue.use(Vuex);

const vuexLocal = new VuexPersistance({
  storage: window.localStorage,
  supportCircular: true,
  modules: ['auth', 'settings'],
  key: 'simple_auction',
});

// create store with RootState
const store: StoreOptions<RootState> = {
  state: {
    version: '0.0.1 alpha',
    settings: undefined,
    auth: undefined,
    profile: undefined,
    record: undefined,
  },
  modules: {
    profile, // my own Store state hook up as a module
    auth,
    settings,
    record,
  },
  plugins: [vuexLocal.plugin], // vuex in localStorage as a plugin
};

// initialize store
export default new Vuex.Store<RootState>(store);
