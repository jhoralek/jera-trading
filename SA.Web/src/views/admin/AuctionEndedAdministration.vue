<template>
  <div class="auctions-ended">
    <v-container grid-list-xs pa-2>
      <v-layout row wrap>
        <v-flex xs12>
          <h1>{{ resx('endedAuctions') }}</h1>
        </v-flex>
      </v-layout>
      <v-layout row wrap>
        <v-flex xs12>
          <admin-ended-table-component />
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script lang="ts">

import Component from 'vue-class-component';

import BaseView from '../BaseView.vue';
import AdminEndedTableComponent from '@/components/auction/AdminEndedTableComponent.vue';

@Component({
  components: {
    AdminEndedTableComponent,
  },
})
export default class AuctionEndedAdministration extends BaseView {
}

</script>

<style>

.auctions-ended h1 {
  font-family: Roboto;
  font-size: 35px;
  font-weight: 500;
  font-style: normal;
  font-stretch: normal;
  letter-spacing: 0px;
  text-align: left;
  color: #000000;
  padding-bottom: 30px;
  padding-top: 30px;
}

</style>