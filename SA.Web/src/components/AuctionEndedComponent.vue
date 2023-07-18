<template>
  <div class="auction-grid-list">
    <loading-component :open="isLoading" />
    <v-container v-bind="{ [`grid-list-xs`]: true }" fluid v-if="records && records.length > 0">
      <v-layout row wrap>
        <v-flex xs12 sm4 v-for="(record, index) in records" :key="index" class="auction-box">
          <v-card>
              <v-card-media :src="firstImagePath(record)">
              </v-card-media>
              <v-card-title>
                <v-layout row wrap>
                  <v-flex xs8>{{ record.name }}</v-flex>
                  <v-flex xs2 class="text-xs-right">
                    <span v-if="record.registrationYear">{{ record.registrationYear }}</span>
                  </v-flex>
                  <v-flex xs2 class="text-xs-right">
                    <span  v-if="record.fuel">{{ record.fuel }}</span>
                  </v-flex>
                </v-layout>
              </v-card-title>
              <v-card-text>
                <v-layout row wrap>
                  <v-flex xs6>
                    <span>{{ resx('endOfAuction') }}</span>
                  </v-flex>
                  <v-flex xs6 class="text-xs-right">
                    <span v-if="record.numberOfBids > 0">{{ resx('finalPrice') }}</span>
                    <span class="price-with-dph" v-if="record.withVat && record.numberOfBids > 0">{{ resx('withVat') }}</span>
                    <span class="price-with-dph" v-else-if="record.numberOfBids > 0">{{ resx('withoutVat') }}</span>
                  </v-flex>
                </v-layout>
                <v-layout row wrap>
                  <v-flex xs6>{{ resx('endOfAuction') }}</v-flex>
                  <v-flex xs6 class="text-xs-right list-item-price">
                    <price-component
                      v-if="record.numberOfBids > 0"
                      :price="record.currentPrice" />
                  </v-flex>
                </v-layout>
              </v-card-text>
            </v-card>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script lang="ts">

import { Component, Prop } from 'vue-property-decorator';
import { State, Action, Getter, namespace } from 'vuex-class';

import BaseComponent from './BaseComponent.vue';

import { PriceComponent, LoadingComponent } from './../components';

import { Record } from './../model';
import { RecordTableDto } from './../poco';

const RecordAction = namespace('record', Action);
const RecordGetter = namespace('record', Getter);

@Component({
    components: {
        PriceComponent,
        LoadingComponent,
    },
})
export default class AuctionEndedComponent extends BaseComponent {
  @RecordGetter('getEndedRecors') private records: RecordTableDto[];
  @RecordAction('getLatestEndedRecords') private loadRecods: any;

  private isLoading: boolean = false;

  private mounted() {
    this.isLoading = true;
    this.loadRecods(10).then(() => {
        this.isLoading = false;
    });
  }

  private firstImagePath(record: Record): string {
    const { files } = record;
    if (files.length === 0) {
      return '';
    }
    const rf = files[0];
    return `/${rf.path}/${rf.recordId}/images/${rf.name}`;
  }
}

</script>

<style>

.auction-grid-list .item {
  cursor: pointer;
}

.auction-grid-list h3 {
  font-family: Roboto;
  font-size: 20px;
  color: black;
  line-height: 1.4;
  width: 100% !important;
  max-width: 100% !important;
}

.auction-grid-list .v-card {
    --webkit-box-shadow: 0 0px 0px 0px rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0) !important;
    box-shadow: 0 0px 0px 0px rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0) !important;
    border-radius: 5px !important;
    background-color: #ffffff !important;
    border: solid 1px #979797 !important;
}

.auction-grid-list .v-card__media__content {
  width: 347px !important;
  height: 279px !important;
}

.auction-grid-list .v-card__title {
  font-family: Roboto;
  font-size: 25px;
  font-weight: 500;
  font-style: normal;
  font-stretch: normal;
  line-height: 2.12;
  letter-spacing: 0px;
  text-align: left;
  color: #000000;
  padding-bottom: 10px !important;
}

.auction-grid-list .v-card__title span {
  font-size: 10px !important;
  font-weight: bold !important;
  color: #929292 !important;
  text-align: right;
}

.auction-grid-list .auction-box {
  padding: 20px !important;
}

.auction-grid-list .v-card__text {
  background-color: #000000 !important;
  font-family: Roboto !important;
  font-size: 18px !important;
  font-weight: bold !important;
  font-style: normal !important;
  font-stretch: normal !important;
  letter-spacing: 0px !important;
  text-align: left !important;
  color: #ffffff !important;
  padding-top: 0px;
}

.auction-grid-list .v-card__text span {
  font-size: 10px !important;
  letter-spacing: 0.8px !important;
  color: #929292 !important;
}

.auction-grid-list .v-card__media {
  cursor: pointer;
}

.auction-grid-list .list-item-price span {
  font-size: 18px !important;
  color: white !important;
}

.auction-list .v-btn {
  color: white !important;
  border-radius: 5px !important;
  text-transform: lowercase;
}

.auction-list .v-btn .car-text {
  font-size: 10px !important;
  padding-left: 5px !important;
}

.auction-list .v-card__title {
  padding: 0px !important;
}

.auction-list .v-card__title .v-btn {
  padding-left: 15px !important;
  height: 23px !important;
  min-width: 50px !important;
}

.auction-list .v-card__title .v-btn .v-btn__content {
  height: 23px !important;
}

.auction-list .v-card__title h1, .auction-list .v-card__title .v-btn {
  display: inline !important;
}

.auction-list .v-card__title h1 {
  font-family: Roboto;
  font-size: 30px;
  font-weight: 500;
  font-style: normal;
  font-stretch: normal;
  letter-spacing: 0px;
  color: #000000;
  padding-top: 0px !important;
  padding-bottom: 0px !important;
}

.auction-list .v-card__title h3 {
  font-family: Roboto;
  font-size: 16px;
  font-weight: 500;
  font-style: normal;
  font-stretch: normal;
  letter-spacing: 0px;
  color: #000000;
  line-height: 4 !important;
}

.auction-list .v-card {
  border-radius: 0px !important;
  background-color: #ffffff !important;
  border: 0px !important;
  padding-bottom: 20px !important;
}

.auction-box .price-with-dph {
  padding-left: 2px;
  color: #929292;
  font-size: 10px;
  font-weight: bold;
}

</style>
