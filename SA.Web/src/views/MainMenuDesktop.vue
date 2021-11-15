<template>
    <v-toolbar class="hidden-sm-and-down">
        <v-toolbar-title class="logo-button">
            <v-btn flat to="/">
                <v-layout row wrap>
                    <v-flex xs12>
                        <img :src="pageLogo" /><span class="logo-upper">{{ resx('webPageName1') }}</span>
                        <span class="logo-capitalize">{{ resx('webPageName2') }}</span>
                    </v-flex>
                </v-layout>
            </v-btn>
        </v-toolbar-title>
        <v-spacer></v-spacer>
        <v-toolbar-items>
            <v-btn flat to="/"><div class="btn-text">{{ resx('home') }}</div></v-btn>
            <!-- <v-btn flat to="/news"><div class="btn-text">{{ resx('news') }}</div></v-btn>
            <v-btn flat to="/forDealers"><div class="btn-text">{{ resx('forDealers') }}</div></v-btn> -->
            <div class="fill-height align-content-center">
                <v-btn class="login-button" flat to="/auctions"><div class="btn-text">{{ resx('auctions') }}</div></v-btn>
            </div>
            <v-btn flat to="/ended"><div class="btn-text">{{ resx('endedAuctions') }}</div></v-btn>
            <!-- <v-btn flat to="/about"><div class="btn-text">{{ resx('about') }}</div></v-btn> -->
            <v-btn flat to="/faq"><div class="btn-text">{{ resx('faq') }}</div></v-btn>
            <v-btn flat to="/contact"><div class="btn-text">{{ resx('contact') }}</div></v-btn>

            <v-menu offset-y v-if="auth.isAuthenticated && auth.isDealer">
                <v-btn slot="activator" flat>{{ resx('administration') }}</v-btn>
                <v-list>
                    <v-list-tile to="/customer">
                        <v-list-tile-title>{{ resx('customerDetail') }}</v-list-tile-title>
                    </v-list-tile>
                    <v-list-tile to="/usersAdministration">
                        <v-list-tile-title>{{ resx('usersAdministration')}}</v-list-tile-title>
                    </v-list-tile>
                    <v-list-tile to="/auctionsAdministration">
                        <v-list-tile-title>{{ resx('auctionsAdministration')}}</v-list-tile-title>
                    </v-list-tile>
                    <!-- <v-list-tile to="/endedAdministration">
                        <v-list-tile-title>{{ resx('endedAuctions')}}</v-list-tile-title>
                    </v-list-tile> -->
                    <!-- <v-list-tile to="/record-administration">
                        <v-list-tile-title>{{ resx('recordAdministration')}}</v-list-tile-title>
                    </v-list-tile> -->
                    <v-list-tile to="/gdprAdministration">
                        <v-list-tile-title>{{ resx('gdprAdministration')}}</v-list-tile-title>
                    </v-list-tile>
                </v-list>
            </v-menu>
            <v-btn flat v-if="auth.isAuthenticated && !auth.isDealer" to="/customer">{{ resx('customerDetail') }}</v-btn>
            <div class="fill-height align-content-center">
                <login-form-component />
            </div>
            <!-- <div class="fill-height align-content-center">
                <language-component />
            </div> -->
        </v-toolbar-items>
    </v-toolbar>
</template>

<script lang="ts">

    import BaseView from './BaseView.vue';
    import Component from 'vue-class-component';
    import { State } from 'vuex-class';
    import { SettingsState, AuthState } from './../store/types';

    import {
        LoginFormComponent,
        LanguageComponent,
    } from './../components';

    @Component({
        components: {
            LoginFormComponent,
            LanguageComponent,
        },
    })
    export default class MainMenuDesktop extends BaseView {
        @State('settings')
        private settings: SettingsState;
        @State('auth')
        private auth: AuthState;

        get pageLogo(): string {
            return `${this.settings.apiUrl.replace('/api', '')}/img/logo-big.png`;
        }
    }

</script>

<style>

    .login-button {
        height: 40px !important;
        margin-top: 65px !important;
    }
</style>