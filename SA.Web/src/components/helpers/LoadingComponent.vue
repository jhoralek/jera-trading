<template>
    <div class="dilog">
        <v-dialog v-if="message && open"
                  v-model="open"
                  hide-overlay
                  persistent
                  attach="true"
                  max-width="300">
            <v-card color="blue-grey darken-1"
                    dark>
                <v-card-text>
                    {{ resx(message) }}
                    <v-progress-linear indeterminate
                                       color="white"
                                       class="mb-0">
                    </v-progress-linear>
                </v-card-text>
            </v-card>
        </v-dialog>
    </div>
</template>

<script lang="ts">

import { Component, Prop } from 'vue-property-decorator';

import BaseComponent from '../BaseComponent.vue';

@Component({})
export default class LoadingComponent extends BaseComponent {
    @Prop({default: false})
    public open: boolean;
    @Prop({default: 'loadingData'})
    public message: string;
}

</script>
