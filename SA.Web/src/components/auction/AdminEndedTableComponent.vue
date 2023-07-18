<template>
  <div class="admin-ended-table">
    <v-dialog v-model="showCustomerInfo" persistent max-width="500px">
      <v-card v-if="customerInfo !== null">
        <v-card-title class="headline grey lighten-2">
          {{ customerInfo.fullName }}
        </v-card-title>
        <v-layout row wrap>
          <v-flex xs6 class="info-text">{{ resx('phoneNumber') }}</v-flex>
          <v-flex xs6 class="info-value">{{ customerInfo.phoneNumber }}</v-flex>
        </v-layout>
        <v-layout row wrap>
          <v-flex xs6 class="info-text">{{ resx('email') }}</v-flex>
          <v-flex xs6 class="info-value">{{ customerInfo.email }}</v-flex>
        </v-layout>
        <v-layout row wrap>
          <v-flex xs6 class="info-text">{{ resx('companyName') }}</v-flex>
          <v-flex xs6 class="info-value">{{ customerInfo.companyName }}</v-flex>
        </v-layout>
        <v-layout row wrap>
          <v-flex xs6 class="info-text">{{ resx('companyNumber') }}</v-flex>
          <v-flex xs6 class="info-value">{{ customerInfo.companyNumber }}</v-flex>
        </v-layout>
        <v-layout row wrap>
          <v-flex xs12 class="info-text"><strong>{{ resx('address') }}</strong></v-flex>
        </v-layout>
        <v-layout row wrap>
          <v-flex xs6 class="info-text">{{ resx('street') }}</v-flex>
          <v-flex xs6 class="info-value">{{ customerInfo.street }}</v-flex>
        </v-layout>
        <v-layout row wrap>
          <v-flex xs6 class="info-text">{{ resx('city') }}, {{ resx('postCode') }}</v-flex>
          <v-flex xs6 class="info-value">{{ customerInfo.postCode }} {{ customerInfo.city }}</v-flex>
        </v-layout>
        <v-divider></v-divider>
        <v-card-actions>
          <v-container>
            <v-layout row wrap justify-end align-center>
              <v-btn color="black" class="white-btn" @click="disposeCustomerInfo()">{{ resx('close') }}</v-btn>
            </v-layout>
          </v-container>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <loading-component :open="loading" />
    <question-dialog-component
      :header="questionWarning"
      :question="questionMessage"
      :dialog="questionDialog"
      @result="deleteRecord($event)" />
    <v-container  grid-list-xs pa-0>
      <v-layout row wrap>
        <v-flex xs12>
          <v-data-table
            :headers="headers"
            :items="records"
            :pagination.sync="pagination"
            hide-actions
            class="elevation-1">
            <template slot="items" slot-scope="props">
              <td>{{ props.item.name }}</td>
              <td>{{ props.item.auctionName }}</td>
              <td class="text-xs-right">{{ props.item.validFrom | moment('DD.MM.YYYY HH:mm') }}</td>
              <td class="text-xs-right">{{ props.item.validTo | moment('DD.MM.YYYY HH:mm') }}</td>
              <td class="text-xs-right"><price-component :price="props.item.startingPrice" /></td>
              <td class="text-xs-right"><price-component :price="props.item.minimumBid" /></td>
              <td class="text-xs-right"><price-component :price="props.item.currentPrice" /></td>
              <td class="text-xs-right">{{ props.item.numberOfBids }}</td>
              <td class="text-xs-right">
                <a href="#" @click="showCustomer(props.item.winningUserId)">
                  {{ resx('show') }}
                </a>
              </td>
              <td class="text-xs-right">
                  <v-icon
                      style="cursor: pointer"
                      small
                      class="mr-2"
                      @click="wantToDeleteRecord(props.item)">
                      delete
                  </v-icon>
              </td>
            </template>
          </v-data-table>
          <div class="text-xs-center pt-2">
              <v-pagination v-model="pagination.page" :length="pages"></v-pagination>
          </div>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script lang="ts">

import { Component, Prop, Watch } from 'vue-property-decorator';
import { State, Getter, Action, namespace } from 'vuex-class';

import BaseComponent from '../BaseComponent.vue';
import PriceComponent from '@/components/helpers/PriceComponent.vue';
import QuestionDialogComponent from '@/components/helpers/QuestionDialogComponent.vue';
import LoadingComponent from '@/components/helpers/LoadingComponent.vue';

import {
  RecordTableDto,
  FileSimpleDto,
  UserShortDto,
  UserShortInfo,
  UserSimpleDto,
} from './../../poco';

const RecordAction = namespace('record', Action);
const RecordGetter = namespace('record', Getter);
const ProfileAction = namespace('profile', Action);

@Component({
  components: {
    PriceComponent,
    QuestionDialogComponent,
    LoadingComponent,
  },
})
export default class AdminEndedTableComponent extends BaseComponent {
  @RecordGetter('getRecords')
  private records: RecordTableDto[];

  @RecordAction('getAllEndedRecords')
  private loadRecords: any;
  @RecordAction('getDetail')
  private getDetail: any;
  @RecordAction('deleteRecord')
  private delete: any;

  @ProfileAction('getCustomerInfo')
  private getCustomerInfo: any;

  private loading: boolean = false;
  private questionDialog: boolean = false;
  private objectToDelete: RecordTableDto = undefined;
  private showCustomerInfo: boolean = false;
  private customerInfo: UserShortDto = null;

  private headers: any[] = [];
  private pagination: any = {
      rowsPerPage: 10,
      totalItems: 0,
  };

  @Watch('records') private changeUsers(records) {
      if (records !== undefined && records.length > 0) {
          this.pagination.totalItems = records.length;
      }
  }

  private mounted() {
      this.loading = true;
      this.loadRecords().then((response) => {
        this.loading = false;
      });

      this.headers.push({
          text: this.settings.resource.name,
          align: 'left',
          sortable: true,
          value: 'name' });
      this.headers.push({
          text: this.settings.resource.auctions,
          align: 'left',
          sortable: true,
          value: 'auctionName' });
      this.headers.push({
          text: this.settings.resource.validFrom,
          align: 'right',
          sortable: true,
          value: 'validFrom' });
      this.headers.push({
          text: this.settings.resource.validTo,
          align: 'right',
          sortable: true,
          value: 'validTo' });
      this.headers.push({
          text: this.settings.resource.startingPrice,
          align: 'rigth',
          sortable: true,
          value: 'startingPrice' });
      this.headers.push({
          text: this.settings.resource.minimumBid,
          align: 'right',
          sortable: true,
          value: 'minimumBid' });
      this.headers.push({
          text: this.settings.resource.currentPrice,
          align: 'right',
          sortable: true,
          value: 'currentPrice' });
      this.headers.push({
          text: this.settings.resource.numberOfBids,
          align: 'right',
          sortable: false,
          value: 'numberOfBids' });
      this.headers.push({
          text: this.settings.resource.winner,
          align: 'right',
          sortable: false,
          value: 'winner' });
      this.headers.push({
          text: this.settings.resource.action,
          align: 'rigth',
          sortable: true,
          value: 'action' });
  }

  get questionMessage(): string {
    return this.settings.resource.messageDeleteItem;
  }

  get questionWarning(): string {
    return this.settings.resource.warning;
  }

  get pages() {
      if (this.pagination.rowsPerPage == null ||
        this.pagination.totalItems == null) {
            return 0;
      }
      return Math.ceil(this.pagination.totalItems / this.pagination.rowsPerPage);
  }

  private showCustomer(userId: number): void {
    this.loading = true;
    this.getCustomerInfo(userId).then((response) => {
      this.showCustomerInfo = true;
      this.customerInfo = response as UserShortDto;
      this.loading = false;
    });
  }

  private disposeCustomerInfo(): void {
    this.customerInfo = null;
    this.showCustomerInfo = false;
  }

  private wantToDeleteRecord(item: RecordTableDto): void {
    this.questionDialog = true;
    this.objectToDelete = item;
  }

  private deleteRecord(decision: boolean): void {
    if (this.objectToDelete.id > 0 && decision) {
      this.delete(this.objectToDelete.id).then((response) => {
        if (response) {
          this.questionDialog = false;
        }
      });
    } else {
      this.questionDialog = false;
      this.objectToDelete = undefined;
    }
  }
}

</script>

<style>

.admin-ended-table h1 {
  padding-bottom: 0px !important;
}

.admin-ended-table .elevation-1 {
    -webkit-box-shadow: 0 0px 0px 0px rgba(0,0,0,.0),0 0px 0px 0 rgba(0,0,0,.0),0 0px 0px 0 rgba(0,0,0,.0) !important;
    box-shadow: 0 0px 0px 0px rgba(0,0,0,.0),0 0px 0px 0 rgba(0,0,0,.0),0 0px 0px 0 rgba(0,0,0,.0)!important;
}

.admin-ended-table .v-pagination .v-pagination__item--active {
    background-color: #546E7A !important;
}

.admin-ended-table .v-toolbar {
  height: 10px !important;
}

.v-dialog .white-btn .v-btn__content {
    color: white !important;
    border-radius: 5px !important;
}

.v-dialog .v-card {
  padding: 10px !important;
}

.info-text {
  font-size: 10px !important;
  font-weight: bold;
  font-style: normal;
  font-stretch: normal;
  line-height: 3;
  letter-spacing: 0.8px;
  text-align: left;
  color: #929292;
  padding-left: 15px !important;
  text-transform: uppercase;
  padding-right: 15px !important;
}

.info-value {
  font-size: 13px !important;
  font-style: normal;
  font-stretch: normal;
  line-height: 2;
  letter-spacing: 0.8px;
  text-align: right;
  padding-right: 15px !important;
  color: #000000;
}

</style>
