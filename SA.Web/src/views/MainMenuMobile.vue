<template>
    <div class="hidden-md-and-up mobile-menu">
        <v-expansion-panel>
        <v-expansion-panel-content>
            <div slot="header">
            <router-link
                class="nav-item btn btn--flat btn--router"
                to="/">
                <img :src="pageLogo" class="mobile-img" />
                <span class="mobile-logo-upper">{{ resx('webPageName1') }}</span>
                <span class="mobile-logo-capitalize">{{ resx('webPageName2') }}</span>
            </router-link>
            </div>
            <!-- <v-card>
            <v-flex justify-start offset-xs1>
                <router-link
                class="nav-item btn btn--flat btn--router"
                style="justify-content: left"
                to="/news">{{ resx('news') }}
                </router-link>
            </v-flex>
            </v-card>
            <v-card>
            <v-flex justify-start offset-xs1>
                <router-link
                class="nav-item btn btn--flat btn--router"
                style="justify-content: left"
                to="/forDealers">{{ resx('forDealers') }}
                </router-link>
            </v-flex>
            </v-card> -->
            <v-card>
            <v-flex justify-start offset-xs1>
                <v-layout row>
                <router-link
                    class="nav-item btn btn--flat btn--router"
                    style="justify-content: left"
                    to="/auctions">{{ resx('auctions') }}
                </router-link>
                </v-layout>
            </v-flex>
            </v-card>
            <v-card>
                <v-flex justify-start offset-xs1>
                    <v-layout row>
                    <router-link
                        class="nav-item btn btn--flat btn--router"
                        style="justify-content: left"
                        to="/ended">{{ resx('endedAuctions') }}
                    </router-link>
                    </v-layout>
                </v-flex>
            </v-card>
            <!-- <v-card>
            <v-flex justify-start offset-xs1>
                <router-link
                class="nav-item btn btn--flat btn--router"
                style="justify-content: left"
                to="/about">{{ resx('about') }}
                </router-link>
            </v-flex>
            </v-card> -->
            <v-card>
            <v-flex justify-start offset-xs1>
                <router-link
                class="nav-item btn btn--flat text-xs-left"
                style="justify-content: left"
                to="/faq">{{ resx('faq') }}
                </router-link>
            </v-flex>
            </v-card>
            <v-card>
            <v-flex justify-start offset-xs1>
                <router-link
                class="nav-item btn btn--flat text-xs-left"
                style="justify-content: left"
                to="/contact">{{ resx('contact') }}
                </router-link>
            </v-flex>
            </v-card>
            <v-card v-if="auth.isAuthenticated && auth.isDealer">
            <v-flex justify-start offset-xs1>
                <router-link
                class="nav-item btn btn--flat btn--router"
                style="justify-content: left"
                to="/usersAdministration">{{ resx('usersAdministration') }}
                </router-link>
            </v-flex>
            </v-card>
            <v-card v-if="auth.isAuthenticated">
            <v-flex justify-start offset-xs1>
                <router-link
                class="nav-item btn btn--flat btn--router"
                style="justify-content: left"
                to="/customer">{{ resx('customerDetail') }}
                </router-link>
            </v-flex>
            </v-card>
            <v-card v-if="auth.isAuthenticated && auth.isDealer">
            <v-flex justify-start offset-xs1>
                <router-link
                class="nav-item btn btn--flat btn--router"
                style="justify-content: left"
                to="/auctionsAdministration">{{ resx('auctionsAdministration') }}
                </router-link>
            </v-flex>
            </v-card>
            <!-- <v-card v-if="auth.isAuthenticated">
            <v-flex justify-start offset-xs1>
                <router-link
                class="nav-item btn btn--flat btn--router"
                style="justify-content: left"
                to="/record-administration">{{ resx('recordAdministration') }}
                </router-link>
            </v-flex>
            </v-card> -->
            <v-card v-if="auth.isAuthenticated && auth.isDealer">
            <v-flex justify-start offset-xs1>
                <router-link
                class="nav-item btn btn--flat btn--router"
                style="justify-content: left"
                to="/gdprAdministration">{{ resx('gdprAdministration') }}
                </router-link>
            </v-flex>
            </v-card>
            <v-card>
            <v-flex justify-start offset-xs1>
                <login-form-component />
            </v-flex>
            </v-card>
        </v-expansion-panel-content>
        </v-expansion-panel>
    </div>
</template>

<script lang="ts">

import Component from 'vue-class-component';
import { State } from 'vuex-class';
import { SettingsState, AuthState } from './../store/types';

import {
  LoginFormComponent,
} from './../components';

import BaseView from './BaseView.vue';

@Component({
    components: {
        LoginFormComponent,
    },
})
export default class MainMenuDesktop extends BaseView {
    @State('settings')
    private settings: SettingsState;
    @State('auth')
    private auth: AuthState;

    get pageLogo(): string {
         return `${this.settings.apiUrl.replace('/api', '')}/img/logo-small.png`;
    }
}

</script>

<style>

.application .theme--light.v-expansion-panel .v-expansion-panel__container, .theme--light .v-expansion-panel .v-expansion-panel__container,
.mobile-menu .v-expansion-panel__body .v-card {
    background-color: black !important;
    color: white !important;
}

.application .theme--light.v-expansion-panel .v-expansion-panel__container .v-expansion-panel__header .v-expansion-panel__header__icon .v-icon, .theme--light .v-expansion-panel .v-expansion-panel__container .v-expansion-panel__header .v-expansion-panel__header__icon .v-icon {
    color: white !important;
}

.mobile-menu .v-expansion-panel__body {
    padding-bottom: 10px !important;
}

.mobile-menu .v-expansion-panel__body .v-card {
    padding-top: 5px !important;
    padding-bottom: 5px !important;
}

.mobile-menu a {
    color: white !important;
}

.mobile-img {
    padding-top: 3px;
    width: 20px !important;
}

.mobile-logo-upper {
    font-size: 15px !important;
    text-transform: uppercase !important;
    letter-spacing: 0.2em !important;
    padding-left: 5px !important;
}

.mobile-logo-capitalize {
    text-transform: none !important;
    font-weight: 300;
    letter-spacing: 0.1em !important;
    padding-left: 5px !important;
}

.v-expansion-panel__header a {
    text-decoration: none !important;
}

.theme--light .mobile-menu .v-expansion-panel .v-expansion-panel__container .v-expansion-panel__header .v-expansion-panel__header__icon .v-icon {
    color: white !important;
}

</style>