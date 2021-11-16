<template>
    <div class="bid-component">
        <question-dialog-component :header="headerMessage"
                                   :question="questionMessage"
                                   :dialog="questionDialog"
                                   @result="submitBid($event)" />
        <v-layout row class="bid-component-field">
            <v-flex xs8 class="text-xs-right">
                <v-text-field type="number"
                              v-model="localBid"
                              v-validate="{
            required: true,
            numeric: true,
            min_value: minValueValidation(this.record.currentPrice)
          }"
                              :error-messages="errors.collect('bid')"
                              data-vv-name="bid"
                              :height="10"
                              outline />
            </v-flex>
            <v-flex xs4>
                <v-btn :loading="loading"
                       :disabled="disable"
                       color="black"
                       @click.native="questionDialog = true">
                    {{ resx("bid") }}
                </v-btn>
            </v-flex>
        </v-layout>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Watch } from 'vue-property-decorator';
    import { Action, Getter, namespace } from 'vuex-class';

    import BaseComponent from '../BaseComponent.vue';
    import QuestionDialogComponent from './QuestionDialogComponent.vue';

    import { Record, Bid } from './../../model';
    import { AuthUser } from './../../poco';

    const RecordAction = namespace('record', Action);
    const RecordGetter = namespace('record', Getter);
    const AuthGetter = namespace('auth', Getter);

    @Component({
        components: {
            QuestionDialogComponent,
        },
    })
    export default class BidComponent extends BaseComponent {
        @Prop({ default: 0 })
        private bid: number;

        @RecordAction('addBid')
        private addBid: any;

        @RecordGetter('getCurrent')
        private record: Record;

        @AuthGetter('getCurrentLoggedUser')
        private user: AuthUser;

        private loading: boolean = false;
        private localBid: number = 0;
        private disable: boolean = false;
        private questionDialog: boolean = false;

        private mounted() {
            this.localBid = this.bid;
        }

        @Watch('localBid')
        private watchBid(newBid: string) {
            if (!/^[0-9]+$/.test(newBid)) {
                this.disable = true;
            } else {
                if (this.record.bids.length === 0 && this.record.currentPrice === parseInt(newBid, 10)) {
                    this.disable = false;
                } else if (
                    parseInt(newBid, 0) <
                    this.record.minimumBid + this.record.currentPrice
                ) {
                    this.disable = true;
                } else {
                    this.disable = false;
                }
                this.localBid = parseInt(newBid, 0);
            }
        }

        @Watch('record.currentPrice')
        private watchPrice(newPrice: number) {
            if (newPrice > this.localBid) {
                this.localBid = newPrice + this.record.minimumBid;
            }
        }

        private minValueValidation(currentPrice: number) {
            if (this.record.bids.length === 0) {
                return this.record.currentPrice;
            } else {
                return this.record.currentPrice + this.record.minimumBid;
            }
        }

        private submitBid(decision: boolean): void {
            // bid was canceled by user
            if (!decision) {
                this.questionDialog = false;
                return;
            }

            this.loading = true;
            this.disable = true;
            const bidPrice = this.localBid;
            this.addBid({
                recordId: this.record.id,
                price: bidPrice,
                created: new Date(),
                userId: this.user.userId,
            } as Bid)
                .then((response) => {
                    this.loading = false;
                    this.disable = false;
                    this.questionDialog = false;

                    this.localBid = (response as number) + this.record.minimumBid;
                });
        }

        get questionMessage(): string {
            return this.settings.resource.bidQuestionMessage
                .replace('{0}', this.localBid.toString());
        }

        get headerMessage(): string {
            return this.settings.resource.warning;
        }
    }
</script>

<style>
    .bid-component-field .v-btn {
        color: white !important;
    }

    .bid-component-field .v-text-field--box input,
    .v-text-field--outline input {
        margin-top: 4px !important;
    }

    .bid-component-field .v-text-field--box .v-input__slot,
    .v-text-field--outline .v-input__slot {
        min-height: 36px !important;
    }

    .bid-component-field button {
        margin: 0px !important;
        border-radius: 5px;
    }

    .bid-component-field .v-input {
        margin-right: 0px !important;
    }
</style>
