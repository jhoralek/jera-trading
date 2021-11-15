<template>
    <span v-if="price && lang">{{ formatPrice() }}</span>
</template>

<script lang="ts">

import { Vue, Prop, Component } from 'vue-property-decorator';
import { Getter, namespace } from 'vuex-class';
import Helpers from './../../helpers';

const SettingsGetter = namespace('settings', Getter);

@Component({})
export default class PriceComponent extends Vue {
    @Prop({default: 0})
    private price: number;

    @SettingsGetter('getLanguage')
    private lang: string;

    private formatPrice() {
        return Helpers.formatPrice(this.price, this.lang);
    }
}

</script>