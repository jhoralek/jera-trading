<template>
  <div class="homePage">
    <v-layout row wrap>
      <v-flex xs12 md6 class="logo-container">
        <div class="rectangle">
        </div>
        <img :src="logo" class="home-page-logo" />
      </v-flex>
      <v-flex xs12 md6>
        <home-component />
      </v-flex>
    </v-layout>
    <!-- <v-layout row wrap>
      <v-flex xs12>
        <actual-random-component />
      </v-flex>
    </v-layout> -->
    <v-container grid-list-md class="registration-container" v-if="!auth.isAuthenticated">
      <v-layout row wrap>
        <v-flex xs12>
          <h2 class="header">{{ resx('registration') }}</h2>
        </v-flex>
      </v-layout>
      <v-layout row wrap>
        <v-flex xs12>
          <registration-component />
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script lang="ts">
import { State, Action, namespace } from 'vuex-class';
import Component from 'vue-class-component';

import BaseView from './BaseView.vue';
import {
  RegistrationComponent,
  HomeComponent,
  ActualRandomComponent,
} from './../components';

import { SettingsState, AuthState } from './../store/types';

const ProfileAction = namespace('profile', Action);

@Component({
  components: {
    RegistrationComponent,
    HomeComponent,
    ActualRandomComponent,
  },
})
export default class Home extends BaseView {
  @State('settings')
  private settings: SettingsState;
  @State('auth')
  private auth: AuthState;

  @ProfileAction('initialState')
  private init: any;

  public mounted() {
    this.init();
  }

  get logo(): string {
    return `${this.settings.apiUrl.replace('/api', '')}/img/home_cars_jera_trading.png`;
  }
}

</script>

<style>

.logo-container {
  position: relative;
}

.home-page-logo {
  width: 90%;
  max-width: 706px;
  top: 0px;
  left: 0px;
  position: absolute;
}

.rectangle {
  max-width: 404px;
  height: 100%;
  min-height: 300px;
  border-bottom-right-radius: 59px;
  background-color: #000000;
  z-index: 1;
}

h2.header {
  font-size: 35px;
  font-family: Roboto;
  font-weight: normal;
  font-style: normal;
  font-stretch: normal;
  line-height: 1.15;
  letter-spacing: 0.3px;
  text-align: left;
  color: #1a1a2e;
}

.registration-container {
  padding-top: 80px !important;
  padding-bottom: 50px !important;
  max-width: 700px !important;
}

</style>