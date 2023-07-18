import 'babel-polyfill';
import Vuetify from 'vuetify';
import Vue from 'vue';
import VeeValidate from 'vee-validate';

import moment from 'moment-timezone';
import VueMoment from 'vue-moment';
import 'moment/locale/cs';

import App from './App.vue';
import router from '@/router';
import store from '@/store';

import '@/registerServiceWorker';
import 'vuetify/dist/vuetify.min.css';
import 'vue-wysiwyg/dist/vueWysiwyg.css';
import wysiwyg from 'vue-wysiwyg';
import Lingallery from 'lingallery';

import cs from 'vee-validate/dist/locale/cs';
import en from 'vee-validate/dist/locale/en';
import sk from 'vee-validate/dist/locale/sk';
import de from 'vee-validate/dist/locale/de';
import ru from 'vee-validate/dist/locale/ru';

Vue.config.productionTip = false;
Vue.use(Vuetify);
Vue.use(VueMoment, {
  moment,
});
Vue.use(wysiwyg, {
  hideModules: {
    image: true,
    code: true,
    justifyLeft: true,
    justifyCenter: true,
    justifyRight: true,
    headings: true,
    link: true,
    orderedList: true,
    unorderedList: true,
    table: true,
    separator: true,
    removeFormat: true,
  },
});
Vue.use(VeeValidate, {
  locale: 'cs',
  dictionary: { cs, en, sk, de, ru },
});

Vue.component('lingallery', Lingallery);

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
