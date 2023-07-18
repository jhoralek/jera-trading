<template>
  <div id="app">
    <v-progress-linear v-if="isLoading" :indeterminate="isLoading"></v-progress-linear>
    <v-app v-if="settings.language">
      <nav>
        <main-menu-desktop />
        <main-menu-mobile />
      </nav>
      <v-content>
        <router-view />
        <footer-view />
        <message-component />
      </v-content>
      <cookie-consent>
        <template slot="message">
          <span>{{ resx('cookieConsent') }}</span>
          <router-link class="cookie-consent-link" to="/cookies">{{ resx('showDetails') }}</router-link>
        </template>
        <template slot="button">
          <v-btn white>{{ resx('cookiesAll') }}</v-btn>
          <v-btn white>{{ resx('cookiesOnlyNecessary') }}</v-btn>
        </template>
      </cookie-consent>
    </v-app>
  </div>
</template>

<script lang="ts">

import CookieConsent from 'vue-cookieconsent-component';
import { Component, Vue } from 'vue-property-decorator';
import { State, Action, Getter, namespace } from 'vuex-class';

import { MessageComponent } from './components';

import {
  AuthState,
  ProfileState,
  SettingsState,
  MessageState,
} from './store/types';

import {
  MainMenuDesktop,
  MainMenuMobile,
  FooterView,
} from './views';

import { UserShortInfo } from './poco';

const SettingsGetter = namespace('settings', Getter);
const AuthGetter = namespace('auth', Getter);
const AuthAction = namespace('auth', Action);
const ProfileAction = namespace('profile', Action);
const SettingsAction = namespace('settings', Action);
const MessageAction = namespace('message', Action);

@Component({
  components: {
    MessageComponent,
    CookieConsent,
    FooterView,
    MainMenuDesktop,
    MainMenuMobile,
  },
})
export default class App extends Vue {
  // define states in app root
  @State('message')
  public message: MessageState;
  @State('auth')
  public auth: AuthState;
  @State('profile')
  public profile: ProfileState;
  @State('settings')
  public settings: SettingsState;

  @SettingsGetter('getTranslate')
  public resx: string;

  @AuthAction('initialState')
  public initAuth: any;
  @AuthAction('startTimer')
  public startTimer: any;

  @ProfileAction('loadByToken')
  public loadByToken: any;
  @ProfileAction('initialState')
  public initProfile: any;

  @SettingsAction('initialState')
  public initSettings: any;
  @SettingsAction('changeLanguage')
  public setLang: any;

  @MessageAction('initialState')
  public initMessage: any;

  private isLoading: boolean = false;
  /**
   * When app is mounted, then initialize stores
   * and restore auth when is available
   */
  public mounted() {
    const element = this.$el.parentElement.getElementsByClassName('api')[0];
    const apiUrl = element.getAttribute('data-bind');
    this.isLoading = true;
    this.initMessage().then((response) => {
      this.initSettings(apiUrl).then((settings) => {
        const parent = element.parentNode;
        parent.removeChild(element);
        this.initProfile().then((x) => {
          if (this.auth.timer != null
            && this.auth.secondsToLogout > 0
            && this.auth.token !== undefined
            && this.auth.isAuthenticated) {
            this.startTimer();
          }

          if (this.auth.isAuthenticated) {
              this.setLang(this.profile.user.language);
          } else {
            this.initAuth();
          }
          this.isLoading = false;
        });
      });
    });
  }
}
</script>

<style>

.login-form-button:before {
  height: 100%;
}

/* cookie consent */

.cookie-consent {
  position: fixed;
  z-index: 9999;
  width: 100%;
  padding: 1rem 0;

  display: flex;
  flex-wrap: wrap;
  align-items: center;
  justify-content: center;

  background: #ced4da;

  cursor: pointer;
  color: #fff;
  background: #757575;
}

.cookie-consent {
  left: 0;
  right: 0;
  bottom: 0;
}

.cookie-consent-link {
  color: #fff;
  padding-left: 5px;
}

.cookie-consent-transition {
  transition: transform .75s;
  transition-timing-function: cubic-bezier(.75,0,0,1);
  transform: translateY(100%);
}

</style>